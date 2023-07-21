using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelloApi.Dto;
using TravelloApi.Interfaces;
using TravelloApi.Models;

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
    [HttpPost("CreateTrip"), Authorize(Roles = "Admin,Moderator,Organizer")] // TODO: add UserId to trip.
    public async Task<IActionResult> AddTrip([FromBody] TripDto tripDto)
    {
      var trip = mapper.Map<Trip>(tripDto);

      var user = await userRepository.GetUserById(trip.UserId);

      trip.Author = user.UserName;
      if (!tripRepository.Add(trip))
      {
        return BadRequest("Smth went wrong.");
      }
      return Ok("Success!");
    }

    [HttpGet("GetNextTrip")]
    public async Task<IActionResult> GetNextTrip()
    {
      return Ok(await tripRepository.GetTripById(2));
    }

    [HttpGet("GetTripList"), Authorize(Roles = "Admin,Moderator,Organizer")]
    public async Task<IActionResult> GetTripList()
    {
      return Ok(await tripRepository.GetAll());
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
  }
}
