using Microsoft.EntityFrameworkCore;
using TravelloApi.Data;
using TravelloApi.Interfaces;
using TravelloApi.Models;

namespace TravelloApi.Reposity
{
  public class HotelRepository : IHotelRepository
  {
    private readonly DataContext dataContext;

    public HotelRepository(DataContext dataContext)
    {
      this.dataContext = dataContext;
    }

    public bool Add(Hotel hotel)
    {
      dataContext.Add(hotel);
      return Save();
    }

    public bool Delete(int id)
    {
      throw new NotImplementedException();
    }

    public async Task<List<Hotel>> AsyncGetAll()
    {
      return await dataContext.Hotels.ToListAsync();

    }

    public bool Save()
    {
      var saved = dataContext.SaveChanges();
      return saved > 0;
    }

    public bool Update(Hotel hotel)
    {
      throw new NotImplementedException();
    }
  }
}
