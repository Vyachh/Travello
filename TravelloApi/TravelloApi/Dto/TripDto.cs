namespace TravelloApi.Dto
{
  public class TripDto
  {
    public string UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public string Image { get; set; } = string.Empty;

  }

}
