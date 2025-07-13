using System.ComponentModel;
using System.Text.Json;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using Spectre.Console.Cli;
using Vocabulearner.Cli.Database;
using Vocabulearner.Cli.Database.Dto;

namespace Vocabulearner.Cli.Commands;

[UsedImplicitly]
public class AddCommand(VocabDb db) : Command<AddCommand.Settings>
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
        using var file = File.OpenRead(settings.ImportPath);
        var quiz = JsonSerializer.Deserialize<QuizFile>(file);
        if (quiz == null)
        {
            AnsiConsole.MarkupLine("[red]Invalid import path specified.[/]");
            return 1;
        }

        var id = db.WithConnection(connection =>
        {
            connection.Open();
            var transaction = connection.BeginTransaction();
            var id = QuizRepository.Add(new QuizDto {Description = quiz.Description ?? "", Name = quiz.Name}, connection, transaction);

            var questions = quiz.Questions.Select((question, i) => new Question
            {
                No = i, QuizId = id, Contents = question.Contents, Answer = question.Answer
            });
            
            QuestionRepository.Add(questions, connection, transaction);
            transaction.Commit();
            
            return id;
        });
        
        AnsiConsole.MarkupLineInterpolated($"[grey]Created quiz {id}.[/]");
        
        return 0;
    }

    public override ValidationResult Validate(CommandContext context, Settings settings)
    {
        return !File.Exists(settings.ImportPath) ? ValidationResult.Error($"File {settings.ImportPath} does not exist") : base.Validate(context, settings);
    }

    private class QuizFile
    {
        public string Name { get; init; } = null!;
        public string? Description { get; init; }
        public List<QuizQuestion> Questions { get; init; } = [];
        public class QuizQuestion
        {
            public required string Contents { get; init; }
            public required string Answer { get; init; }
        }
    }
}