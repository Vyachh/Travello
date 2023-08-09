using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TravelloApi.Controllers;
using TravelloApi.Dto;
using TravelloApi.Interfaces;
using TravelloApi.Models;

namespace TravelloApi.Test.Controller
{
  public class TripControllerTest
  {
    private readonly TripController controller;
    private readonly Mock<ITripRepository> tripMockRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<IUserRepository> mockUserRepository;
    private readonly Mock<IHttpContextAccessor> mockContextAccessor;

    public TripControllerTest()
    {
      tripMockRepository = new Mock<ITripRepository>();
      mockMapper = new Mock<IMapper>();
      mockUserRepository = new Mock<IUserRepository>();
      mockContextAccessor = new Mock<IHttpContextAccessor>();

      controller = new TripController(
          tripMockRepository.Object,
          mockUserRepository.Object,
          mockMapper.Object,
          mockContextAccessor.Object
      );
    }

    [Fact]
    public async Task GetTrip_ExistingTrip_ReturnsOkResult()
    {
      // Arrange
      const int tripId = 17;
      var existingTrip = new Trip
      {
        Id = tripId,
        // Initialize other properties as needed
      };

      tripMockRepository.Setup(repo => repo.GetById(tripId))
                         .ReturnsAsync(existingTrip);

      // Act
      var result = await controller.GetTrip(id: tripId);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      var trip = Assert.IsType<Trip>(okResult.Value);
      Assert.Equal(tripId, trip.Id);
    }

    [Fact]
    public async Task GetTrip_NonExistentTrip_ReturnsNotFoundResult()
    {
      // Arrange
      const int nonExistentTripId = 456;

      tripMockRepository.Setup(repo => repo.GetById(nonExistentTripId))
                         .ReturnsAsync((Trip)null);

      // Act
      var result = await controller.GetTrip(id: nonExistentTripId);

      // Assert
      Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task SetOngoingTrip_ValidData_ReturnsOkResult()
    {
      // Arrange
      const int tripId = 123;

      tripMockRepository.Setup(repo => repo.SetOngoingTrip(tripId))
                         .Returns(true);

      // Act
      var result = await controller.SetOngoingTrip(id: tripId);

      // Assert
      Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task SetOngoingTrip_SettingFailed_ReturnsBadRequest()
    {
      // Arrange
      const int tripId = 123;

      tripMockRepository.Setup(repo => repo.SetOngoingTrip(tripId))
                         .Returns(false);

      // Act
      var result = await controller.SetOngoingTrip(id: tripId);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal("Unable to set ongoing trip.", badRequestResult.Value);
    }

    [Fact]
    public async Task SetNextTrip_ValidData_ReturnsOkResult()
    {
      // Arrange
      const int tripId = 17;

      tripMockRepository.Setup(repo => repo.SetOngoingTrip(tripId))
                         .Returns(true);

      // Act
      var result = await controller.SetOngoingTrip(id: tripId);

      // Assert
      Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task SetNextTrip_SettingFailed_ReturnsBadRequest()
    {
      // Arrange
      const int tripId = 123;

      tripMockRepository.Setup(repo => repo.SetOngoingTrip(tripId))
                         .Returns(false);

      // Act
      var result = await controller.SetOngoingTrip(id: tripId);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal("Unable to set ongoing trip.", badRequestResult.Value);
    }

    [Fact]
    public async Task AddTrip_ValidData_ReturnsOkResult()
    {
      // Arrange
      var tripDto = new TripDto
      {
        Id = 0,
        UserId = "testUserId",
        Title = "Test Trip",
        Description = "Test Description",
        Price = 100,
        DateFrom = DateTime.UtcNow,
        DateTo = DateTime.UtcNow.AddDays(7),
      };

      var fakeFormFile = Mock.Of<IFormFile>(f =>
    f.FileName == "test.jpg" &&
    f.Length == 1234 // Set the appropriate length
);

      tripDto.Image = fakeFormFile;

      var trip = new Trip
      {
        Id = 0,
        UserId = "testUserId",
        Title = "Test Trip",
        Description = "Test Description",
        Price = 100,
        DateFrom = DateTime.UtcNow,
        DateTo = DateTime.UtcNow.AddDays(7),
        ImageUrl = ""

      };

      var expectedAuthor = "Test User";
      var expectedImageUrl = "testImageUrl";

      mockUserRepository.Setup(repo => repo.GetUserName(tripDto.UserId))
                         .ReturnsAsync(expectedAuthor);
      tripMockRepository.Setup(repo => repo.Add(trip))
        .Returns(true);

      mockMapper.Setup(m => m.Map<Trip>(tripDto))
        .Returns(trip);

      controller.ControllerContext = new ControllerContext();
      controller.ControllerContext.HttpContext = new DefaultHttpContext();

      // Act
      var result = await controller.AddTrip(tripDto);

      // Assert
      Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task AddTrip_AddingFailed_ReturnsBadRequest()
    {
      // Arrange
      var tripDto = new TripDto
      {
        // Populate other properties as needed
      };

      mockUserRepository.Setup(repo => repo.GetUserName(tripDto.UserId))
                         .ReturnsAsync("Test User");

      tripMockRepository.Setup(repo => repo.Add(It.IsAny<Trip>()))
                         .Returns(false);

      controller.ControllerContext = new ControllerContext();
      controller.ControllerContext.HttpContext = new DefaultHttpContext();

      // Act
      var result = await controller.AddTrip(tripDto);

      // Assert
      Assert.IsType<StatusCodeResult>(result);

    }

    [Fact]
    public async Task Approve_ExistingTrip_ReturnsOkResult()
    {
      // Arrange
      const int tripId = 123;
      var existingTrip = new Trip
      {
        Id = tripId,
        IsApproved = false
      };

      tripMockRepository.Setup(repo => repo.GetById(tripId))
                         .ReturnsAsync(existingTrip);

      tripMockRepository.Setup(repo => repo.Save())
                         .Returns(true);

      // Act
      var result = await controller.Approve(id: tripId);

      // Assert
      var okResult = Assert.IsType<OkResult>(result);
      Assert.True(existingTrip.IsApproved);
    }

    [Fact]
    public async Task Approve_NonExistentTrip_ReturnsBadRequest()
    {
      // Arrange
      const int nonExistentTripId = 456;

      tripMockRepository.Setup(repo => repo.GetById(nonExistentTripId))
                         .ReturnsAsync((Trip)null);

      // Act
      var result = await controller.Approve(id: nonExistentTripId);

      // Assert
      Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Approve_SaveFailed_ReturnsInternalServerError()
    {
      // Arrange
      const int tripId = 123;
      var existingTrip = new Trip
      {
        Id = tripId,
        IsApproved = false
      };

      tripMockRepository.Setup(repo => repo.GetById(tripId))
                         .ReturnsAsync(existingTrip);

      tripMockRepository.Setup(repo => repo.Save())
                         .Returns(false);

      // Act
      var result = await controller.Approve(id: tripId);

      // Assert
      var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
      Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task Update_ImageAddingFailed_ReturnsBadRequest()
    {
      // Arrange
      var tripDto = new TripDto
      {
        Id = 0,
        UserId = "testUserId",
        Title = "Test Trip",
        Description = "Test Description",
        Price = 100,
        DateFrom = DateTime.UtcNow,
        DateTo = DateTime.UtcNow.AddDays(7),
      };

      var fakeFormFile = Mock.Of<IFormFile>(f =>
          f.FileName == "test.jpg"

      );

      tripDto.Image = fakeFormFile;

      mockMapper.Setup(m => m.Map<Trip>(tripDto))
                 .Returns(new Trip());

      controller.ControllerContext = new ControllerContext();
      controller.ControllerContext.HttpContext = new DefaultHttpContext();

      // Act
      var result = await controller.Update(tripDto);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal("Unable to upload photo.", badRequestResult.Value);
    }

    [Fact]
    public async Task Update_TripUpdateFailed_ReturnsBadRequest()
    {
      // Arrange
      var tripDto = new TripDto
      {
        Id = 0,
        UserId = "testUserId",
        Title = "Test Trip",
        Description = "Test Description",
        Price = 100,
        DateFrom = DateTime.UtcNow,
        DateTo = DateTime.UtcNow.AddDays(7),
      };

      var fakeFormFile = Mock.Of<IFormFile>(f =>
          f.FileName == "test.jpg" &&
          f.Length == 1234 // Set the appropriate length
      );

      tripDto.Image = fakeFormFile;

      var updatedTrip = new Trip
      {
        // Populate properties as needed
      };

      mockMapper.Setup(m => m.Map<Trip>(tripDto))
                 .Returns(updatedTrip);

      tripMockRepository.Setup(repo => repo.Update(updatedTrip))
                         .Returns(false);


      controller.ControllerContext = new ControllerContext();
      controller.ControllerContext.HttpContext = new DefaultHttpContext();

      // Act
      var result = await controller.Update(tripDto);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal("Unable to update trip.", badRequestResult.Value);
    }

    [Fact]
    public async Task Update_SuccessfulUpdate_ReturnsOkResult()
    {
      // Arrange
      var tripDto = new TripDto
      {
        Id = 0,
        UserId = "testUserId",
        Title = "Test Trip",
        Description = "Test Description",
        Price = 100,
        DateFrom = DateTime.UtcNow,
        DateTo = DateTime.UtcNow.AddDays(7),
      };

      var fakeFormFile = Mock.Of<IFormFile>(f =>
          f.FileName == "test.jpg" &&
          f.Length == 1234 // Set the appropriate length
      );

      tripDto.Image = fakeFormFile;

      var expectedImageUrl = "testImageUrl";

      var updatedTrip = new Trip
      {
        Id = 0,
        UserId = "testUserId",
        Title = "Test Trip",
        Description = "Test Description",
        Price = 100,
        DateFrom = DateTime.UtcNow,
        DateTo = DateTime.UtcNow.AddDays(7),
      };

      mockMapper.Setup(m => m.Map<Trip>(tripDto))
                 .Returns(updatedTrip);

      tripMockRepository.Setup(repo => repo.Update(updatedTrip))
                         .Returns(true);

      controller.ControllerContext = new ControllerContext();
      controller.ControllerContext.HttpContext = new DefaultHttpContext();

      // Act
      var result = await controller.Update(tripDto);

      // Assert
      Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteTrip_SuccessfulDeletion_ReturnsOkResult()
    {
      // Arrange
      const int tripId = 123;

      tripMockRepository.Setup(repo => repo.Delete(tripId))
                         .Returns(true);

      // Act
      var result = await controller.DeleteTrip(id: tripId);

      // Assert
      Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteTrip_DeletionFailed_ReturnsBadRequest()
    {
      // Arrange
      const int tripId = 123;

      tripMockRepository.Setup(repo => repo.Delete(tripId))
                         .Returns(false);

      // Act
      var result = await controller.DeleteTrip(id: tripId);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal("Error while Deleting Trip.", badRequestResult.Value);
    }


  }
}
