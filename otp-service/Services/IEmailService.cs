namespace FarmaTrade_OTP_Service.Services;

public interface IEmailService
{
    Task SendOtpEmailAsync(string email, string otp);
}