using TravelloApi.Models;

namespace TravelloApi.Interfaces
{
  public interface IPhotoRepository
  {
    bool UploadPhoto(Photo photo);
    Task<Photo> GetUserPhoto(string userId);
  }
}
