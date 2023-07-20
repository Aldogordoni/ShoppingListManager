using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Business;

public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly IConfiguration configuration;
    public ConfigureJwtBearerOptions(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void Configure(string name, JwtBearerOptions options)
    {
        RSA rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(this.configuration["Authentication:PublicKey"]), out _);

        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new RsaSecurityKey(rsa),
            ValidateIssuer = true,
            ValidIssuer = this.configuration["Authentication:Issuer"],
            ValidateAudience = true,
            ValidAudience = this.configuration["Authentication:Audience"],
            CryptoProviderFactory = new CryptoProviderFactory()
            {
                CacheSignatureProviders = false
            }
        };
    }

    public void Configure(JwtBearerOptions options)
    {
        throw new System.NotImplementedException();
    }
}