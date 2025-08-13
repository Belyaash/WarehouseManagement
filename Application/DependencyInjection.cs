using System.Reflection;
using Application.Common;
using FluentValidation;
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

                c.AddBehavior(typeof(IPipelineBehavior<,>),
                    typeof(ValidationBehavior<,>));
            }
        );
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return builder;
    }
}