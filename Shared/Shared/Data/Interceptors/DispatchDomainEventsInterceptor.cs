using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.DDD;

namespace Shared.Data.Interceptors
{
    public class DispatchDomainEventsInterceptor(IMediator mediator):SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispatchEvents(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await DispatchEvents(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private async Task DispatchEvents(DbContext? context)
        {
            if (context == null)return;

            var aggragates=context.ChangeTracker.Entries<IAggregate>()
                           .Where(a=>a.Entity.DomainEvents.Any())
                           .Select(a=>a.Entity);

            var domainEvents=aggragates.SelectMany(a=>a.DomainEvents).ToList();

            aggragates.ToList().ForEach(a=>a.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }
        }
    }
}