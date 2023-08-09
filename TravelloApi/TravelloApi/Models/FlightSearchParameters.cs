namespace TravelloApi.Models
{
  public class FlightSearchParameters
  {
    public string Departure { get; set; }
    public string Arrival { get; set; }
    public DateTime ArrivalTime { get; set; }
    public DateTime DepartureTime { get; set; }
    public double MaxPrice { get; set; }
    public int Passengers { get; set; }
  }
}
