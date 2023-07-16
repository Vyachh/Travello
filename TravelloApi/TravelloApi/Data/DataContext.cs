using Microsoft.EntityFrameworkCore;
using TravelloApi.Models;

namespace TravelloApi.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<User> User { get; set; }
    public DbSet<Trip> Trip { get; set; }
    public DbSet<UserTrips> UserTrips { get; set; }

  }
}
