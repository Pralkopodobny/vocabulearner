using Dapper;
using Microsoft.Data.Sqlite;
using Vocabulearner.Cli.Database.Dto;

namespace Vocabulearner.Cli.Database;

public static class QuizRepository
{
    public static int Add(QuizDto quiz, SqliteConnection connection, SqliteTransaction? transaction = null)
    {
        return connection.QueryFirst<int>("INSERT INTO Quizes(Name, Description) VALUES (@Name, @Description) RETURNING Id", quiz, transaction);
    }
    
    public static void RemoveByName(string name, SqliteConnection connection, SqliteTransaction? transaction = null)
    {
        connection.Execute("DELETE FROM Quizes WHERE Name = @name", new {name}, transaction);
    }
}