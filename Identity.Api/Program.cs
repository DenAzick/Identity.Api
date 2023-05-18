using Identity.Api.Context;
using Identity.Api.Extentions;
using Identity.Api.MiddleWares;
using Identity.Api.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .WriteTo.File("logs.txt",
    LogEventLevel.Information,
    rollingInterval: RollingInterval.Day);


//builder.Logging.AddSerilog(logger);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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