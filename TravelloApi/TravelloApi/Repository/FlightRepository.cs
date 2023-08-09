using Microsoft.EntityFrameworkCore;
using TravelloApi.Data;
using TravelloApi.Interfaces;
using TravelloApi.Models;

namespace TravelloApi.Reposity
{
  public class FlightRepository : IFlightRepository
  {
    private readonly DataContext dataContext;

    public FlightRepository(DataContext dataContext)
    {
      this.dataContext = dataContext;
    }
    public bool Add(Flight flight)
    {
      dataContext.Add(flight);
      return Save();
    }

    public Task<List<Flight>> AsyncGet(int id)
    {
      throw new NotImplementedException();
    }

    public async Task<List<Flight>> AsyncGetAll()
    {
      return await dataContext.Flights.ToListAsync();
    }

    public bool Delete(int id)
    {
      dataContext.Remove(id);
      return Save();
    }

    public bool Save()
    {
      var saved = dataContext.SaveChanges();
      return saved > 0;
    }

    public async Task<bool> SaveAsync()
    {
      var saved = await dataContext.SaveChangesAsync();
      return saved > 0;
    }

    public async Task<List<Flight>> SearchFlights(FlightSearchParameters parameters)
    {
      return await dataContext.Flights
        .Where(flight =>
        flight.Departure == parameters.Departure &&
        flight.Arrival == parameters.Arrival &&
        flight.ArrivalTime >= parameters.ArrivalTime &&
        flight.ArrivalTime >= parameters.ArrivalTime
        )
        .ToListAsync();
    }

    public bool Update(Flight flight)
    {
      dataContext.Update(flight);
      return Save();
    }
  }
}
