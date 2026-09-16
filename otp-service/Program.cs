using FarmaTrade_OTP_Service.Configuration;
using FarmaTrade_OTP_Service.Data;
using FarmaTrade_OTP_Service.Middleware;
using FarmaTrade_OTP_Service.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("DefaultConnection is not configured.");
}

builder.Services.AddDbContext<OtpDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)));

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OtpDbContext>();
    db.Database.Migrate();
}

app.UseMiddleware<InternalTokenMiddleware>();

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new
{
    service = "FarmaTrade OTP Service",
    status = "UP"
}));

app.Run();
