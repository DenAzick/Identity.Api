using Identity.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Api.Extentions;

public static class ServiceCollectionExtentions
{
    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOption>(configuration.GetSection("JwtBearer"));


        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
    {
        var signInKey = System.Text.Encoding.UTF32.GetBytes("JwtBearer:SigninKey");

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = configuration["JwtBearer:ValidIssuer"],
            ValidAudience = configuration["JwtBearer:ValidAudience"],
            ValidateIssuer = true,
            ValidateAudience = true,
            IssuerSigningKey = new SymmetricSecurityKey(signInKey),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };


    });
    }

}
