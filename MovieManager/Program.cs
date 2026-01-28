using MovieManager.ExceptionHandling;
using MovieManager.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.AddDependencies();

var app = builder.Build();

await app.IdentityRoleSeedingAsync();

app.UseSwaggerGen();

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();