using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Vocabulearner.Cli.Commands;
using Vocabulearner.Cli.Database;
using Vocabulearner.Cli.Services;

var registrations = new ServiceCollection()
    .AddSingleton<SettingsService>()
    .AddSingleton<VocabDb>();

var vocabDb = registrations.BuildServiceProvider().GetRequiredService<VocabDb>();

switch (vocabDb.CheckVersion())
{
    case VocabDb.DbValidation.Ok:
        Spectre.Console.AnsiConsole.MarkupLine("[bold green]Ok[/]");
        break;
    case VocabDb.DbValidation.Missing:
    {
        Spectre.Console.AnsiConsole.MarkupLine("[bold yellow]Creating database[/]");
        vocabDb.CreateDatabase();
        Spectre.Console.AnsiConsole.MarkupLine("[bold green]Database successfully created[/]");
        break;
    }
    case VocabDb.DbValidation.Corrupted:
    case VocabDb.DbValidation.Invalid:
        Spectre.Console.AnsiConsole.MarkupLineInterpolated($"[bold red]{vocabDb.CheckVersion().ToString()}[/]");
        break;
    default:
        throw new ArgumentOutOfRangeException();
}

var app = new CommandApp(new DependencyRegistrar(registrations));
app.Configure(config =>
{
    config.AddCommand<AddCommand>("add");
});

return app.Run(args);