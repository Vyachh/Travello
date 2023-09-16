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
      return await dataContext.Trip.FirstOrDefaultAsync(t => t.Id == id) ?? new Trip();
    }

    public async Task<Trip> GetByName(string title)
    {
      return await dataContext.Trip.FirstOrDefaultAsync(t => t.Title == title) ?? new Trip();
    }

    public async Task<Trip> GetNextTrip()
    {
      return await dataContext.Trip.FirstOrDefaultAsync(t => t.IsNextTrip == true) ?? new Trip();
    }

    public async Task<Trip> GetOngoingTrip()
    {
      return await dataContext.Trip.FirstOrDefaultAsync(t => t.IsOngoingTrip == true) ?? new Trip();
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

    public bool UndoNextTrip(int id)
    {
      var currentTrip = dataContext.Trip.FirstOrDefault(t => t.Id == id);

      if (currentTrip != null && currentTrip.IsNextTrip)
      {
        currentTrip.IsNextTrip = false;
        return Save();

      }

      throw new ArgumentException();
    }

    public bool UndoOngoingTrip(int id)
    {
      var currentTrip = dataContext.Trip.FirstOrDefault(t => t.Id == id);

      if (currentTrip != null && currentTrip.IsOngoingTrip)
      {
        currentTrip.IsOngoingTrip = false;
        return Save();
      }

      throw new ArgumentException();
    }

    public bool Update(Trip trip)
    {
      dataContext.Trip.Update(trip);
      return Save();
    }
  }
}
