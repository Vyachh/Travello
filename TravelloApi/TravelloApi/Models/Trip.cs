using System.ComponentModel.DataAnnotations.Schema;

namespace TravelloApi.Models
{
  public class Trip
  {
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public string Author { get; set; } = string.Empty;
    public int Price { get; set; }
    public string ImageUrl { get; set; }
    public bool IsNextTrip { get; set; }
    public bool IsOngoingTrip { get; set; }
    public bool IsApproved { get; set; }

  }
}
