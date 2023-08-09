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
      var photo = await dataContext.Photos.FirstOrDefaultAsync(p => p.IdentityId == userId);
      return photo ?? new Photo();
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
