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
using TravelloApi.Reposity;

namespace TravelloApi.Test.Controller
{
  public class AccountControllerTest
  {
    private readonly AccountController controller;
    private readonly Mock<IUserRepository> userMockRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<IConfiguration> mockConfiguration;
    private readonly Mock<IHttpContextAccessor> mockContextAccessor;
    private readonly Mock<IRequestCookieCollection> mockRequestCollection;

    private static readonly string UserToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJ1c2VyTmFtZSI6IlZ5YWNoIiwiY3VycmVudFRyaXBJZCI6IjE3IiwiaWQiOiJlMDI2MDFjOC0yNmFjLTRiNGYtYjliNS0zNjRkNWM5MzhkMWYiLCJyb2xlIjoiQWRtaW4iLCJiaXJ0aGRhdGUiOiIzMS4wNy4yMDAwIDA6MDA6MDAiLCJlbWFpbCI6ImZ1Y2tpbkBtYWlsLnJ1IiwiZXhwIjoxNjkyMDA1Mzg3fQ.rqhUC6LAAo76DIPKTldie70drDbJn6Su-8IlhwyAe_aUQ8CVgwIulN69kkOBE24w-h9xfadQDHwKkwmkBpxEhg";
    private static readonly string Id = "e02601c8-26ac-4b4f-b9b5-364d5c938d1f";

    private readonly User TestUser = new()
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
      mockRequestCollection = new Mock<IRequestCookieCollection>();

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

      mockConfiguration.Setup(c => c.GetSection("AppSettings:Token").Value).Returns(UserToken);
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

      mockConfiguration.Setup(c => c.GetSection("AppSettings:Token").Value).Returns(UserToken);
      mockContextAccessor.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());
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

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsBadRequest()
    {
      // Arrange
      var userDto = new UserDto
      {
        UserName = "testuser",
        Password = "incorrectpassword"
      };

      var hashedPassword = BCrypt.Net.BCrypt.HashPassword("testpassword");

      var user = new User
      {
        UserName = "testuser",
        PasswordHash = hashedPassword
      };

      mockConfiguration.Setup(c => c.GetSection("AppSettings:Token").Value).Returns(UserToken);
      userMockRepository.Setup(repo => repo.GetUserByName(userDto.UserName)).ReturnsAsync(user);

      // Act
      var result = await controller.Login(userDto);

      // Assert
      // Проверяем, что возвращается BadRequestObjectResult
      Assert.IsType<BadRequestObjectResult>(result);

      // Получаем текст ошибки из результата
      var badRequestResult = (BadRequestObjectResult)result;
      var errorMessage = badRequestResult.Value as string;

      // Проверяем, что текст ошибки не пустой или null
      Assert.NotNull(errorMessage);
      Assert.NotEmpty(errorMessage);
    }

    [Fact]
    public async Task RefreshToken_WithValidRefreshToken_ReturnsOkResultWithNewToken()
    {
      // Arrange
      var userId = Id;
      var refreshTokenValue = "3IjA6c5+KDfGGOFmWU4BAc/y+QQT+hctLNBqYNrtXP34qJlmKw7qX6GcirgrZ1w6BESB4FSLYOXb4sw/UTCyVg=="; // Provide a valid refresh token value
      var requestHeader = UserToken; // Provide a valid JWT token in the request header

      var user = new User
      {
        Id = userId,
        RefreshToken = refreshTokenValue,
        TokenExpires = DateTime.Now.AddHours(1) // Assume the token is still valid for 1 hour
      };

      mockConfiguration.Setup(c => c.GetSection("AppSettings:Token").Value).Returns(UserToken);

      var mockHttpContext = new Mock<HttpContext>();
      var mockResponse = new Mock<HttpResponse>();
      var mockRequest = new Mock<HttpRequest>();
      var mockResponseCookies = new Mock<IResponseCookies>();
      mockHttpContext.Setup(c => c.Response).Returns(mockResponse.Object);
      mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);
      mockResponse.Setup(r => r.Cookies).Returns(mockResponseCookies.Object);
      userMockRepository.Setup(u => u.GetUserById(userId)).ReturnsAsync(user);

      var contextAccessor = new Mock<IHttpContextAccessor>();
      contextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

      // Set the refreshToken in cookies
      mockRequest.Setup(r => r.Cookies["refreshToken"]).Returns(refreshTokenValue);

      // Now you can inject the contextAccessor into your controller

      var controller = new AccountController(userMockRepository.Object, mockMapper.Object, mockConfiguration.Object, contextAccessor.Object);

      // Act
      var result = await controller.RefreshToken(requestHeader);

      // Assert
      // Проверяем, что возвращается OkObjectResult
      Assert.IsType<OkObjectResult>(result.Result);

      // Получаем токен из результата
      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      var token = okResult.Value as string;

      // Проверяем, что токен не пустой или null
      Assert.NotNull(token);
      Assert.NotEmpty(token);
    }

    [Fact]
    public async Task ChangeInfo_WithValidData_ReturnsOkResultWithToken()
    {
      // Arrange
      var userDto = new UserInfoDto
      {
        UserName = "Vyach",
        BirthDate = DateTime.Parse("31.07.1995"),
        Email = "newemail@mail.com",
        Role = "User"
      };

      mockMapper.Setup(m => m.Map<User>(It.IsAny<UserInfoDto>())).Returns(TestUser);
      userMockRepository.Setup(repo => repo.GetUserByName(userDto.UserName)).ReturnsAsync(TestUser);
      userMockRepository.Setup(repo => repo.Update(It.IsAny<User>())).Returns(true);
      mockConfiguration.Setup(c => c.GetSection("AppSettings:Token").Value).Returns(UserToken);

      // Act
      var result = await controller.ChangeInfo(userDto);

      // Assert
      // Проверяем, что возвращается OkObjectResult
      Assert.IsType<OkObjectResult>(result);

      // Получаем токен из результата
      var okResult = Assert.IsType<OkObjectResult>(result);
      var token = okResult.Value as string;

      // Проверяем, что токен не пустой или null
      Assert.NotNull(token);
      Assert.NotEmpty(token);
    }

    [Fact]
    public async Task ChangeInfo_WithInvalidData_ReturnsBadRequest()
    {
      // Arrange
      var userDto = new UserInfoDto
      {
        UserName = "NonExistentUser",
        BirthDate = DateTime.Parse("31.07.1995"),
        Email = "newemail@mail.com",
        Role = "User"
      };


      userMockRepository.Setup(repo => repo.GetUserByName(userDto.UserName)).ReturnsAsync((User)null);
      mockConfiguration.Setup(c => c.GetSection("AppSettings:Token").Value).Returns(UserToken);


      // Act
      var result = await controller.ChangeInfo(userDto);

      // Assert
      // Проверяем, что возвращается BadRequestObjectResult
      Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task ChangeInfo_UpdateFails_ReturnsInternalServerError()
    {
      // Arrange
      var userDto = new UserInfoDto
      {
        UserName = "Vyach",
        BirthDate = DateTime.Parse("31.07.1995"),
        Email = "newemail@mail.com",
        Role = "User"
      };

      mockMapper.Setup(m => m.Map<User>(It.IsAny<UserInfoDto>())).Returns(TestUser);
      userMockRepository.Setup(repo => repo.GetUserByName(userDto.UserName)).ReturnsAsync(TestUser);
      userMockRepository.Setup(repo => repo.Update(It.IsAny<User>())).Returns(false);
      mockConfiguration.Setup(c => c.GetSection("AppSettings:Token").Value).Returns(UserToken);

      // Act
      var result = await controller.ChangeInfo(userDto);

      // Assert
      // Проверяем, что возвращается StatusCodeResult с кодом 500
      var statusCodeResult = Assert.IsType<ObjectResult>(result);
      Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task SetCurrentTrip_WithValidData_ReturnsOkResult()
    {
      // Arrange
      var userDtos = new[]
      {
        new UserInfoDto
        {
            UserName = "User1",
            CurrentTripId = 1
        },
        new UserInfoDto
        {
            UserName = "User2",
            CurrentTripId = 2
        }
        // Add more userDtos if needed
    };

      var users = new List<User>
    {
        new User
        {
            UserName = "User1"
        },
        new User
        {
            UserName = "User2"
        }
        // Add more users if needed
    };

      foreach (var user in users)
      {
        userMockRepository.Setup(repo => repo.GetUserByName(user.UserName)).ReturnsAsync(user);
      }

      userMockRepository.Setup(repo => repo.Update(It.IsAny<User>())).Returns(true);

      // Act
      var result = await controller.SetCurrentTrip(userDtos);

      // Assert
      // Проверяем, что возвращается OkResult
      Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task SetCurrentTrip_UpdateFails_ReturnsBadRequest()
    {
      // Arrange
      var userDtos = new[]
      {
        new UserInfoDto
        {
            UserName = "User1",
            CurrentTripId = 1
        },
        new UserInfoDto
        {
            UserName = "User2",
            CurrentTripId = 2
        }
    };

      var users = new List<User>
    {
        new User
        {
            UserName = "User1"
        },
        new User
        {
            UserName = "User2"
        }
    };

      foreach (var user in users)
      {
        userMockRepository.Setup(repo => repo.GetUserByName(user.UserName)).ReturnsAsync(user);
      }

      userMockRepository.Setup(repo => repo.Update(It.IsAny<User>())).Returns(false);

      // Act
      var result = await controller.SetCurrentTrip(userDtos);

      // Assert
      // Проверяем, что возвращается BadRequestResult
      Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task ChangePassword_ValidData_ReturnsOkResult()
    {
      // Arrange
      var userDto = new UserDto
      {
        UserName = "Vyach",
        Password = "newpassword"
      };

      var existingUser = new User
      {
        Id = "e02601c8-26ac-4b4f-b9b5-364d5c938d1f",
        UserName = "Vyach",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("oldpassword")
      };

      userMockRepository.Setup(repo => repo.GetUserByName(userDto.UserName))
                         .ReturnsAsync(existingUser);

      userMockRepository.Setup(repo => repo.Update(existingUser))
                         .Returns(true);
      mockConfiguration.Setup(c => c.GetSection("AppSettings:Token").Value)
                         .Returns(UserToken);

      // Act
      var result = await controller.ChangePassword(userDto);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      var token = Assert.IsType<string>(okResult.Value);
      Assert.NotNull(token);
    }

    [Fact]
    public async Task ChangePassword_InvalidUserName_ReturnsBadRequest()
    {
      // Arrange
      var userDto = new UserDto
      {
        UserName = "NonExistentUser",
        Password = "newpassword"
      };

      userMockRepository.Setup(repo => repo.GetUserByName(userDto.UserName))
                         .ReturnsAsync((User)null);
      mockConfiguration.Setup(c => c.GetSection("AppSettings:Token").Value).Returns(UserToken);

      // Act
      var result = await controller.ChangePassword(userDto);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal("User not found.", badRequestResult.Value);
    }

    [Fact]
    public async Task ChangePassword_UpdateFailed_ReturnsInternalServerError()
    {
      // Arrange
      var userDto = new UserDto
      {
        UserName = "Vyach",
        Password = "newpassword"
      };

      var existingUser = new User
      {
        Id = "e02601c8-26ac-4b4f-b9b5-364d5c938d1f",
        UserName = "Vyach",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("oldpassword")
      };

      userMockRepository.Setup(repo => repo.GetUserByName(userDto.UserName))
                         .ReturnsAsync(existingUser);

      userMockRepository.Setup(repo => repo.Update(existingUser))
                         .Returns(false);
      mockConfiguration.Setup(c => c.GetSection("AppSettings:Token").Value)
        .Returns(UserToken);

      // Act
      var result = await controller.ChangePassword(userDto);

      // Assert
      var statusCodeResult = Assert.IsType<ObjectResult>(result);
      Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task SetRole_ValidData_ReturnsOkResult()
    {
      // Arrange
      const int roleId = 2;
      const string userId = "e02601c8-26ac-4b4f-b9b5-364d5c938d1f";

      var existingUser = new User
      {
        Id = userId,
        UserName = "Vyach",
        Role = Role.User
      };

      userMockRepository.Setup(repo => repo.GetUserById(userId))
                         .ReturnsAsync(existingUser);

      userMockRepository.Setup(repo => repo.Save())
                         .Returns(true);
      mockConfiguration.Setup(c => c.GetSection("AppSettings:Token").Value)
                        .Returns(UserToken);
      // Act
      var result = await controller.SetRole(roleId, userId);

      // Assert
      Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task SetRole_UserNotFound_ReturnsBadRequest()
    {
      // Arrange
      const int roleId = 2;
      const string userId = "e02601c8-26ac-4b4f-b9b5-364d5c938d1f";

      userMockRepository.Setup(repo => repo.GetUserById(userId))
                         .ReturnsAsync((User)null);
      mockConfiguration.Setup(c => c.GetSection("AppSettings:Token").Value)
                         .Returns(UserToken);
      // Act
      var result = await controller.SetRole(roleId, userId);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal("User Not Found.", badRequestResult.Value);
    }

    [Fact]
    public async Task SetRole_SaveFailed_ReturnsBadRequest()
    {
      // Arrange
      const int roleId = 2;
      const string userId = "e02601c8-26ac-4b4f-b9b5-364d5c938d1f";

      var existingUser = new User
      {
        Id = userId,
        UserName = "Vyach",
        Role = Role.User
      };

      userMockRepository.Setup(repo => repo.GetUserById(userId))
                         .ReturnsAsync(existingUser);

      userMockRepository.Setup(repo => repo.Save())
                         .Returns(false);

      // Act
      var result = await controller.SetRole(roleId, userId);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal("Unable to set role.", badRequestResult.Value);
    }



  }
}
