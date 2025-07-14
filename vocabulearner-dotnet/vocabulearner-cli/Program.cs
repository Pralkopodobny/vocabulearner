using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Vocabulearner.Cli.Commands;
using Vocabulearner.Cli.Database;
using Vocabulearner.Cli.Services;

var registrations = new ServiceCollection()
    .AddSingleton<SettingsService>()
    .AddSingleton<VocabDb>();

var app = new CommandApp(new DependencyRegistrar(registrations));
app.Configure(config =>
{
    config.AddCommand<AddCommand>("add");
    config.AddCommand<InitCommand>("init");
    config.AddCommand<QuizCommand>("quiz");
});

return app.Run(args);