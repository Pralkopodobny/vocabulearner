using Microsoft.Extensions.Configuration;

namespace Vocabulearner.Cli.Services;

public class SettingsService
{
    public string DatabasePath {private set; get;}

    public SettingsService()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .Build();
        
        DatabasePath = configuration["DatabasePath"] ?? "vocab.db";
    }
}