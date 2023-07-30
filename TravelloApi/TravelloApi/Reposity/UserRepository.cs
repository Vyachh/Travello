using Microsoft.EntityFrameworkCore;
using TravelloApi.Data;
using TravelloApi.Interfaces;
using TravelloApi.Models;

namespace TravelloApi.Reposity
{
  public class UserRepository : IUserRepository
  {
    private readonly DataContext dataContext;

    public UserRepository(DataContext dataContext)
    {
      this.dataContext = dataContext;
    }

    public bool Add(User user)
    {
      dataContext.User.Add(user);
      return Save();
    }

    public bool Delete(string id)
    {
      dataContext.Remove(id);
      return Save();
    }

    public async Task<IEnumerable<User>> GetAll()
    {
      return dataContext.User
        .Include(u => u.Photo)
        .ToList();
    }

    public async Task<User> GetUserById(string id)
    {
      return dataContext.User
        .Include(u => u.Photo)
        .FirstOrDefault(u => u.Id == id);
    }

    public async Task<User> GetUserByName(string userName)
    {
      return dataContext.User.FirstOrDefault(u => u.UserName == userName);
    }

    public async Task<string> GetUserName(string id)
    {
      return dataContext.User.FirstOrDefault(u => u.Id == id).UserName;
    }



    public bool Save()
    {
      var saved = dataContext.SaveChanges();
      return saved > 0;
    }

    public bool Update(User user)
    {
      dataContext.Update(user);
      return Save();
    }

    public async Task<List<TripCount>> GetOngoingPeopleCount()
    {
      return await dataContext.Trip
    .GroupJoin(dataContext.User, t => t.Id, u => u.CurrentTripId, (t, userGroup) => new { Trip = t, Users = userGroup })
    .SelectMany(t => t.Users.DefaultIfEmpty(), (t, u) => new { t.Trip, User = u })
    .GroupBy(t => new { t.Trip.Id })
    .Select(g => new TripCount
    {
      TripId = g.Key.Id,
      Count = g.Count(u => u.User != null)
    })
    .ToListAsync();
    }
  }
}
