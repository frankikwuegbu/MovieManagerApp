using Application.Interface;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System.Linq.Expressions;
namespace Tests.Helpers;


public static class MockAppDbContextFactory
{
    public static Mock<IApplicationDbContext> Create()
    {
        var mockContext = new Mock<IApplicationDbContext>();

        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);

        return mockContext;
    }

    public static void SetupDbSet<TEntity>(
        Mock<IApplicationDbContext> mockContext,
        Expression<Func<IApplicationDbContext, DbSet<TEntity>>> dbSetExpression,
        List<TEntity> data)
        where TEntity : class
    {
        mockContext.Setup(dbSetExpression)
                   .ReturnsDbSet(data);
    }
}