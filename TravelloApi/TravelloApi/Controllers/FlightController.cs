using Microsoft.AspNetCore.Mvc;
using TravelloApi.Interfaces;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelloApi.Models;

namespace TravelloApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class FlightController : ControllerBase
  {
    private readonly IFlightRepository flightRepository;

    public FlightController(IFlightRepository flightRepository)
    {
      this.flightRepository = flightRepository;
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAll()
    {
      return Ok(await flightRepository.AsyncGetAll());
    }

    [HttpPost("search")] // Server side search 
    public async Task<IActionResult> SearchFlights([FromBody] FlightSearchParameters searchParameters)
    {
      var flights = await flightRepository.SearchFlights(searchParameters);
      return Ok(flights);
    }

    //[HttpGet("FakePacient")]
    //public IActionResult AddFakePacients()
    //{
    //  Flight[] flights = new Flight[]
    //  {
    //new Flight
    //{
    //    Departure = "New York",
    //    Arrival = "Los Angeles",
    //    DepartureTime = DateTime.Now,
    //    ArrivalTime = DateTime.Now.AddHours(5),
    //    Price = 250.00
    //},
    //new Flight
    //{
    //    Departure = "Miami",
    //    Arrival = "Chicago",
    //    DepartureTime = DateTime.Now.AddHours(2),
    //    ArrivalTime = DateTime.Now.AddHours(4),
    //    Price = 180.00
    //},
    //new Flight
    //{
    //    Departure = "San Francisco",
    //    Arrival = "Las Vegas",
    //    DepartureTime = DateTime.Now.AddHours(3),
    //    ArrivalTime = DateTime.Now.AddHours(5),
    //    Price = 280.00
    //},
    //new Flight
    //{
    //    Departure = "Boston",
    //    Arrival = "Washington, D.C.",
    //    DepartureTime = DateTime.Now.AddHours(1),
    //    ArrivalTime = DateTime.Now.AddHours(3),
    //    Price = 220.00
    //},
    //new Flight
    //{
    //    Departure = "Seattle",
    //    Arrival = "Denver",
    //    DepartureTime = DateTime.Now.AddHours(2),
    //    ArrivalTime = DateTime.Now.AddHours(4),
    //    Price = 150.00
    //},
    //new Flight
    //{
    //    Departure = "Los Angeles",
    //    Arrival = "San Francisco",
    //    DepartureTime = DateTime.Now.AddHours(1),
    //    ArrivalTime = DateTime.Now.AddHours(2),
    //    Price = 450.00
    //},
    //new Flight
    //{
    //    Departure = "Chicago",
    //    Arrival = "New York",
    //    DepartureTime = DateTime.Now.AddHours(2),
    //    ArrivalTime = DateTime.Now.AddHours(4),
    //    Price = 210.00
    //},
    //new Flight
    //{
    //    Departure = "Honolulu",
    //    Arrival = "Maui",
    //    DepartureTime = DateTime.Now.AddHours(2),
    //    ArrivalTime = DateTime.Now.AddHours(1),
    //    Price = 290.00
    //},
    //new Flight
    //{
    //    Departure = "London",
    //    Arrival = "Paris",
    //    DepartureTime = DateTime.Now.AddHours(2),
    //    ArrivalTime = DateTime.Now.AddHours(1),
    //    Price = 130.00
    //},
    //new Flight
    //{
    //    Departure = "Tokyo",
    //    Arrival = "Seoul",
    //    DepartureTime = DateTime.Now.AddHours(3),
    //    ArrivalTime = DateTime.Now.AddHours(2),
    //    Price = 240.00
    //}
    //  };

    //  foreach (var item in flights)
    //  {
    //    flightRepository.Add(item);
    //  }
    //  return Ok();
    //}
  }
}
