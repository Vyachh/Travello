using TravelloApi.Models;

namespace TravelloApi.Interfaces
{
  public interface IHotelRepository
  {
    Task<List<Hotel>> AsyncGetAll();

    bool Add(Hotel hotel);
    bool Update(Hotel hotel);
    bool Delete(int id);
    bool Save();
  }
}
