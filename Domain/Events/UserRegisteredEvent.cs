using Domain.Common;
using Domain.Entities;
using MediatR;
using System.Globalization;

namespace Domain.Events;

public class UserRegisteredEvent(string fullName, string? userName, UserRoles roles) : BaseEntity, INotification
{
    public string FullName { get; set; } = fullName;
    public override string? UserName { get; set; } = userName;
    public UserRoles Roles { get; set; } = roles;
}