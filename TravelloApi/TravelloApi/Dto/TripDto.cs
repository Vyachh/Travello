namespace TravelloApi.Dto
{
  public class TripDto
  {
    public string UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TravelTime { get; set; }
    public string Image { get; set; } = string.Empty;

  }

}
