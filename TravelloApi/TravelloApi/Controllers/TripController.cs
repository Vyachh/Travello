using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Globalization;
using TravelloApi.Dto;
using TravelloApi.Enums;
using TravelloApi.Helpers;
using TravelloApi.Interfaces;
using TravelloApi.Models;
using TravelloApi.Reposity;
using static System.Net.Mime.MediaTypeNames;

namespace TravelloApi.Controllers
{
  /// <summary>
  /// Контроллер для управления данными о поездках.
  /// </summary>
  [ApiController]
  [Route("[controller]")]
  public class TripController : ControllerBase
  {
    private readonly ITripRepository tripRepository;
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor contextAccessor;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера для управления данными о поездках.
    /// </summary>
    /// <param name="tripRepository">Репозиторий для работы с данными о поездках.</param>
    /// <param name="userRepository">Репозиторий для работы с данными о пользователях.</param>
    /// <param name="mapper">Объект для маппинга данных между сущностями и DTO.</param>
    public TripController(ITripRepository tripRepository, IUserRepository userRepository, IMapper mapper, IHttpContextAccessor contextAccessor)
    {
      this.tripRepository = tripRepository;
      this.userRepository = userRepository;
      this.mapper = mapper;
      this.contextAccessor = contextAccessor;
    }

    /// <summary>
    /// Получает информацию о конкретной поездке по её идентификатору.
    /// </summary>
    [HttpGet("GetTrip")]
    public async Task<IActionResult> GetTrip([FromQuery] int id)
    {
      var trip = await tripRepository.GetById(id);
      if (trip == null)
      {
        return NotFound();
      }

      return Ok(trip);
    }

    /// <summary>
    /// Устанавливает текущую поездку как активную (в процессе).
    /// </summary>
    [HttpGet("SetOngoingTrip")]
    public async Task<IActionResult> SetOngoingTrip([FromQuery] int id)
    {
      if (!tripRepository.SetOngoingTrip(id))
      {
        return BadRequest("Unable to set ongoing trip.");
      }

      return Ok();
    }

    /// <summary>
    /// Устанавливает текущую поездку как следующую.
    /// </summary>
    [HttpGet("SetNextTrip")]
    public async Task<IActionResult> SetNextTrip([FromQuery] int id)
    {
      if (!tripRepository.SetNextTrip(id))
      {
        return BadRequest("Unable to set ongoing trip.");
      }

      return Ok();
    }

    /// <summary>
    /// Получает информацию о следующей поездке.
    /// </summary>
    [HttpGet("GetNextTrip")]
    public async Task<IActionResult> GetNextTrip()
    {
      return Ok(await tripRepository.GetNextTrip());
    }

    /// <summary>
    /// Получает информацию о текущей (в процессе) поездке.
    /// </summary>
    [HttpGet("GetOngoingTrip")]
    public async Task<IActionResult> GetOngoingTrip()
    {
      return Ok(await tripRepository.GetOngoingTrip());
    }

    /// <summary>
    /// Получает список всех поездок.
    /// </summary>
    [HttpGet("GetTripList"), Authorize(Roles = "Admin,Moderator,Organizer")]
    public async Task<IActionResult> GetTripList()
    {
      var tripList = await tripRepository.GetAll();

      if (tripList == null)
      {
        return BadRequest();
      }

      return Ok(tripList);
    }

    /// <summary>
    /// Создает новую поездку согласно переданным данным.
    /// </summary>
    /// <param name="tripDto">Данные о поездке.</param>
    /// <returns>Результат операции создания.</returns>
    [HttpPost("CreateTrip"), Authorize(Roles = "Admin,Moderator,Organizer")]
    public async Task<IActionResult> AddTrip([FromForm] TripDto tripDto)
    {
      var trip = mapper.Map<Trip>(tripDto);

      if (trip == null)
      {
        return StatusCode(500);
      }

      trip.Author = await userRepository.GetUserName(trip.UserId);

      if (!await AddImage(tripDto))
      {
        return BadRequest("Smth went wrong.");
      }

      trip.ImageUrl = await GetTripPhoto(tripDto.Image.FileName);

      if (!tripRepository.Add(trip))
      {
        return BadRequest("Smth went wrong.");
      }
      return Ok();
    }

    /// <summary>
    /// Утверждает указанную поездку.
    /// </summary>
    [HttpPut("Approve")]
    public async Task<IActionResult> Approve(int id)
    {
      var trip = await tripRepository.GetById(id);

      if (trip == null)
      {
        return BadRequest();
      }

      trip.IsApproved = true;
      if (!tripRepository.Save())
      {
        return StatusCode(500);
      }

      return Ok();
    }

    /// <summary>
    /// Обновляет информацию о поездке.
    /// </summary>
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromForm] TripDto tripDto)
    {
      var trip = mapper.Map<Trip>(tripDto);

      if (!await AddImage(tripDto))
      {
        return BadRequest("Unable to upload photo.");
      }

      trip.ImageUrl = await GetTripPhoto(tripDto.Image.FileName);

      if (!tripRepository.Update(trip))
      {
        return BadRequest("Unable to update trip.");
      }

      return Ok();
    }

    /// <summary>
    /// Удаляет указанную поездку.
    /// </summary>
    [HttpDelete("Delete"), Authorize(Roles = "Admin,Moderator,Organizer")]
    public async Task<IActionResult> DeleteTrip([FromQuery] int id)
    {

      if (!tripRepository.Delete(id))
      {
        return BadRequest("Error while Deleting Trip.");
      }
      return Ok();
    }

    /// <summary>
    /// Получает публичный URL для фотографии поездки.
    /// </summary>
    /// <param name="fileName">Имя файла фотографии.</param>
    /// <returns>Публичный URL фотографии.</returns>
     [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<string> GetTripPhoto(string fileName)
    {

      string filePath = Path.Combine("wwwroot", "TripImage", fileName);

      // Проверяем, существует ли файл на сервере
      if (System.IO.File.Exists(filePath))
      {
        // Отправляем файл из папки Avatars в ответ на запрос
        string publicUrl = $"https://localhost:7001/{filePath.Replace("\\", "/").Replace("wwwroot", "").ToLower()}";
        return publicUrl;
      }
      else
      {
        throw new FileNotFoundException(filePath);
      }
    }

    /// <summary>
    /// Добавляет изображение к поездке, если оно предоставлено.
    /// </summary>
    /// <param name="tripDto">Данные о поездке.</param>
    /// <returns>Значение true, если изображение было успешно добавлено, иначе false.</returns>
     [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<bool> AddImage(TripDto tripDto)
    {
      var photo = tripDto.Image;

      if (tripDto.Image != null && tripDto.Image.Length > 0)
      {
        var fileName = photo.FileName;
        var filePath = Common.GetFilePath(fileName, FileType.TripImage);
        using var stream = new FileStream(filePath, FileMode.Create);
        await photo.CopyToAsync(stream);
        return true;
      }
      return false;
    }

  }

}
