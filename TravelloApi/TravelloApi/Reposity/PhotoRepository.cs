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
      return dataContext.Photos.FirstOrDefault(p => p.UserId == userId);
    }
    public bool UploadPhoto(Photo photo)
    {
      dataContext.Photos.Add(photo);
      return Save();
    }

    //public async Task<string> UploadFile(IFormFile formFile)
    //{
    //  string fileName = "";
    //  try
    //  {
    //    FileInfo fileInfo = new FileInfo(formFile.FileName);
    //    fileName = formFile.FileName + "_" + DateTime.Now.Ticks.ToString() + fileInfo.Extension;
    //    var getFilePath = Common.GetFilePath(fileName);
    //    using (var fileStream = new FileStream(getFilePath, FileMode.Create))
    //    {
    //      await formFile.CopyToAsync(fileStream);
    //    }
    //    return fileName;
    //  }
    //  catch (Exception ex)
    //  {
    //    throw ex;
    //  }
    //}


    //public bool Add(Photo photo)
    //{
    //  dataContext.Photo.Add(photo);
    //  return Save();
    //}

    //public bool Delete(int id)
    //{
    //  var photo = dataContext.Photo.FirstOrDefault(p => p.Id == id);
    //  dataContext.Photo.Remove(photo);
    //  return Save();
    //}

    //public async Task<IEnumerable<Photo>> GetAll()
    //{
    //  return dataContext.Photo.ToList();
    //}

    //public async Task<Photo> GetPhotoById(int id)
    //{
    //  return dataContext.Photo.FirstOrDefault(p => p.Id == id);
    //}

    public bool Save()
    {
      var saved = dataContext.SaveChanges();
      return saved > 0;
    }

    //public bool Update(Photo photo)
    //{
    //  dataContext.Photo.Update(photo);
    //  return Save();
    //}

  }
}
