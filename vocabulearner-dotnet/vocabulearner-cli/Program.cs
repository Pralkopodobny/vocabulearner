using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Vocabulearner.Cli.Commands;
using Vocabulearner.Cli.Services;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var registrations = new ServiceCollection();
registrations.AddSingleton<IConfigurationRoot>(configuration);

var app = new CommandApp(new DependencyRegistrar(registrations));
app.Configure(config =>
{
    config.AddCommand<AddCommand>("add");
});

return app.Run(args);