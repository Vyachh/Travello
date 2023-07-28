using TravelloApi.Enums;

namespace TravelloApi.Dto
{
  public class PhotoDto
  {
    public required string UserId { get; set; }
    public required IFormFile Photo { get; set; }
    public required FileType FileType { get; set; }

  }
}
