using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Portfolio.WebApi.Security.Token;
public class SigningConfigurations
{
  public SecurityKey SecurityKey { get; }
  public SigningCredentials SigningCredentials { get; }

  public SigningConfigurations(string key)
  {
    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"])),
    byte[] keyBytes = Encoding.ASCII.GetBytes(key);
    SecurityKey = new SymmetricSecurityKey(keyBytes);
    SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);
  }
}

