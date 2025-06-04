using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using TablesSQLSignInOut.Database;
using TablesSQLSignInOut.Models;

public class YourDbContext : DbContext
{
    private readonly List<AuditEntry> _auditEntries;
    
    private readonly AuditInterceptor _auditInterceptor;

    //public AuditInterceptor(List<AuditEntry> auditEntries)
    //{
    //    _auditEntries = auditEntries;
    //}


    //public YourDbContext(DbContextOptions<YourDbContext> options, List<AuditEntry> auditEntries)
    //    : base(options)
    //{
    //    _auditEntries = auditEntries;
    //}



    public YourDbContext(DbContextOptions<YourDbContext> options, AuditInterceptor auditInterceptor)
        : base(options)
    {
        _auditInterceptor = auditInterceptor;
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditInterceptor);
    }



    public DbSet<Employee> Employees { get; set; }
    public DbSet<AuditEntry> AuditEntries { get; set; }
}

public class Employee
{
    public int ID { get; set; }
    public int WorkID { get; set; }
    public string Name { get; set; }
    public string FamilyName { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string UserPage { get; set; }
    public int WorkTimeTotal { get; set; }
    public string JobTitle { get; set; }
    public string privileges { get; set; }
}
public class AuditEntry
{
    public int ID { get; set; }
    public string? MetaData { get; set; }
    public DateTime StartTimeUtc { get; set; }
    public DateTime EndTimeUtc { get; set; }
    public bool Succeeded { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}