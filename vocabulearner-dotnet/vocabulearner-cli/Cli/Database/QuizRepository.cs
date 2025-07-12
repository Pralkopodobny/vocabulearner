using Dapper;
using Microsoft.Data.Sqlite;
using Vocabulearner.Cli.Database.Dto;

namespace Vocabulearner.Cli.Database;

public static class QuizRepository
{
    public static void Add(QuizDto quiz, SqliteConnection connection, SqliteTransaction? transaction = null)
    {
        connection.Execute("INSERT INTO Quizes(Name, Description) VALUES (@Name, @Description)", quiz, transaction);
    }
    
    public static void RemoveByName(string name, SqliteConnection connection, SqliteTransaction? transaction = null)
    {
        connection.Execute("DELETE FROM Quizes WHERE Name = @name", new {name}, transaction);
    }
}