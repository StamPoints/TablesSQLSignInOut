using Microsoft.EntityFrameworkCore;


  public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
}

public class SqlServerDbContext : DbContext
{
    public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
        : base(options)
    {
    }


    public DbSet<Employee> Employees { get; set; }
}
public class MySqlDbContext : DbContext
{
    public MySqlDbContext(DbContextOptions<MySqlDbContext> options)
        : base(options)
    {
    }

    // Define DbSet properties for MySQL tables here
}


public class Employee
{
    public int ID { get; set; }
    public int WorkID { get; set; }
    public string Name { get; set; }
    public string FamilyName { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string UserPage { get; set;}
    public int WorkTimeTotal { get; set; }
    public string JobTitle { get; set; }
    public string privileges { get; set; }
}

public class User
{
    public int WorkID { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}