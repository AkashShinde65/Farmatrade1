namespace FarmaTrade_OTP_Service.Services;

public interface IOtpService
{
    Task<OtpSendResult> SendOtpAsync(string email);
    Task<bool> VerifyOtpAsync(string email, string otp);
}
