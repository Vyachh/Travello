using TravelloApi.Models;

namespace TravelloApi.Interfaces
{
  public interface ITripRepository
  {
    Task<Trip> GetTripById(int id);
    Task<Trip> GetTripByName(string userName);
    Task<IEnumerable<Trip>> GetAll();

    bool Add(Trip user);
    bool Update(Trip user);
    bool Delete(int id);
    bool Save();
  }
}
