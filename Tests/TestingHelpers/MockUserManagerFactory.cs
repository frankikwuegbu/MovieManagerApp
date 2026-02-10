using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Tests.TestingHelpers
{
    public static class MockUserManagerFactory
    {
        public static Mock<UserManager<TUser>> Create<TUser>()
            where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();

            var options = Options.Create(new IdentityOptions());
            var passwordHasher = new PasswordHasher<TUser>();

            var userValidators = new List<IUserValidator<TUser>>
        {
            new UserValidator<TUser>()
        };

            var passwordValidators = new List<IPasswordValidator<TUser>>
        {
            new PasswordValidator<TUser>()
        };

            var keyNormalizer = new UpperInvariantLookupNormalizer();
            var errorDescriber = new IdentityErrorDescriber();
            var services = new Mock<IServiceProvider>().Object;
            var logger = new Mock<ILogger<UserManager<TUser>>>().Object;

            return new Mock<UserManager<TUser>>(
                store.Object,
                options,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errorDescriber,
                services,
                logger
            );
        }
    }
}