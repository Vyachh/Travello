using Microsoft.EntityFrameworkCore;
using TravelloApi.Data;
using TravelloApi.Interfaces;
using TravelloApi.Models;

namespace TravelloApi.Reposity
{
  public class TripRepository : ITripRepository
  {
    private readonly DataContext dataContext;

    public TripRepository(DataContext dataContext)
    {
      this.dataContext = dataContext;
    }
    public bool Add(Trip trip)
    {
      dataContext.Trip.Add(trip);
      return Save();
    }

    public bool Delete(int id)
    {
      var trip = dataContext.Trip.FirstOrDefault(t => t.Id == id);
      dataContext.Trip.Remove(trip);
      return Save();
    }

    public async Task<IEnumerable<Trip>> GetAll()
    {
      return await dataContext.Trip.ToListAsync();
    }

    public async Task<Trip> GetTripById(int id)
    {
      return dataContext.Trip.FirstOrDefault(t => t.Id == id);
    }

    public async Task<Trip> GetTripByName(string title)
    {
      return dataContext.Trip.FirstOrDefault(t => t.Title == title);

    }

    public bool Save()
    {
      var saved = dataContext.SaveChanges();
      return saved > 0;
    }

    public bool Update(Trip trip)
    {
      dataContext.Trip.Update(trip);
      return Save();
    }
  }
}
