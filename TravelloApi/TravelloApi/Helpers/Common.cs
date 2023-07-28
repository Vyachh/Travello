using TravelloApi.Enums;

namespace TravelloApi.Helpers
{
  public class Common
  {
    public static string GetCurrentDirectory()
    {
      var result = Directory.GetCurrentDirectory();
      return result;
    }
    public static string GetStaticContentDirectory(FileType fileType)
    {
      string result = "";
      if (fileType.Equals(FileType.TripImage))
      {
        result = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\TripImage\\");
      }
      if (fileType.Equals(FileType.AvatarImage))
      {
        result = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Avatars\\");
      }
      if (!Directory.Exists(result))
      {
        Directory.CreateDirectory(result);
      }
      return result;
    }
    public static string GetFilePath(string FileName, FileType fileType)
    {
      var _GetStaticContentDirectory = GetStaticContentDirectory(fileType);
      var result = Path.Combine(_GetStaticContentDirectory, FileName);
      return result;
    }
  }
}
