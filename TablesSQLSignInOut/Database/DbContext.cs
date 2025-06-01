using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;

public class YourDbContext : DbContext
{
    public YourDbContext(DbContextOptions<YourDbContext> options)
        : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
    {
       optionBuilder.UseSqlServer("Data Source= 100.117.170.73; Database = SignInOutData; User Id = sa; Password = Gjholli & 2201; TrustServerCertificate = True;").AddInterceptors(new AuditInterceptor());

    }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<AuditEntity> AuditEntries { get; set; }
}
public class AuditEntity
{
     public int ID { get; set; }

     public string MetaData { get; set; }

    public DateTime StartTimeUtc { get; set; }

    public DateTime EndTimeUtc { get; set; }

    public bool Succeeded { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;
}
public class Employee
{
    public int ID { get; set; }
    public int WorkID { get; set; } // Unique identifier for the employee 
    public string Name { get; set; } = string.Empty; 
    public string FamilyName { get; set; } = string.Empty;
    public int Age { get; set; }    // in years
    public string Email { get; set; } = string.Empty;
    public string UserPage { get; set; } = string.Empty;
    public int WorkTimeTotal { get; set; } // in minutes
    public string JobTitle { get; set; } = string.Empty;    
    public string privileges { get; set; } = string.Empty;
}
