using Microsoft.AspNetCore.Identity;
using Moq;

namespace Tests
{
    public static class MockUserManagerFactory
    {
        public static Mock<UserManager<TUser>> Create<TUser>(TUser? user = default)
        where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();

            var mock = new Mock<UserManager<TUser>>(store.Object,
            null!, null!, null!, null!, null!, null!, null!, null!);

            // Tests will configure specific setups (e.g., CreateAsync, FindByEmailAsync) as needed.
            return mock;
        }
    }
}