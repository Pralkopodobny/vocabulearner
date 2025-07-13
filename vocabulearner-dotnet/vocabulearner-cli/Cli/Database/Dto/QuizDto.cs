namespace Vocabulearner.Cli.Database.Dto;

public record QuizDto
{
    public int Id { get; init; } = -1;
    public required string Name { get; init; }
    public required string Description { get; init; }
}