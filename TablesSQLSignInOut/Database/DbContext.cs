using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using TablesSQLSignInOut.Database;
using TablesSQLSignInOut.Models;

public class YourDbContext : DbContext
{
    public YourDbContext(DbContextOptions<YourDbContext> options)
        : base(options)
    {
    }
    //protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
    //{
    //   optionBuilder.UseSqlServer("Data Source= 100.117.170.73; Database = SignInOutData; User Id = ; Password = Gjholli&2201; TrustServerCertificate = True;")
    //        .AddInterceptors(new AuditInticeptor());

    //}

    public DbSet<Employee> Employees { get; set; }

    public DbSet<AuditEntity> AuditEntries { get; set; }
}

