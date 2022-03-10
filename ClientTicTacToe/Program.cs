using ClientTicTacToe;
using ClientTicTacToe.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

IServiceCollection services = new ServiceCollection();
services.AddLogging(config =>
{
    _ = config.AddFile("Logs/TicTacToeClient-{Date}.log");
});

var provider = services.BuildServiceProvider();

var hubLogger = provider.GetService<ILogger<HubHandler>>();
var roomLogger = provider.GetService<ILogger<RoomHandler>>();
var trainingLogger = provider.GetService<ILogger<TrainingRoomHandler>>();

Handler.SetHandlers(hubLogger, roomLogger, trainingLogger);

return new TicTacToeApplication().Run();
