using TravelloApi.Models;

namespace TravelloApi.Interfaces
{
  public interface ITripRepository
  {
    Task<Trip> GetById(int id);
    Task<Trip> GetByName(string userName);
    Task<Trip> GetNextTrip();
    Task<Trip> GetOngoingTrip();
    Task<IEnumerable<Trip>> GetAll();
    bool SetNextTrip(int id);
    bool UndoNextTrip(int id);
    bool SetOngoingTrip(int id);
    bool UndoOngoingTrip(int id);
    bool Add(Trip user);
    bool Update(Trip user);
    bool Delete(int id);
    bool Save();
  }
}
