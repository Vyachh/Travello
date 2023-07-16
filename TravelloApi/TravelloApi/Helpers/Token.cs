using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravelloApi.Models;

namespace TravelloApi.Helpers
{
  public class Token
  {
    //private readonly IConfiguration configuration;

    //public Token(IConfiguration configuration)
    //{
    //  this.configuration = configuration;
    //}
    public static string CreateToken(User user, IConfiguration configuration)
    {
      List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Sid, user.CurrentTripId.ToString() ?? string.Empty),
                new Claim(ClaimTypes.UserData, user.Id ?? string.Empty),
            };

      var key = new SymmetricSecurityKey(Encoding.UTF8
        .GetBytes(configuration
        .GetSection("AppSettings:Token").Value!));

      var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var token = new JwtSecurityToken(
          claims: claims,
          expires: DateTime.UtcNow.AddDays(1),
          signingCredentials: cred
          );

      return new JwtSecurityTokenHandler().WriteToken(token);

    }
    public static IDictionary<string, string> DecodeJwtToken(string token)
    {

      var jwtHandler = new JwtSecurityTokenHandler();
      var middle = token.Replace("Bearer ", "");
      var jwtToken = jwtHandler.ReadJwtToken(middle);

      var claims = new Dictionary<string, string>();
      foreach (var claim in jwtToken.Claims)
      {
        claims.Add(claim.Type, claim.Value);
      }

      return claims;
    }
  }
}
