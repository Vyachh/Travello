using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using TravelloApi.Dto;
using TravelloApi.Helpers;
using TravelloApi.Interfaces;
using TravelloApi.Models;

namespace TravelloApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
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

      string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

      user.PasswordHash = passwordHash;

      if (!userRepository.Add(user))
      {
        ModelState.AddModelError("", "Что-то пошло не так. Попробуйте еще раз.");
        return StatusCode(500, ModelState);
      }

      return Ok("Пользователь успешно добавлен!");
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

      string token = Token.CreateToken(user, configuration);
      return Ok(token);
    }
  }
}
