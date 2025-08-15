using Application;
using Persistence;
using Persistence.Contracts;
using Web.Components;
using Web.Middlewares;

namespace Web;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.AddPersistence()
            .AddApplication()
            .AddWeb();

        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.Limits.MaxRequestBodySize = 100_000_000;
        });

        await using var app = builder.Build();

// Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();

        // if (!app.Environment.IsDevelopment())
        // {
            await using var s = app.Services.CreateAsyncScope();

            var scopeFactory = s.ServiceProvider.GetRequiredService<IServiceScopeFactory>();
            await using var scope = scopeFactory.CreateAsyncScope();
            var appDbContext = scope.ServiceProvider.GetRequiredService<IMigrationContext>();
            await appDbContext.MigrateAsync();
        // }


        app.UseAntiforgery();


        app.MapStaticAssets();
        app.UseRouting();
        app.MapControllers();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.UseAntiforgery();


        app.UseHealthChecks(new PathString("/health"));

        await app.RunAsync();
    }
}