using TravelloApi.Enums;

namespace TravelloApi.Models
{
  public class Photo
  {
    public int Id { get; set; }
    public string IdentityId { get; set; }
    public FileType FileType { get; set; }
    public string Name { get; set; }
  }
}
