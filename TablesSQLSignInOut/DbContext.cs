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

    public DbSet<Employee> Employees { get; set; }
    public DbSet<AuditEntity> AuditEntries { get; set; }
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
