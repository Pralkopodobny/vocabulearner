namespace Vocabulearner.Cli.Database.Dto;

public record Question
{
    public required int No { get; init; }
    public required int QuizId { get; init; }
    public required string Contents { get; init; }
    public required string Answer { get; init; }
}