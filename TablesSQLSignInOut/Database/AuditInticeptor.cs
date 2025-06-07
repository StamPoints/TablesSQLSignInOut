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
        private readonly List<AuditEntry> _auditEntries = new();
        private bool _isSavingAuditEntries;

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (_isSavingAuditEntries)
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            var context = eventData.Context;
            if (context != null)
            {
                var startTime = DateTime.UtcNow;

                var entries = context.ChangeTracker.Entries()
                    .Where(e => !(e.Entity is AuditEntry) &&
                                (e.State == EntityState.Added ||
                                 e.State == EntityState.Modified ||
                                 e.State == EntityState.Deleted))
                    .Select(e => new AuditEntry
                    {
                        StartTimeUtc = startTime,
                        MetaData = e.DebugView.LongView
                    })
                    .ToList();

                _auditEntries.AddRange(entries);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override async ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {
            if (_auditEntries.Any() && eventData.Context != null)
            {
                var endTime = DateTime.UtcNow;

                foreach (var auditEntry in _auditEntries)
                {
                    auditEntry.EndTimeUtc = endTime;
                    auditEntry.Succeeded = true;
                }

                try
                {
                    _isSavingAuditEntries = true;
                    eventData.Context.Set<AuditEntry>().AddRange(_auditEntries);
                    _auditEntries.Clear();
                    await eventData.Context.SaveChangesAsync(cancellationToken);
                }
                finally
                {
                    _isSavingAuditEntries = false;
                }
            }

            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }
    }
}