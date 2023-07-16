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

    public async Task<User> GetUserById(string id)
    {
      return dataContext.User
                .FirstOrDefault(u => u.Id == id);
    }

    public async Task<User> GetUserByName(string userName)
    {
      return dataContext.User.FirstOrDefault(u => u.UserName == userName);
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
  }
}
