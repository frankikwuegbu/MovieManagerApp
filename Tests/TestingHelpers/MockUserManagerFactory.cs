using Microsoft.AspNetCore.Identity;
using Moq;

namespace Tests.TestingHelpers;

public static class MockUserManagerFactory
{
    public static Mock<UserManager<TUser>> Create<TUser>(TUser? user = default)
    where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();

        var mock = new Mock<UserManager<TUser>>(store.Object,
        null!, null!, null!, null!, null!, null!, null!, null!);

        return mock;
    }
}