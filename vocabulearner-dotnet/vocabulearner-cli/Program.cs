using Spectre.Console.Cli;
using Vocabulearner.Cli.Commands;


var app = new CommandApp();
app.Configure(config =>
{
    config.AddCommand<AddCommand>("add");
});

return app.Run(args);