using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;

public class YourDbContext : DbContext
{
    
    public DbSet<Employee> Employees { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server = 100.117.170.73; Database = SignInOutData; User Id = StamPoints; Password = Gjholli & 2201;");
    }

    
}


public class Employee
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string FamilyName { get; set; }
    public int Age { get; set; }
    public int WorkID { get; set; }
    public string Email { get; set; }
    public DateTime SignInTime { get; set; }
    public DateTime SignOutTime { get; set; }
}

namespace TablesSQLSignInOut
{
    public class DbContext
    {
    }
}
