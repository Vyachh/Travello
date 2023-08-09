using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Moq;
using TravelloApi.Controllers;
using TravelloApi.Dto;
using TravelloApi.Enums;
using TravelloApi.Helpers;
using TravelloApi.Interfaces;
using TravelloApi.Models;

namespace TravelloApi.Test.Controller
{
  public class AccountControllerTest
  {
    private AccountController controller;
    private Mock<IUserRepository> userMockRepository;
    private Mock<IMapper> mockMapper;
    private Mock<IConfiguration> mockConfiguration;
    private Mock<IHttpContextAccessor> mockContextAccessor;

    private static string UserToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJ1c2VyTmFtZSI6IlZ5YWNoIiwiY3VycmVudFRyaXBJZCI6IjE3IiwiaWQiOiJlMDI2MDFjOC0yNmFjLTRiNGYtYjliNS0zNjRkNWM5MzhkMWYiLCJyb2xlIjoiQWRtaW4iLCJiaXJ0aGRhdGUiOiIzMS4wNy4yMDAwIDA6MDA6MDAiLCJlbWFpbCI6ImZ1Y2tpbkBtYWlsLnJ1IiwiZXhwIjoxNjkyMDA1Mzg3fQ.rqhUC6LAAo76DIPKTldie70drDbJn6Su-8IlhwyAe_aUQ8CVgwIulN69kkOBE24w-h9xfadQDHwKkwmkBpxEhg";
    private static string Id = "e02601c8-26ac-4b4f-b9b5-364d5c938d1f";

    private User TestUser = new User
    {
      Id = Id,
      UserName = "Vyach",
      CurrentTripId = 17,
      Role = Role.Admin,
      BirthDate = DateTime.Parse("31.07.2000 0:00:00"),
      Email = "fuckin@mail.ru"
    };

    public AccountControllerTest()
    {
      userMockRepository = new Mock<IUserRepository>();
      mockMapper = new Mock<IMapper>();
      mockConfiguration = new Mock<IConfiguration>();
      mockContextAccessor = new Mock<IHttpContextAccessor>();

      controller = new AccountController(userMockRepository.Object, mockMapper.Object, mockConfiguration.Object, mockContextAccessor.Object);
    }

    [Fact]
    public async Task GetInfo_ReturnsOkResult_WithUserInfo()
    {
      // Arrange

      var expectedUserInfo = new Dictionary<string, string>
            {
                {"userName", "Vyach" },
                {"currentTripId", "17" },
                {"id","e02601c8-26ac-4b4f-b9b5-364d5c938d1f" },
                {"role", "Admin" },
                {"birthdate",TestUser.BirthDate.ToString("dd.MM.yyyy H:mm:ss") },
                {"email", "fuckin@mail.ru" },
            };

      userMockRepository.Setup(repo => repo.GetUserById(Id)).ReturnsAsync(TestUser);

      // Act
      var result = await controller.GetInfo(UserToken);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      var userInfo = Assert.IsType<Dictionary<string, string>>(okResult.Value);
      userInfo.Remove("exp");
      Assert.Equal(expectedUserInfo, userInfo);
    }

    [Fact]
    public async Task GetCurrentTrip_ReturnsOkResult_WithCurrentTripId()
    {
      // Arrange
      var expectedCurrentTripId = 17;

      userMockRepository.Setup(repo => repo.GetUserById(Id)).ReturnsAsync(TestUser);

      var controller = new AccountController(userMockRepository.Object, mockMapper.Object, mockConfiguration.Object, mockContextAccessor.Object);

      // Act
      var result = await controller.GetCurrentTrip(Id);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      var currentTripId = Assert.IsType<int>(okResult.Value);

      Assert.Equal(expectedCurrentTripId, currentTripId);
    }

    [Fact]
    public async Task SignUp_ReturnsOkResult_WithToken()
    {
      // Arrange

      var userDto = new UserDto
      {
        UserName = "testuser",
        Password = "testpassword",
      };

      var user = new User
      {
        UserName = userDto.UserName,
      };

      userMockRepository.Setup(repo => repo.GetUserByName(userDto.UserName)).ReturnsAsync((User)null);
      userMockRepository.Setup(repo => repo.Add(It.IsAny<User>())).Returns(true);
      mockMapper.Setup(m => m.Map<User>(userDto)).Returns(user);
      mockConfiguration.Setup(c => c.GetSection("AppSettings:Token").Value).Returns(UserToken);

      // Act
      var result = await controller.SignUp(userDto);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      var resultToken = Assert.IsType<string>(okResult.Value);

      var decodedToken = Token.DecodeJwtToken(resultToken);


      Assert.Equal(userDto.UserName, decodedToken["userName"]);
    }

    [Fact]
    public async Task SignUp_ReturnsBadRequest_WhenUserExists()
    {
      // Arrange

      var existingUserName = "Vyach";
      var existingPassword = "SxeKgu";
      var userDto = new UserDto
      {
        UserName = existingUserName,
        Password = existingPassword
      };

      var existingUser = new User
      {
        UserName = existingUserName,
      };

      userMockRepository.Setup(repo => repo.GetUserByName(existingUserName)).ReturnsAsync(existingUser);


      // Act
      var result = await controller.SignUp(userDto);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal("Ошибка! Пользователь найден.", badRequestResult.Value);
    }

    [Fact]
    public async Task SignUp_ReturnsServerError_WhenUserRepositoryFailsToAddUser()
    {
      // Arrange

      var userDto = new UserDto
      {
        UserName = "testuser",
        Password = "testpassword",
        // Set other properties here...
      };

      var user = new User
      {
        UserName = userDto.UserName,
        // Set other properties here...
      };

      userMockRepository.Setup(repo => repo.GetUserByName(userDto.UserName)).ReturnsAsync((User)null);
      userMockRepository.Setup(repo => repo.Add(It.IsAny<User>())).Returns(false);
      mockMapper.Setup(m => m.Map<User>(userDto)).Returns(user);

      var controller = new AccountController(userMockRepository.Object, mockMapper.Object, mockConfiguration.Object, mockContextAccessor.Object);

      // Act
      var result = await controller.SignUp(userDto);

      // Assert
      var serverErrorResult = Assert.IsType<ObjectResult>(result);
      Assert.Equal(500, serverErrorResult.StatusCode);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOkResultWithToken()
    {
      // Arrange


      var userDto = new UserDto
      {
        UserName = "testuser",
        Password = "testpassword"
      };

      var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

      var user = new User
      {
        UserName = userDto.UserName,
        PasswordHash = hashedPassword
      };

      // Настроим мок репозитория для возвращения тестового пользователя по имени
      userMockRepository.Setup(repo => repo.GetUserByName(userDto.UserName)).ReturnsAsync(user);

      // Act
      var result = await controller.Login(userDto);

      // Assert
      // Проверяем, что возвращается OkObjectResult
      Assert.IsType<OkObjectResult>(result);

      // Получаем токен из результата
      var okResult = (OkObjectResult)result;
      var token = okResult.Value as string;

      // Проверяем, что токен не пустой или null
      Assert.NotNull(token);
      Assert.NotEmpty(token);
    }
  }
}
