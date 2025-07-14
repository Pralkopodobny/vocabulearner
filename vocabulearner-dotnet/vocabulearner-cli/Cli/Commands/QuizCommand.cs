using System.ComponentModel;
using JetBrains.Annotations;
using Spectre.Console;
using Spectre.Console.Cli;
using Vocabulearner.Cli.Database;
using Vocabulearner.Cli.Database.Dto;

namespace Vocabulearner.Cli.Commands;

[UsedImplicitly]
public class QuizCommand(VocabDb db) : Command<QuizCommand.Settings>
{
    [UsedImplicitly]
    public sealed class Settings : CommandSettings
    {
        [Description("Name of the quiz")]
        [CommandArgument(0, "<Name>")]
        public string Name { get; init; } = null!;
    }


    public override int Execute(CommandContext context, Settings settings)
    {
        var questions = db.WithConnection(connection => QuestionRepository.GetByQuizName(settings.Name, connection));
        List<Question> mistakes = [];
        
        AnsiConsole.Clear();
        for (var i = 0; i < questions.Count; i += 10)
        {
            mistakes = Round(questions.Slice(i, int.Min(10, questions.Count - i)), mistakes);
        }

        return 0;
    }

    private List<Question> Round(IEnumerable<Question> questions, IEnumerable<Question> mistakes)
    {
        List<Question> newMistakes = [];
        foreach (var question in questions.Concat(mistakes))
        {
            
            if (AskQuestion(question))
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"[green]correct[/] {question.Contents} is {question.Answer}");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red] incorrect[/] [underline]{question.Contents} is {question.Answer}[/]");
                
                if (AnsiConsole.Prompt(new TextPrompt<bool>("Got it?")
                            .AddChoice(true)
                            .AddChoice(false)
                            .DefaultValue(true)
                            .WithConverter(c => c ? "yes" : "bullshit")))
                {
                    newMistakes.Add(question);
                }
                
                AnsiConsole.Clear();
            }
        }

        foreach (var mistake in newMistakes)
        {
            while (AskQuestion(mistake) == false)
            {
                AnsiConsole.MarkupLine($"[red] incorrect[/] [underline]{mistake.Contents} is {mistake.Answer}[/]");
                AnsiConsole.Ask<string>("Got it?");
            }
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"[green]correct[/] {mistake.Contents} is {mistake.Answer}");
        }
        return newMistakes;
    }

    private bool AskQuestion(Question question)
    {
        var answer = AnsiConsole.Ask<string>($"Translate [underline]{question.Contents}[/]");
        return answer == question.Answer;
    }
}