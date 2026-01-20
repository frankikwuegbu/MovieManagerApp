namespace MovieManager.Startup;

public static class SwaggerConfig
{
    public static void AddSwaggerGenServices(this IServiceCollection services)
    {
        services.AddSwaggerGen();
    }

    public static void UseSwaggerGen(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
