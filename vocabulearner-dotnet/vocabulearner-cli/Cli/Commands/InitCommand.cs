using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using Spectre.Console.Cli;
using Vocabulearner.Cli.Database;

namespace Vocabulearner.Cli.Commands;

[UsedImplicitly]
public class InitCommand(VocabDb db) : Command<EmptyCommandSettings>
{
    public override int Execute(CommandContext context, EmptyCommandSettings settings)
    {
        switch (db.CheckVersion())
        {
            case VocabDb.DbValidation.Ok:
                AnsiConsole.WriteLine("Database is already initialized.");
                break;
            case VocabDb.DbValidation.Missing:
                AnsiConsole.WriteLine("Initializing database...");
                db.CreateDatabase();
                AnsiConsole.MarkupLine("[green]Database initialized.[/]");
                break;
            case VocabDb.DbValidation.Corrupted:
                AnsiConsole.WriteLine("Database is corrupted.");
                break;
            case VocabDb.DbValidation.Invalid:
                AnsiConsole.WriteLine("Database is invalid.");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        return 0;
    }
}