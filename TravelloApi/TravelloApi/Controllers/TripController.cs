using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
  [ApiController]
  [Route("[controller]")]
  public class TripController : ControllerBase
  {
    private readonly ITripRepository tripRepository;
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;

    public TripController(ITripRepository tripRepository, IUserRepository userRepository, IMapper mapper)
    {
      this.tripRepository = tripRepository;
      this.userRepository = userRepository;
      this.mapper = mapper;
    }


    [HttpGet("GetTrip")]
    public async Task<IActionResult> GetTrip([FromQuery]int id)
    {
      return Ok(tripRepository.GetById(id));
    }

    [HttpPost("CreateTrip"), Authorize(Roles = "Admin,Moderator,Organizer")]
    public async Task<IActionResult> AddTrip([FromForm] TripDto tripDto)
    {
      var trip = mapper.Map<Trip>(tripDto);

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
      return Ok("Success!");
    }

    [HttpGet("SetOngoingTrip")]
    public async Task<IActionResult> SetOngoingTrip([FromQuery] int id)
    {
      if (!tripRepository.SetOngoingTrip(id))
      {
        return BadRequest("Smth went wrong.");
      }

      return Ok();
    }


    [HttpGet("SetNextTrip")]
    public async Task<IActionResult> SetNextTrip(int id)
    {
      if (!tripRepository.SetNextTrip(id))
      {
        return BadRequest("Smth went wrong.");
      }

      return Ok();
    }

    [HttpGet("GetNextTrip")]
    public async Task<IActionResult> GetNextTrip()
    {
      return Ok(await tripRepository.GetNextTrip());
    }
    [HttpGet("GetOngoingTrip")]
    public async Task<IActionResult> GetOngoingTrip()
    {
      return Ok(await tripRepository.GetOngoingTrip());
    }

    [HttpGet("GetTripList"), Authorize(Roles = "Admin,Moderator,Organizer")]
    public async Task<IActionResult> GetTripList()
    {
      var tripList = await tripRepository.GetAll();

      return Ok(tripList);
    }

    [HttpDelete("Delete"), Authorize(Roles = "Admin,Moderator,Organizer")]
    public async Task<IActionResult> DeleteTrip([FromQuery] int id)
    {

      if (!tripRepository.Delete(id))
      {
        return BadRequest("Error while Deleting Trip.");
      }
      return Ok();
    }


    private async Task<bool> AddImage(TripDto tripDto)
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

    private async Task<string> GetTripPhoto(string fileName)
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
        return "";
      }
    }
  }
}
