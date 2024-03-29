using Identity.Api.Context;
using Identity.Api.Extentions;
using Identity.Api.MiddleWares;
using Identity.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Identity.Api.MiddleWares;


var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .WriteTo.File("logs.txt",
    LogEventLevel.Information,
    rollingInterval: RollingInterval.Day).CreateLogger();


  builder.Logging.AddSerilog(logger);




builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "JWT Bearer. : \"Authorization: Bearer { token }\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("FreeAspHost"));
});


builder.Services.AddScoped<TokenService>();
builder.Services.AddJwt(builder.Configuration);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}



app.UseCors(c =>
	c.AllowAnyOrigin()
		.AllowAnyHeader()
		.AllowAnyMethod());

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.UseErrorHandlerMiddlewere();

app.MapControllers();

app.Run();

public static class ErrorHandlerMiddlewereExtentions
{
    public static IApplicationBuilder UseErrorHandlerMiddlewere(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlerMiddlewere>();
    }
}