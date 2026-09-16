namespace FarmaTrade_OTP_Service.Data;

public class OtpEntity
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string OtpHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }
}