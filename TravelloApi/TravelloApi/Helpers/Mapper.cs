using AutoMapper;
using TravelloApi.Dto;
using TravelloApi.Models;

namespace TravelloApi.Helpers
{
  public class Mapper : Profile
  {
    public Mapper()
    {
      CreateMap<UserDto, User>();
    }
  }
}
