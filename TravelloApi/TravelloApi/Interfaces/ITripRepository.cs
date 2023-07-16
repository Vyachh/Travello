using TravelloApi.Models;

namespace TravelloApi.Interfaces
{
  public interface ITripRepository
  {
    Task<Trip> GetTripById(int id);
    Task<Trip> GetTripByName(string userName);

    bool Add(Trip user);
    bool Update(Trip user);
    bool Delete(string id);
    bool Save();
  }
}
