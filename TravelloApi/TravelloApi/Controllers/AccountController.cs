using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TravelloApi.Dto;
using TravelloApi.Helpers;
using TravelloApi.Interfaces;
using TravelloApi.Models;

namespace TravelloApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  [EnableCors("MyPolicy")]
  public class AccountController : ControllerBase
  {
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;
    private readonly IConfiguration configuration;

    public AccountController(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
    {
      this.userRepository = userRepository;
      this.mapper = mapper;
      this.configuration = configuration;
    }
    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp([FromBody] UserDto userDto)
    {
      if (await userRepository.GetUserByName(userDto.UserName) != null)
      {
        return BadRequest("Ошибка! Пользователь найден.");
      }
      var id = Guid.NewGuid();

      var user = mapper.Map<User>(userDto);
      user.Id = id.ToString();

      user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

      if (!userRepository.Add(user))
      {
        ModelState.AddModelError("", "Что-то пошло не так. Попробуйте еще раз.");
        return StatusCode(500, ModelState);
      }

      string token = CreateToken(user);
      return Ok(token);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserDto userDto)
    {
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

    [HttpPut("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] UserDto userDto)
    {
      var user = await userRepository.GetUserByName(userDto.UserName);

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

    [HttpGet("GetInfo"), Authorize]
    public async Task<IActionResult> GetInfo([FromHeader] string Authorization)
    {
      return Ok(DecodeJwtToken(Authorization));
    }

    [HttpPost("RefreshToken")]
    public async Task<ActionResult<string>> RefreshToken([FromHeader] string request)
    {
      var refreshToken = Request.Cookies["refreshToken"];

      var userInfo = DecodeJwtToken(request);
      var userId = userInfo["id"];

      var user = await userRepository.GetUserById(userId);

      if (user == null)
      {
        return BadRequest("Пользователь не найден!");
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

    private void SetRefreshToken(RefreshToken newRefreshToken, User user)
    {
      var cookieOptions = new CookieOptions
      {
        HttpOnly = true,
        Expires = newRefreshToken.Expires,
      };
      Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

      user.TokenExpires = newRefreshToken.Expires;
      user.TokenCreated = newRefreshToken.Created;
      user.RefreshToken = newRefreshToken.Token;

      userRepository.Update(user);
    }

    private string CreateToken(User user)
    {
      List<Claim> claims = new()
            {
                new Claim("userName", user.UserName),
                new Claim("currentTripId", user.CurrentTripId.ToString() ?? string.Empty),
                new Claim("id", user.Id ?? string.Empty),
                new Claim("role", user.Role.ToString()),
            };

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

    // Admin
    // Moderator
    // Organizer
    //
    //
  }
}
