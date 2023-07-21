namespace TravelloApi.Models
{
  public class Trip
  {
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Title { get; set; }= string.Empty;
    public string Description { get; set; } = string.Empty;
    //public int TravelTime { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;

  }
}
