using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelloApi.Dto;
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

    public PhotoController(IPhotoRepository photoRepository, IUserRepository userRepository)
    {
      this.photoRepository = photoRepository;
      this.userRepository = userRepository;
    }
    [HttpPost("upload")]
    public async Task<IActionResult> UploadPhoto([FromForm] UserPhotoDto userPhotoDto)
    {
      if (!await AddToFolder(userPhotoDto.Photo))
      {
        return BadRequest("Photo Upload");
      }

      var user = await userRepository.GetUserById(userPhotoDto.UserId);

      if (!await AddToRepository(userPhotoDto, user))
      {
        return BadRequest("Photo Repo");
      }

      return Ok();
    }

    private static async Task<bool> AddToFolder(IFormFile photo)
    {
      if (photo != null && photo.Length > 0)
      {
        FileInfo fileInfo = new(photo.FileName);
        var fileName = photo.FileName;
        var filePath = Common.GetFilePath(fileName);
        using var stream = new FileStream(filePath, FileMode.Create);
        await photo.CopyToAsync(stream);
        return true;
      }
      return false;
    }

    private async Task<bool> AddToRepository(UserPhotoDto userPhotoDto, User user)
    {
      if (userPhotoDto.Photo != null && userPhotoDto.Photo.Length > 0)
      {
        Photo photo = new()
        {
          Name = userPhotoDto.Photo.FileName,
          UserId = user.Id
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
  }
}
