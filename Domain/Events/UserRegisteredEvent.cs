using Application.Entities;
using Application.Interfaces;

namespace Domain.Events;

public record UserRegisteredEvent(User User) : IDomainEvent;