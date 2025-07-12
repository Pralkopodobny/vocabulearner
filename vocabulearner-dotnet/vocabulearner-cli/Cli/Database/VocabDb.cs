using Dapper;
using Microsoft.Data.Sqlite;
using Vocabulearner.Cli.Database.Dto;
using Vocabulearner.Cli.Services;

namespace Vocabulearner.Cli.Database;

public class VocabDb(SettingsService settings)
{
    private const int Major = 0;
    private const int Minor = 1;

    private SqliteConnection CreateConnection()
    {
        return new SqliteConnection($"Data Source={settings.DatabasePath}");
    }
    
    public void WithConnection(Action<SqliteConnection> action)
    {
        using var connection = CreateConnection();
        action(connection);
    }
    
    public T WithConnection<T>(Func<SqliteConnection, T> action)
    {
        using var connection = CreateConnection();
        return action(connection);
    }

    public void CreateDatabase()
    {
        using var connection = CreateConnection();
        connection.Open();
        var transaction = connection.BeginTransaction();
        connection.Execute("""
                           CREATE TABLE DbVersions
                           (
                               Major INT NOT NULL,
                               Minor INT NOT NULL,
                               PRIMARY KEY (Major, Minor)
                           )
                           """, transaction: transaction);

        connection.Execute("INSERT INTO DbVersions (Major, Minor) VALUES (@Major, @Minor)",
            new DbVersion { Major = Major, Minor = Minor },
            transaction: transaction);
        
        connection.Execute("""
                           CREATE TABLE Quizes
                           (
                               Id INT PRIMARY KEY NOT NULL,
                               Name TEXT NOT NULL,
                               Description TEXT NOT NULL
                           )
                           """, transaction: transaction);
        
        connection.Execute("""
                           CREATE TABLE Questions
                           (
                           No INT NOT NULL,
                           QuizId INT NOT NULL,
                           Contents TEXT NOT NULL,
                           Answer TEXT NOT NULL,
                           PRIMARY KEY (No, QuizId)
                           )
                           """, transaction: transaction);
        transaction.Commit();
    }

    public DbValidation CheckVersion()
    {
        try
        {
            var version = WithConnection(GetVersion);
            return version switch
            {
                null => DbValidation.Missing,
                { Major: Major, Minor: Minor } => DbValidation.Ok,
                _ => DbValidation.Invalid
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return DbValidation.Corrupted;
        }
    }

    public enum DbValidation
    {
        Ok,
        Missing,
        Corrupted,
        Invalid,
    }
    
    private DbVersion? GetVersion(SqliteConnection connection)
    {
        var hasVersionTable = connection.QueryFirst<bool>("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='DbVersions'");
        if (!hasVersionTable)
        {
            return null;
        }
        return connection.QueryFirstOrDefault<DbVersion>("SELECT * FROM DbVersions ORDER BY Major, Minor DESC LIMIT 1");
    }
}