using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelloApi.Dto;
using TravelloApi.Enums;
using TravelloApi.Helpers;
using TravelloApi.Interfaces;
using TravelloApi.Models;
using TravelloApi.Reposity;

namespace TravelloApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class PhotoController : Controller
  {
    private readonly IPhotoRepository photoRepository;
    private readonly IUserRepository userRepository;
    private readonly ITripRepository tripRepository;

    public PhotoController(IPhotoRepository photoRepository, IUserRepository userRepository,
      ITripRepository tripRepository)
    {
      this.photoRepository = photoRepository;
      this.userRepository = userRepository;
      this.tripRepository = tripRepository;
    }
    [HttpPost("upload")]
    public async Task<IActionResult> UploadPhoto([FromForm] PhotoDto photoDto)
    {
      if (!await AddToFolder(photoDto.Photo, photoDto.FileType))
      {
        return BadRequest("Photo Upload");
      }

      var user = await userRepository.GetUserById(photoDto.UserId); // Вот эта хуйня все ломает

      if (photoDto.FileType.Equals(FileType.AvatarImage)) {
        if (!await AddToUserRepository(photoDto, user))
        {
          return BadRequest("Photo Repo");
        }
      }

      //if (photoDto.FileType.Equals(FileType.AvatarImage))
      //{
      //  if (!await AddToTripRepository(photoDto, user))
      //  {
      //    return BadRequest("Photo Repo");
      //  }
      //}


      return Ok();
    }

    private static async Task<bool> AddToFolder(IFormFile photo, FileType fileType)
    {
      if (photo != null && photo.Length > 0)
      {
        var fileName = photo.FileName;
        var filePath = Common.GetFilePath(fileName, fileType);
        using var stream = new FileStream(filePath, FileMode.Create);
        await photo.CopyToAsync(stream);
        return true;
      }
      return false;
    }

    private async Task<bool> AddToUserRepository(PhotoDto photoDto, User user)
    {
      if (photoDto.Photo != null && photoDto.Photo.Length > 0)
      {
        Photo photo = new()
        {
          Name = photoDto.Photo.FileName,
          IdentityId = user.Id,
        };

        if (!photoRepository.UploadPhoto(photo))
        {
          return false;
        }

        user.Photo = photo;

        if (!userRepository.Update(user))
        {
          return false;
        }

        return true;
      }
      return false;
    }

    private async Task<bool> AddToTripRepository(PhotoDto photoDto, Trip trip, User user)
    {
      if (photoDto.Photo != null && photoDto.Photo.Length > 0)
      {
        Photo photo = new()
        {
          Name = photoDto.Photo.FileName,
          IdentityId = trip.Id.ToString(),
        };

        if (!photoRepository.UploadPhoto(photo))
        {
          return false;
        }

        user.Photo = photo;

        if (!userRepository.Update(user))
        {
          return false;
        }

        return true;
      }
      return false;
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetUserPhoto([FromQuery] string userId)
    {
      var user = await userRepository.GetUserById(userId);

      string filePath = Path.Combine("wwwroot", "Avatars", user.Photo.Name);

      // Проверяем, существует ли файл на сервере
      if (System.IO.File.Exists(filePath))
      {
        // Отправляем файл из папки Avatars в ответ на запрос
        string publicUrl = $"https://localhost:7001/{filePath.Replace("\\", "/").Replace("wwwroot", "").ToLower()}";
        return Ok(publicUrl);
      }
      else
      {
        // Если файл не найден, возвращаем ошибку 404 Not Found
        return NotFound();
      }

    }

    [HttpGet("GetTripPhoto")]
    public async Task<IActionResult> GetTripPhoto([FromQuery] int tripId)
    {
      var trip = await tripRepository.GetById(tripId);

      string filePath = Path.Combine("wwwroot", "TripImage", "ROT");

      // Проверяем, существует ли файл на сервере
      if (System.IO.File.Exists(filePath))
      {
        // Отправляем файл из папки Avatars в ответ на запрос
        string publicUrl = $"https://localhost:7001/{filePath.Replace("\\", "/").Replace("wwwroot", "").ToLower()}";
        return Ok(publicUrl);
      }
      else
      {
        // Если файл не найден, возвращаем ошибку 404 Not Found
        return NotFound();
      }
    }

  }
}
