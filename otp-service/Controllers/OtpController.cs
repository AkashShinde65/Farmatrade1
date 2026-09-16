using FarmaTrade_OTP_Service.Models;
using FarmaTrade_OTP_Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace FarmaTrade_OTP_Service.Controllers;

[ApiController]
[Route("api/otp")]
public class OtpController : ControllerBase
{
    private readonly IOtpService _otpService;

    public OtpController(IOtpService otpService)
    {
        _otpService = otpService;
    }

    [HttpPost("send")]
    public async Task<ActionResult<OtpResponse>> SendOtp(SendOtpRequest request)
    {
        var result = await _otpService.SendOtpAsync(request.Email);

        return result switch
        {
            OtpSendResult.Sent => Ok(new OtpResponse
            {
                Success = true,
                Message = "OTP sent successfully."
            }),
            OtpSendResult.Cooldown => StatusCode(429, new OtpResponse
            {
                Success = false,
                Message = "Please wait before requesting another OTP."
            }),
            _ => StatusCode(502, new OtpResponse
            {
                Success = false,
                Message = "Unable to send OTP email."
            })
        };
    }

    [HttpPost("verify")]
    public async Task<ActionResult<OtpResponse>> VerifyOtp(VerifyOtpRequest request)
    {
        var result = await _otpService.VerifyOtpAsync(
            request.Email,
            request.Otp);

        if (!result)
        {
            return BadRequest(new OtpResponse
            {
                Success = false,
                Message = "Invalid or expired OTP."
            });
        }

        return Ok(new OtpResponse
        {
            Success = true,
            Message = "OTP verified successfully."
        });
    }
}
