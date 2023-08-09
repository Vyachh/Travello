using TravelloApi.Dto;

namespace TravelloApi.Interfaces
{
  public interface IEmailService
  {
    void SendEmail(EmailDto request);
  }
}
