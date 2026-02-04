using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Domain.Common;

public class BaseEntity : IdentityUser, IHasDomainEvents
{
    private readonly List<object> domainEvents = [];

    public void AddDomainEvent(object domainEvent)
    {
        domainEvents.Add(domainEvent);
    }

    public IReadOnlyCollection<object> DomainEvents => domainEvents.AsReadOnly();

    public void ClearDomainEvents() => domainEvents.Clear();
}