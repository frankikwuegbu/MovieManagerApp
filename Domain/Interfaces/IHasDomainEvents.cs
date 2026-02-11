namespace Domain.Interfaces;

public interface IHasDomainEvents
{
    IReadOnlyCollection<object> DomainEvents { get; }
    void AddDomainEvent(object domainEvent);
    void ClearDomainEvents();
}