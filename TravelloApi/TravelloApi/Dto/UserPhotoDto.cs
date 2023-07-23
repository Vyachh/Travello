namespace TravelloApi.Dto
{
  public class UserPhotoDto
  {
    public required string UserId { get; set; }
    public required IFormFile Photo { get; set; }

  }
}
