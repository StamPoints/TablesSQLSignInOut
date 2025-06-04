using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

// This namespace contains the AuditInterceptor class which listens for EF Core SaveChanges events
namespace TablesSQLSignInOut.Database
{
    // AuditInterceptor inherits from SaveChangesInterceptor to hook into EF Core lifecycle events
    public class AuditInterceptor : SaveChangesInterceptor
    {
        // This list temporarily stores AuditEntry records during the SaveChanges process
        private readonly List<AuditEntry> _auditEntries = new List<AuditEntry>();

        // This method is called right before SaveChangesAsync runs in EF Core
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            // Retrieve the current DbContext from the event data
            var context = eventData.Context;

            // Only proceed if the context is not null
            if (context != null)
            {
                // Capture the start time of the save operation
                var startTime = DateTime.UtcNow;

                // Get all changed entities that are not already AuditEntry records
                // and are being Added, Modified, or Deleted
                var entries = context.ChangeTracker.Entries()
                    .Where(e => !(e.Entity is AuditEntry) &&
                                (e.State == EntityState.Added ||
                                 e.State == EntityState.Modified ||
                                 e.State == EntityState.Deleted))
                    .Select(e => new AuditEntry
                    {
                        // Set the start time and include metadata about the change
                        StartTimeUtc = startTime,
                        MetaData = e.DebugView.LongView
                    })
                    .ToList();

                // Store these audit entries in the internal list for later processing
                _auditEntries.AddRange(entries);
            }

            // Continue with the normal save operation
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        // This method is called after SaveChangesAsync completes successfully
        public override async ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {
            // Ensure the context is still valid
            if (eventData.Context != null)
            {
                // Capture the end time of the save operation
                var endTime = DateTime.UtcNow;

                // Update each stored audit entry to record the end time and success status
                foreach (var auditEntry in _auditEntries)
                {
                    auditEntry.EndTimeUtc = endTime;
                    auditEntry.Succeeded = true;
                }

                // If there are any audit entries to save
                if (_auditEntries.Any())
                {
                    // Add the audit entries to the AuditEntries table
                    eventData.Context.Set<AuditEntry>().AddRange(_auditEntries);

                    // Clear the list after storing to the database to avoid duplication
                    _auditEntries.Clear();

                    // Persist the audit entries to the database
                    await eventData.Context.SaveChangesAsync(cancellationToken);
                }
            }

            // Continue with the normal save flow
            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        // This method is called if SaveChanges fails
        public override void SaveChangesFailed(DbContextErrorEventData eventData)
        {
            // Log the error message to the console
            Console.WriteLine($"SaveChanges failed: {eventData.Exception.Message}");

            // Call base implementation
            base.SaveChangesFailed(eventData);
        }
    }
}
