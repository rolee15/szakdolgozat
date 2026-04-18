using KanjiKa.Api.Filters;
using Microsoft.OpenApi.Models;

namespace KanjiKa.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "KanjiKa API",
                Version = "v1",
                Description =
                    "REST API for the KanjiKa Japanese Hiragana/Katakana learning platform.\n\n" +
                    "**How to authenticate:**\n" +
                    "1. Call `POST /api/users/login` with your credentials.\n" +
                    "2. Copy the `token` value from the response.\n" +
                    "3. Click the **Authorize** button at the top and paste the token.\n\n" +
                    "All endpoints marked with a lock icon require a valid JWT token."
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token (without the 'Bearer ' prefix)."
            });

            options.OperationFilter<AuthorizeOperationFilter>();
        });

        return services;
    }

    public static WebApplication UseSwaggerConfiguration(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
            return app;

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "KanjiKa API v1");
            options.DisplayRequestDuration();
        });

        return app;
    }
}
