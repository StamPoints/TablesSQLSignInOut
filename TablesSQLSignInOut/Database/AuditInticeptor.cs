using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TablesSQLSignInOut.Database
{
   
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

                var StartTime = DateTime.UtcNow;

                var auditEntries = eventData.Context.ChangeTracker.Entries()
     .Where(x => !(x.Entity is AuditEntry) &&
                 (x.State == EntityState.Added ||
                  x.State == EntityState.Modified ||
                  x.State == EntityState.Deleted))
     .Select(x => new AuditEntry
     {
        
         StartTimeUtc = StartTime,
         MetaData = x.DebugView.LongView 
     })
     .ToList();



                foreach (var entry in context.ChangeTracker.Entries())
                {
                    if (entry.State == EntityState.Added ||
                        entry.State == EntityState.Modified ||
                        entry.State == EntityState.Deleted)
                    {
                        var entityName = entry.Entity.GetType().Name;
                        var state = entry.State.ToString();

                        Console.WriteLine($"Entity: {entityName}, State: {state}");

                        foreach (var property in entry.Properties)
                        {
                            var propertyName = property.Metadata.Name;
                            var currentValue = property.CurrentValue;
                            var originalValue = property.OriginalValue;

                            Console.WriteLine($"    Property: {propertyName}");
                            Console.WriteLine($"        Original Value: {originalValue}");
                            Console.WriteLine($"        Current Value: {currentValue}");
                        }
                    }
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"SaveChanges completed successfully. {result} entities saved.");
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        public override void SaveChangesFailed(DbContextErrorEventData eventData)
        {
            Console.WriteLine($"SaveChanges failed: {eventData.Exception.Message}");
            base.SaveChangesFailed(eventData);
        }
    }
}
