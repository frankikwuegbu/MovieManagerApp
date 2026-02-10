using Application.Common;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Entities;

public enum UserRoles
{
    ADMIN = 0,
    USER = 1
}

public class User : IdentityUser, IHasDomainEvents
{
    public required string FullName { get; set; }
    public UserRoles Role { get; set; } = UserRoles.ADMIN;

    private readonly List<object> domainEvents = [];
    public IReadOnlyCollection<object> DomainEvents => domainEvents.AsReadOnly();

    public void AddDomainEvent(object domainEvent)
    {
        domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents() => domainEvents.Clear();
}