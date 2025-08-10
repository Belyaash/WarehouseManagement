using Persistence;

namespace Web;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddWeb(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks();

        builder.Services.AddControllers();


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

        return builder;
    }
}