namespace FarmaTrade_OTP_Service.Configuration;

public class EmailSettings
{
    public string SmtpServer { get; set; } = "smtp.gmail.com";

    public int Port { get; set; } = 587;

    public string Username { get; set; } = string.Empty;

    public string AppPassword { get; set; } = string.Empty;

    public string FromName { get; set; } = "FarmaTrade";
}