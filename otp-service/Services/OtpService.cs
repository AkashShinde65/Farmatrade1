using System.Security.Cryptography;
using FarmaTrade_OTP_Service.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FarmaTrade_OTP_Service.Services;

public class OtpService : IOtpService
{
    private readonly OtpDbContext _dbContext;
    private readonly IEmailService _emailService;
    private readonly ILogger<OtpService> _logger;

    private const int OtpExpiryMinutes = 5;
    private const int ResendCooldownSeconds = 60;

    public OtpService(
        OtpDbContext dbContext,
        IEmailService emailService,
        ILogger<OtpService> logger)
    {
        _dbContext = dbContext;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<OtpSendResult> SendOtpAsync(string email)
    {
        email = email.Trim().ToLowerInvariant();

        var latestOtp = await _dbContext.OtpRecords
            .Where(x => x.Email == email)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();

        if (latestOtp != null &&
            (DateTime.UtcNow - latestOtp.CreatedAt).TotalSeconds < ResendCooldownSeconds)
        {
            return OtpSendResult.Cooldown;
        }

        var otp = RandomNumberGenerator
            .GetInt32(100000, 1000000)
            .ToString();

        var otpRecord = new OtpEntity
        {
            Email = email,
            OtpHash = BCrypt.Net.BCrypt.HashPassword(otp),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(OtpExpiryMinutes),
            IsUsed = false
        };

        _dbContext.OtpRecords.Add(otpRecord);
        await _dbContext.SaveChangesAsync();

        try
        {
            await _emailService.SendOtpEmailAsync(email, otp);
            return OtpSendResult.Sent;
        }
        catch (Exception ex)
        {
            otpRecord.IsUsed = true;
            await _dbContext.SaveChangesAsync();

            _logger.LogError(ex, "OTP email delivery failed for {Email}", email);
            return OtpSendResult.EmailFailed;
        }
    }

    public async Task<bool> VerifyOtpAsync(string email, string otp)
    {
        email = email.Trim().ToLowerInvariant();

        var record = await _dbContext.OtpRecords
            .Where(x => x.Email == email && !x.IsUsed)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();

        if (record == null || record.ExpiresAt <= DateTime.UtcNow)
        {
            return false;
        }

        if (!BCrypt.Net.BCrypt.Verify(otp, record.OtpHash))
        {
            return false;
        }

        record.IsUsed = true;
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
