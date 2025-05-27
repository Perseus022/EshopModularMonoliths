namespace Shared.Data.Interceptors;

public class DispatchDomainEventsInterseptor(IMediator mediator)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEvents(DbContext? context)
    {
        if (context is null) return;
        var agrigates = context.ChangeTracker
            .Entries<IAggregate>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity);

        var domainEvents = agrigates
            .SelectMany(e => e.DomainEvents)
            .ToList();

        agrigates.ToList().ForEach(e => e.ClearDomainEvents());


        foreach (var domainEvent in domainEvents)
        {
            // Publish the domain event using MediatR
            // This will ensure that the event is handled by any registered handlers

            await mediator.Publish(domainEvent);
        }
    }
}
