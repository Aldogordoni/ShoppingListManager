using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Claims;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Services;
public class JwtGenerator
{
    readonly RsaSecurityKey _key;
    private readonly IConfiguration configuration;
    public JwtGenerator(IConfiguration configuration)
    {
        this.configuration = configuration;
        RSA privateRSA = RSA.Create();
        privateRSA.ImportPkcs8PrivateKey(Convert.FromBase64String(this.configuration["Authentication:JwtPrivateSigningKey"]), out _);
        _key = new RsaSecurityKey(privateRSA);
    }

    public string CreateUserAuthToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = configuration["Authentication:Audience"],
            Issuer = configuration["Authentication:Issuer"],
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Sid, userId.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.RsaSha256)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
