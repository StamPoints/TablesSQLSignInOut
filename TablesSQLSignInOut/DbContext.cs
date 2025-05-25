using Microsoft.EntityFrameworkCore;

public class YourDbContext : DbContext
{
    public YourDbContext(DbContextOptions<YourDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
}

public class Employee
{
    public int ID { get; set; }
    public int WorkID { get; set; }
    public string Name { get; set; }
    public string FamilyName { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public DateTime SignInOutTime { get; set; }
    public bool InOutWork { get; set; }
    public int SessionId { get; set; }
    public string UserPage { get; set; }
}
    