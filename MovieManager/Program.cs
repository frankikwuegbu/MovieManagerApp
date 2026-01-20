using MovieManager.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.AddDependencies();

var app = builder.Build();

await app.IdentityRoleSeedingAsync();

app.UseSwaggerGen();

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();