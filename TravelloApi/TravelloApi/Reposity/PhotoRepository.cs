using Microsoft.EntityFrameworkCore;
using TravelloApi.Data;
using TravelloApi.Helpers;
using TravelloApi.Interfaces;
using TravelloApi.Models;

namespace TravelloApi.Reposity
{
  public class PhotoRepository : IPhotoRepository
  {
    private readonly DataContext dataContext;

    public PhotoRepository(DataContext dataContext)
    {
      this.dataContext = dataContext;
    }


    public async Task<Photo> GetUserPhoto(string userId)
    {
      return dataContext.Photos.FirstOrDefault(p => p.IdentityId == userId);
    }

    public bool UploadPhoto(Photo photo)
    {
      dataContext.Photos.Add(photo);
      return Save();
    }

    public bool Save()
    {
      var saved = dataContext.SaveChanges();
      return saved > 0;
    }

  }
}
