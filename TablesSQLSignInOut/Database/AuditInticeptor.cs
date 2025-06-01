using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TablesSQLSignInOut.Database


{
    public class AuditInticeptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var context = eventData.Context;
            if (context != null)
            {
                foreach (var entry in context.ChangeTracker.Entries())
                {
                    // Add your audit logic here, e.g.:
                    // if (entry.State == EntityState.Added) { ... }
                }
            }
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override ValueTask<int> SavedChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override SavedChagesFailed(EventData);



    }
}