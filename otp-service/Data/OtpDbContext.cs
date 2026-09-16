
using Microsoft.EntityFrameworkCore;

namespace FarmaTrade_OTP_Service.Data;

public class OtpDbContext : DbContext
{
    public OtpDbContext(DbContextOptions<OtpDbContext> options)
        : base(options)
    {
    }

    public DbSet<OtpEntity> OtpRecords => Set<OtpEntity>();
}