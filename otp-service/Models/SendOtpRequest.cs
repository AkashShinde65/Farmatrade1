using System.ComponentModel.DataAnnotations;

namespace FarmaTrade_OTP_Service.Models;

public class SendOtpRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}