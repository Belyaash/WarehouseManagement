using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMediatR(static c =>
            {
                c.RegisterServicesFromAssemblies(
                    Assembly.GetCallingAssembly(),
                    Assembly.GetExecutingAssembly());
            }
        );
        return builder;
    }
}