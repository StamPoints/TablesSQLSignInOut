using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Threading;
using System.Threading.Tasks;

public class AuditInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context != null)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    Console.WriteLine($"Entity Added: {entry.Entity.GetType().Name}");
                }
                else if (entry.State == EntityState.Modified)
                {
                    Console.WriteLine($"Entity Modified: {entry.Entity.GetType().Name}");
                }
                else if (entry.State == EntityState.Deleted)
                {
                    Console.WriteLine($"Entity Deleted: {entry.Entity.GetType().Name}");
                }
            }
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
