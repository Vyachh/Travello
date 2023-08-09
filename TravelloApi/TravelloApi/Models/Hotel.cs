namespace TravelloApi.Models
{
  public class Hotel
  {
    
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string PictureUrl { get; set; }
    public double Grade { get; set; }
    public double Price { get; set; }
    public string City { get; set; }
  }
}
