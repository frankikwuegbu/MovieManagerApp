using Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Common;

public class BaseEntity : IHasDomainEvents
{
    private readonly List<object> domainEvents = [];

    public void AddDomainEvent(object domainEvent)
    {
        domainEvents.Add(domainEvent);
    }

    public IReadOnlyCollection<object> DomainEvents => domainEvents.AsReadOnly();

    public void ClearDomainEvents() => domainEvents.Clear();
}