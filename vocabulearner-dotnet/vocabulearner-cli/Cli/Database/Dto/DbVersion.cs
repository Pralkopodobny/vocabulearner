namespace Vocabulearner.Cli.Database.Dto;

public readonly record struct DbVersion
{
    public int Major { get; init; }
    public int Minor { get; init; }
}