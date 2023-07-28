using TravelloApi.Enums;
using TravelloApi.Models;

namespace TravelloApi.Dto
{
  public class UserInfoDto
  {
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
  }
}
