using System.ComponentModel.DataAnnotations;
using TravelloApi.Enums;

namespace TravelloApi.Models
{
  public class User
  {
    [Key]
    public string Id { get; set; }
    public string UserName { get; set; }
    public Role Role { get; set; } = Role.User;
    public string PasswordHash { get; set; } = string.Empty;
    public int CurrentTripId { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime TokenCreated { get; set; }
    public DateTime TokenExpires { get; set; }
  }
}
