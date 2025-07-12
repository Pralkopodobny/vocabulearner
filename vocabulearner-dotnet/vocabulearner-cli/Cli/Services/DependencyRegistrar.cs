using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Vocabulearner.Cli.Services;

public class DependencyRegistrar(IServiceCollection builder) : ITypeRegistrar
{
    public ITypeResolver Build()
    {
        return new DependencyResolver(builder.BuildServiceProvider());
    }

    public void Register(Type service, Type implementation)
    {
        builder.AddSingleton(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        builder.AddSingleton(service, implementation);
    }

    public void RegisterLazy(Type service, Func<object> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        builder.AddSingleton(service, (provider) => func());
    }
}