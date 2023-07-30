using TravelloApi.Models;

namespace TravelloApi.Interfaces
{
  public interface IUserRepository
  {
    Task<IEnumerable<User>> GetAll();
    Task<User> GetUserById(string id);
    Task<User> GetUserByName(string userName);
    Task<string> GetUserName(string id);
    Task<List<TripCount>> GetOngoingPeopleCount();
    bool Add(User user);
    bool Update(User user);
    bool Delete(string id);
    bool Save();
  }
}
