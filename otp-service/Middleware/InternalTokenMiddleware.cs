using System.Security.Cryptography;

namespace FarmaTrade_OTP_Service.Middleware;

public class InternalTokenMiddleware
{
    private const string TokenHeader = "X-Internal-Token";

    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly ILogger<InternalTokenMiddleware> _logger;

    public InternalTokenMiddleware(
        RequestDelegate next,
        IConfiguration configuration,
        ILogger<InternalTokenMiddleware> logger)
    {
        _next = next;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/health"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Path.StartsWithSegments("/api/otp"))
        {
            await _next(context);
            return;
        }

        var expectedToken = _configuration["INTERNAL_SERVICE_TOKEN"];

        if (string.IsNullOrWhiteSpace(expectedToken))
        {
            _logger.LogError("INTERNAL_SERVICE_TOKEN is not configured.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message = "Internal service authentication is not configured."
            });
            return;
        }

        if (!context.Request.Headers.TryGetValue(TokenHeader, out var providedToken) ||
            string.IsNullOrWhiteSpace(providedToken))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var provided = providedToken.ToString();

        if (!CryptographicOperations.FixedTimeEquals(
                System.Text.Encoding.UTF8.GetBytes(provided),
                System.Text.Encoding.UTF8.GetBytes(expectedToken)))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next(context);
    }
}
