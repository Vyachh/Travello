using TravelloApi.Models;

namespace TravelloApi.Interfaces
{
  public interface IFlightRepository
  {
    Task<List<Flight>> AsyncGetAll();
    Task<List<Flight>> AsyncGet(int id);
    Task<List<Flight>> SearchFlights(FlightSearchParameters parameters);
    bool Add(Flight flight);
    bool Update(Flight flight);
    bool Delete(int id);
    Task<bool> SaveAsync();
    bool Save();
  }
}
