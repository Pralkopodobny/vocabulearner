using Dapper;
using Microsoft.Data.Sqlite;
using Vocabulearner.Cli.Database.Dto;

namespace Vocabulearner.Cli.Database;

public static class QuestionRepository
{
    public static void Add(Question question, SqliteConnection connection, SqliteTransaction? transaction)
    {
        connection.Execute("INSERT INTO Questions (No, QuizId, Name, Translation) VALUES (@No, @QuizId, @Contents, @Answer)", question, transaction);
    }
    
    public static void Add(IEnumerable<Question> questions, SqliteConnection connection, SqliteTransaction? transaction)
    {
        foreach (var question in questions)
        {
            Add(question, connection, transaction);
        }
    }
}