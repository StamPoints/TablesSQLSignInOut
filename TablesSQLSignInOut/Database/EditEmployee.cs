using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

public class DbEdit
{
    private readonly IDbContextFactory<YourDbContext> _dbContextFactory;
    private readonly IJSRuntime _jsRuntime;
    private bool _isRendered = false;

    public DbEdit(IDbContextFactory<YourDbContext> dbContextFactory, IJSRuntime jsRuntime)
    {
        _dbContextFactory = dbContextFactory;
        _jsRuntime = jsRuntime;
    }

    public void MarkAsRendered()
    {
        _isRendered = true;
    }

    public async Task EditEmployee(int workId)
    {
        try
        {
            using var context = _dbContextFactory.CreateDbContext();
            var employee = await context.Employees.FirstOrDefaultAsync(e => e.WorkID == workId);

            if (employee != null)
            {
                employee.Name = "Kazuma";
                await context.SaveChangesAsync();

                if (_isRendered)
                {
                    await _jsRuntime.InvokeVoidAsync("alert", "Employee details updated successfully.");
                }
            }
            else
            {
                if (_isRendered)
                {
                    await _jsRuntime.InvokeVoidAsync("alert", "Employee not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in EditEmployee: {ex.Message}");
            if (_isRendered)
            {
                await _jsRuntime.InvokeVoidAsync("alert", "An error occurred while updating employee details.");
            }
        }
    }
}
