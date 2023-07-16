using Microsoft.AspNetCore.Mvc;
using TravelloApi.Interfaces;
using TravelloApi.Models;

namespace TravelloApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class TripController : ControllerBase
  {
    private readonly ITripRepository tripRepository;

    public TripController(ITripRepository tripRepository)
    {
      this.tripRepository = tripRepository;
    }
    [HttpPost("tripcreate")] // TODO: add UserId to trip.
    public async Task<IActionResult> AddTrip([FromQuery] Trip trip)
    {
      if (!tripRepository.Add(trip))
      {
        return BadRequest("Smth went wrong.");
      }
      return Ok("Success!");
    }
  }
}
