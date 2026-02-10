using Application.Entities;
using Application.Interfaces;

namespace Application.Events;

public record UserRegisteredEvent(User User) : IDomainEvent;