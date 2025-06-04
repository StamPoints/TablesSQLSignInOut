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
        private readonly List<AuditEntry> _auditEntries = new List<AuditEntry>();

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
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
            if (eventData.Context != null)
            {
                var endTime = DateTime.UtcNow;

                foreach (var auditEntry in _auditEntries)
                {
                    auditEntry.EndTimeUtc = endTime;
                    auditEntry.Succeeded = true;
                }

                if (_auditEntries.Any())
                {
                    eventData.Context.Set<AuditEntry>().AddRange(_auditEntries);
                    _auditEntries.Clear();
                    await eventData.Context.SaveChangesAsync(cancellationToken);
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