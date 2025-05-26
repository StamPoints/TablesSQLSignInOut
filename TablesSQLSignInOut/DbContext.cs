using Microsoft.EntityFrameworkCore;

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
    public string UserPage { get; set; }
    public int WorkTimeTotal { get; set; }
    public string JobTitle { get; set; }
    public string Privilege { get; set; }
}
