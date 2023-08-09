using System.ComponentModel.DataAnnotations.Schema;

namespace TravelloApi.Dto
{
  public class TripDto
  {
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Price { get; set; }
    public IFormFile? Image { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
  }

}
