using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using TravelloApi.Models;

namespace TravelloApi.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<User> User { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Trip> Trip { get; set; }
    public DbSet<UserTrips> UserTrips { get; set; }
    public DbSet<Photo> Photos { get; set; }

  }
}
