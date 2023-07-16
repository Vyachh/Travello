using System.ComponentModel.DataAnnotations;

namespace TravelloApi.Models
{
  public class User
  {
    [Key]
    public string Id { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public int CurrentTripId { get; set; }
  }
}
