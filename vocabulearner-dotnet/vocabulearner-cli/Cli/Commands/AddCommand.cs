using System.ComponentModel;
using JetBrains.Annotations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Vocabulearner.Cli.Commands;

[UsedImplicitly]
public class AddCommand : Command<AddCommand.Settings>
{
    [UsedImplicitly]
    public sealed class Settings : CommandSettings
    {
        [Description("Path to the quizz export file")]
        [CommandArgument(0, "<ImportPath>")]
        public string ImportPath { get; init; } = null!;
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLineInterpolated($"Added [bold green]{settings.ImportPath}[/]");
        return 0;
    }

    public override ValidationResult Validate(CommandContext context, Settings settings)
    {
        return !File.Exists(settings.ImportPath) ? ValidationResult.Error($"File {settings.ImportPath} does not exist") : base.Validate(context, settings);
    }
}