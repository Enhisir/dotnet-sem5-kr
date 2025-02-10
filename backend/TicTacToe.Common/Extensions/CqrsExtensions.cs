using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Common.CQRS;

namespace TicTacToe.Common.Extensions;

public static class CqrsExtensions
{
    public static IServiceCollection AddMediatorConfigured(
        this IServiceCollection services, 
        params Assembly[] handlersAssemblies)
    {
        var set = new HashSet<Type>();
        foreach (var assembly in handlersAssemblies)
        {
            foreach (var type in assembly.GetTypes().Where(t => t.IsClass))
            {
                foreach (var inf in type.GetInterfaces())
                {
                    if (inf.IsGenericType && (inf.GetGenericTypeDefinition().IsAssignableTo(typeof(IHandler<>)) ||
                                              inf.GetGenericTypeDefinition().IsAssignableTo(typeof(IHandler<,>))))
                    {
                        services.AddScoped(inf, type);
                        set.Add(inf.GenericTypeArguments[0]);
                    }
                }
            }
        }

        services.AddSingleton<IMediator, ServiceMediator>(provider =>
        {
            var med = new ServiceMediator(provider);
            foreach (var type in set)
                med.AddRequestImpl(type);
            return med;
        });

        return services;
    }
}