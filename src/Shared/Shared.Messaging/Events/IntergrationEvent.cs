namespace Shared.Messaging.Events;

public record IntergrationEvent
{
    public Guid EventId => Guid.NewGuid();
    public DateTime OccuredOn => DateTime.Now;
    public string EventType => GetType().AssemblyQualifiedName;
}
