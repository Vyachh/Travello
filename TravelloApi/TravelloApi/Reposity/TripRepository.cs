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

    public async Task<Trip> GetById(int id)
    {
      return dataContext.Trip.FirstOrDefault(t => t.Id == id);
    }

    public async Task<Trip> GetByName(string title)
    {
      return dataContext.Trip.FirstOrDefault(t => t.Title == title);

    }

    public async Task<Trip> GetNextTrip()
    {
      return dataContext.Trip.FirstOrDefault(t => t.IsNextTrip == true);
    }

    public async Task<Trip> GetOngoingTrip()
    {
      return dataContext.Trip.FirstOrDefault(t => t.IsOngoingTrip == true);
    }

    public bool Save()
    {
      var saved = dataContext.SaveChanges();
      return saved > 0;
    }

    public bool SetNextTrip(int id)
    {
      var currentTrip = dataContext.Trip.FirstOrDefault(t => t.IsNextTrip);

      if (currentTrip != null)
      {
        currentTrip.IsNextTrip = false;
      }


      var nextTrip = dataContext.Trip.FirstOrDefault(t => t.Id == id);

      if (nextTrip != null)
      {
        nextTrip.IsNextTrip = true;
      }

      return Save();
    }

    public bool SetOngoingTrip(int id)
    {
      var currentTrip = dataContext.Trip.FirstOrDefault(t => t.IsOngoingTrip);

      if (currentTrip != null)
      {
        currentTrip.IsOngoingTrip = false;
      }


      var onGoingTrip = dataContext.Trip.FirstOrDefault(t => t.Id == id);

      if (onGoingTrip != null)
      {
        onGoingTrip.IsOngoingTrip = true;
      }

      return Save();
    }

    public bool Update(Trip trip)
    {
      dataContext.Trip.Update(trip);
      return Save();
    }
  }
}
