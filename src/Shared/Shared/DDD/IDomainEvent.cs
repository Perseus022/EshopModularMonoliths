using MediatR;

namespace Shared.DDD;

public interface IDomainEvent : INotification
{
    Guid EveentId => Guid.NewGuid(); // Unique identifier for the event
    DateTime OccurredOn => DateTime.Now; // Timestamp when the event occurred
    string EventType => GetType().AssemblyQualifiedName!; // Type of the event, can be used for logging or processing purposes
}
