using Persistence;
using Web.Middlewares;

namespace Web;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddWeb(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks();
        builder.Services.AddControllers();
        builder.Services.AddScoped<ExceptionMiddleware>();

        var corsAllowedOrigins = builder.Configuration.GetSection("CorsAllowedOrigins").Get<string[]>();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("default", policyBuilder =>
            {
                if (corsAllowedOrigins != null)
                    policyBuilder
                        .WithOrigins(corsAllowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders("Content-Disposition");
            });
        });
        builder.Services.AddScoped(sp =>
            new HttpClient { BaseAddress = new Uri(builder.Configuration["ASPNETCORE_URLS"]!.Split(";").First()) });

        return builder;
    }
}