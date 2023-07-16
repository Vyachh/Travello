using TravelloApi.Models;

namespace TravelloApi.Interfaces
{
  public interface IUserRepository
  {
    Task<User> GetUserById(string id);
    Task<User> GetUserByName(string userName);

    bool Add(User user);
    bool Update(User user);
    bool Delete(string id);
    bool Save();
  }
}
