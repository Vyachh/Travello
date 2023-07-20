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
    private readonly IMapper mapper;

    public TripController(ITripRepository tripRepository, IMapper mapper)
    {
      this.tripRepository = tripRepository;
      this.mapper = mapper;
    }
    [HttpPost("TripCreate"),Authorize(Roles ="Admin,Moderator,Organizer")] // TODO: add UserId to trip.
    public async Task<IActionResult> AddTrip([FromQuery] TripDto tripDto)
    {
      var trip = mapper.Map<Trip>(tripDto);
      if (!tripRepository.Add(trip))
      {
        return BadRequest("Smth went wrong.");
      }
      return Ok("Success!");
    }

    //public Task<IActionResult> 

  }
}
