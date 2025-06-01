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

        // The method signature for SavedChangesAsync is incorrect. 
        // The correct override in SaveChangesInterceptor is:

        public override ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }
        // The method signature is incorrect. 
        // The correct override for SaveChangesFailed in SaveChangesInterceptor is:

      
        public override Task SaveChangesFailedAsync(
            DbContextErrorEventData eventData,
            CancellationToken cancellationToken = default)
        {
            return base.SaveChangesFailedAsync(eventData, cancellationToken);
        }

       


    }
}