namespace Vocabulearner.Cli.Database.Dto;

public record QuizDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
}