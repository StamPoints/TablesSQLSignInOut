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
        private readonly List<AuditEntry> _auditEntries;
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

 _auditEntries.AddRange(auditEntries);


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

        public override async ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {
            if(eventData.Context == null)
            {
                Console.WriteLine("No DbContext available.");
                return await base.SavedChangesAsync(eventData, result, cancellationToken); ;
            }

            var endTime = DateTime.UtcNow;

            foreach(var auditEntry in _auditEntries)
            {
                auditEntry.EndTimeUtc = endTime;
                auditEntry.Succeeded = true;
               
            }
            if (_auditEntries.Count > 0)
            {
                var context = eventData.Context;
                if (context != null)
                {
                    eventData.Context.Set<AuditEntry>().AddRange(_auditEntries);
                    _auditEntries.Clear(); // Clear the list after saving
                    await eventData.Context.SaveChangesAsync();
                }
            }
            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        public override void SaveChangesFailed(DbContextErrorEventData eventData)
        {
            Console.WriteLine($"SaveChanges failed: {eventData.Exception.Message}");
            base.SaveChangesFailed(eventData);
        }
    }
}
