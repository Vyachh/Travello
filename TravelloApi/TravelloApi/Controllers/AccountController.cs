using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TravelloApi.Dto;
using TravelloApi.Enums;
using TravelloApi.Interfaces;
using TravelloApi.Models;

namespace TravelloApi.Controllers
{
  /// <summary>
  /// Контроллер для управления данными пользователей.
  /// </summary>
  [ApiController]
  [Route("[controller]")]
  public class AccountController : ControllerBase
  {
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;
    private readonly IConfiguration configuration;
    private readonly IHttpContextAccessor contextAccessor;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера для управления данными пользователей.
    /// </summary>
    /// <param name="userRepository">Репозиторий для работы с данными о пользователях.</param>
    /// <param name="mapper">Объект для маппинга данных между сущностями и DTO.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    public AccountController(IUserRepository userRepository, IMapper mapper, IConfiguration configuration, IHttpContextAccessor contextAccessor)
    {
      this.userRepository = userRepository;
      this.mapper = mapper;
      this.configuration = configuration;
      this.contextAccessor = contextAccessor;
    }

    /// <summary>
    /// Получает информацию о пользователе на основе JWT-токена.
    /// </summary>
    /// <param name="Authorization">JWT-токен, предоставленный клиентом в заголовке Authorization.</param>
    /// <returns>Информация о пользователе из токена.</returns>
    [HttpGet("GetInfo"), Authorize]
    public async Task<IActionResult> GetInfo([FromHeader] string Authorization)
    {
      return Ok(DecodeJwtToken(Authorization));
    }

    /// <summary>
    /// Получает текущий поездку пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Идентификатор текущей поездки пользователя.</returns>
    [HttpGet("GetCurrentTrip"), Authorize]
    public async Task<IActionResult> GetCurrentTrip([FromQuery] string id)
    {
      var user = await userRepository.GetUserById(id);

      return Ok(user.CurrentTripId);
    }

    /// <summary>
    /// Получает список всех пользователей.
    /// </summary>
    /// <returns>Список всех пользователей.</returns>
    [HttpGet("GetAll"), Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> GetAll()
    {
      return Ok(userRepository.GetAll());
    }

    [HttpGet("HasTokenExpired")]
    public IActionResult HasTokenExpired(string token)
    {
      var decodedToken = DecodeJwtToken(token);
      if (!decodedToken.ContainsKey("exp"))
      {
        return BadRequest("Invalid Token.");
      }

      long expireDate = Convert.ToInt64(decodedToken["exp"]);

      DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(expireDate);

      // Convert DateTimeOffset to DateTime if needed
      DateTime dateTime = dateTimeOffset.LocalDateTime;

      if (dateTime.Date < DateTime.Now)
      {
        return Ok(false);
      }

      return Ok(true);
    }

    [HttpGet("GetTokenExpiresDate")]
    public IActionResult GetTokenExpiresDate(string token)
    {
      var decodedToken = DecodeJwtToken(token);
      if (!decodedToken.ContainsKey("exp"))
      {
        return BadRequest("Invalid Token.");
      }

      long expireDate = Convert.ToInt64(decodedToken["exp"]);

      DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(expireDate);

      // Convert DateTimeOffset to DateTime if needed
      DateTime dateTime = dateTimeOffset.LocalDateTime;


      return Ok(dateTime);
    }

    /// <summary>
    /// Получает количество пользователей, участвующих в текущей поездке.
    /// </summary>
    /// <returns>Количество пользователей, участвующих в текущей поездке.</returns>
    [HttpGet("GetOngoingCount"), Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> GetOngoingPeopleCount()
    {
      var result = await userRepository.GetOngoingPeopleCount();
      return Ok(result);
    }

    /// <summary>
    /// Регистрирует нового пользователя.
    /// </summary>
    /// <param name="userDto">Данные нового пользователя.</param>
    /// <returns>Токен, предоставляющий доступ к приложению для зарегистрированного пользователя.</returns>
    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp([FromBody] UserDto userDto)
    {
      if (await userRepository.GetUserByName(userDto.UserName) == null)
      {
        return BadRequest("Ошибка! Пользователь найден.");
      }
      var id = Guid.NewGuid();

      var user = mapper.Map<User>(userDto);
      user.Id = id.ToString();

      user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

      Photo photo = new()
      {
        IdentityId = user.Id,
        Name = "",
        FileType = FileType.AvatarImage
      };

      user.Photo = photo;

      if (!userRepository.Add(user))
      {
        ModelState.AddModelError("", "Что-то пошло не так. Попробуйте еще раз.");
        return StatusCode(500, ModelState);
      }

      string token = CreateToken(user);
      return Ok(token);
    }

    /// <summary>
    /// Авторизует пользователя на основе предоставленных учетных данных.
    /// </summary>
    /// <param name="userDto">Данные пользователя для аутентификации.</param>
    /// <returns>Токен, предоставляющий доступ к приложению для аутентифицированного пользователя.</returns>
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserDto userDto)
    {
      if (userDto.UserName == string.Empty || userDto.Password == string.Empty)
      {
        return BadRequest("Incorrect data");
      }
      var user = await userRepository.GetUserByName(userDto.UserName);

      if (userDto.UserName != user.UserName)
      {
        return BadRequest("User not found.");
      }
      if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
      {
        return BadRequest("Bad password.");
      }


      var refreshToken = GenerateRefreshToken();
      SetRefreshToken(refreshToken, user);
      string token = CreateToken(user);

      return Ok(token);
    }

    /// <summary>
    /// Обновляет существующий JWT-токен с помощью Refresh Token.
    /// </summary>
    /// <param name="request">Заголовок запроса с JWT-токеном.</param>
    /// <returns>Обновленный JWT-токен.</returns>
    [HttpPost("RefreshToken"), Authorize]
    public async Task<ActionResult<string>> RefreshToken([FromHeader] string request)
    {
      var refreshToken = contextAccessor.HttpContext.Request.Cookies["refreshToken"];

      var userInfo = DecodeJwtToken(request);
      var userId = userInfo["id"];

      var user = await userRepository.GetUserById(userId);

      if (user == null)
      {
        return BadRequest("User not found.");
      }

      if (!user.RefreshToken.Equals(refreshToken))
      {
        return Unauthorized("Invalid Refresh Token");
      }
      else if (user.TokenExpires < DateTime.Now)
      {
        return Unauthorized("Token expired.");
      }
      string token = CreateToken(user);
      var newRefreshToken = GenerateRefreshToken();
      SetRefreshToken(newRefreshToken, user);

      return Ok(token);
    }

    /// <summary>
    /// Изменяет информацию о пользователе.
    /// </summary>
    /// <param name="userDto">Данные пользователя для обновления.</param>
    /// <returns>Обновленный токен для пользователя.</returns>
    [HttpPut("ChangeInfo"), Authorize]
    public async Task<IActionResult> ChangeInfo([FromBody] UserInfoDto userDto)
    {
      var user = await userRepository.GetUserByName(userDto.UserName);

      if (user == null)
      {
        return BadRequest("User not found.");
      }

      if (user.UserName != user.UserName)
      {
        return BadRequest("User not found.");
      }

      user.BirthDate = userDto.BirthDate;
      user.Email = userDto.Email;
      user.Role = Enum.Parse<Role>(userDto.Role);

      if (!userRepository.Update(user))
      {
        ModelState.AddModelError("", "Что-то пошло не так. Попробуйте еще раз.");
        return base.StatusCode(500, ModelState);
      }

      string token = CreateToken(user);
      return Ok(token);
    }

    /// <summary>
    /// Устанавливает текущую поездку для указанных пользователей.
    /// </summary>
    /// <param name="userDto">Массив данных о пользователях и их текущих поездках.</param>
    /// <returns>Результат выполнения операции.</returns>
    [HttpPut("SetCurrentTrip"), Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> SetCurrentTrip([FromBody] UserInfoDto[] userDto)
    {

      User user = new();
      foreach (var item in userDto)
      {
        user = await userRepository.GetUserByName(item.UserName);
        user.CurrentTripId = item.CurrentTripId;
      }
      if (!userRepository.Update(user))
      {
        return BadRequest();
      }

      return Ok();

    }

    /// <summary>
    /// Изменяет пароль пользователя.
    /// </summary>
    /// <param name="userDto">Данные пользователя для изменения пароля.</param>
    /// <returns>Обновленный токен для пользователя.</returns>
    [HttpPut("ChangePassword"), Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] UserDto userDto)
    {
      var user = await userRepository.GetUserByName(userDto.UserName);

      if (user == null)
      {
        return BadRequest("User not found.");

      }

      if (userDto.UserName != user.UserName)
      {
        return BadRequest("User not found.");
      }
      user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

      if (!userRepository.Update(user))
      {
        ModelState.AddModelError("", "Что-то пошло не так. Попробуйте еще раз.");
        return StatusCode(500, ModelState);
      }

      string token = CreateToken(user);
      return Ok(token);
    }

    /// <summary>
    /// Устанавливает роль для указанного пользователя.
    /// </summary>
    /// <param name="roleId">Идентификатор роли.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Результат выполнения операции.</returns>
    [HttpPut("SetRole"), Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> SetRole([FromQuery] int roleId, string userId)
    {
      var user = await userRepository.GetUserById(userId);
      if (user == null)
      {
        return BadRequest("User Not Found.");
      }

      user.Role = (Role)roleId;

      if (!userRepository.Save())
      {
        return BadRequest("Unable to set role.");
      }

      return Ok();
    }

    /// <summary>
    /// Генерирует объект обновления Refresh Token.
    /// </summary>
    /// <returns>Объект Refresh Token.</returns>
    private RefreshToken GenerateRefreshToken()
    {
      var refreshToken = new RefreshToken
      {
        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
        Expires = DateTime.Now.AddDays(7),
        Created = DateTime.Now,
      };
      return refreshToken;
    }

    /// <summary>
    /// Устанавливает Refresh Token для пользователя и создает соответствующий HTTP-кукис.
    /// </summary>
    /// <param name="newRefreshToken">Новый Refresh Token.</param>
    /// <param name="user">Пользователь, для которого устанавливается Refresh Token.</param>
    private void SetRefreshToken(RefreshToken newRefreshToken, User user)
    {
      var cookieOptions = new CookieOptions
      {
        HttpOnly = true,
        Expires = newRefreshToken.Expires,
      };
      contextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

      user.TokenExpires = newRefreshToken.Expires;
      user.TokenCreated = newRefreshToken.Created;
      user.RefreshToken = newRefreshToken.Token;

      userRepository.Update(user);
    }

    /// <summary>
    /// Создает JWT-токен на основе данных пользователя.
    /// </summary>
    /// <param name="user">Пользователь, для которого создается токен.</param>
    private string CreateToken(User user)
    {
      List<Claim> claims = new()
            {
                new Claim("userName", user.UserName),
                new Claim("currentTripId", user.CurrentTripId.ToString() ?? string.Empty),
                new Claim("id", user.Id ?? string.Empty),
                new Claim("role", user.Role.ToString()),
                new Claim("birthdate", user.BirthDate.ToString()),
                new Claim("email", user.Email.ToString()),
            };

      //var tokenSettings = configuration.GetSection("AppSettings:Token").Value;
      //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings));
      var key = new SymmetricSecurityKey(Encoding.UTF8
        .GetBytes(configuration
        .GetSection("AppSettings:Token").Value!));

      var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var token = new JwtSecurityToken(
          claims: claims,
          expires: DateTime.UtcNow.AddDays(7),
          signingCredentials: cred
          );
      var result = new JwtSecurityTokenHandler().WriteToken(token);

      return result;

    }

    /// <summary>
    /// Декодирует JWT-токен и возвращает его содержимое как словарь пар "тип-значение".
    /// </summary>
    /// <param name="token">JWT-токен для декодирования.</param>
    /// <returns>Словарь, содержащий пары "тип-значение" из JWT-токена.</returns>
    private static IDictionary<string, string> DecodeJwtToken(string token)
    {

      var jwtHandler = new JwtSecurityTokenHandler();
      var middle = token.Replace("bearer ", "");
      var jwtToken = jwtHandler.ReadJwtToken(middle);

      var claims = new Dictionary<string, string>();
      foreach (var claim in jwtToken.Claims)
      {
        claims.Add(claim.Type, claim.Value);
      }

      return claims;
    }

  }
}
