namespace Shared.DDD;

public interface IAggregate<T> : IEntity<T>, IAggregate
    //where T : notnull // Ensures that T is a non-nullable type
{
    // This interface combines the properties of IEntity<T> and IAggregate
    // It allows for aggregates to have a specific identifier type while also being an aggregate root
}
public interface IAggregate: IEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; } // Collection of domain events associated with the aggregate

    IDomainEvent[] ClearDomainEvents(); // Method to clear the domain events after processing 

}
