using ServerTicTacToe.Interfaces;
using TicTacToeLibrary;
using TicTacToeLibrary.Models;
using ServerTicTacToe.Models;

namespace ServerTicTacToe.Services
{
    public class FileHostedService : IHostedService
    {
        private readonly IFileSystemProvider _fileSystemProvider;
        private readonly JsonHandler _jsonHandler;
        private readonly ILogger<FileHostedService> _logger;
        private readonly IWebHostEnvironment _env;
        public FileHostedService(IFileSystemProvider provider,
            JsonHandler json,
            ILogger<FileHostedService> logger,
            IWebHostEnvironment env)
        {
            _fileSystemProvider = provider;
            _jsonHandler = json;
            _logger = logger;
            _env = env;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var path = Path.Combine(_env.WebRootPath, Server.RegisteredPlayers);
            if (_fileSystemProvider.Exists(path))
            {
                try
                {
                    _logger.LogInformation("Try to read register file");
                    using var data = _fileSystemProvider.Read(path);
                    var collectionPlayers = (List<Player>)_jsonHandler.Deserialize(data);
                    foreach (var obj in collectionPlayers)
                    {
                        _ = Storage.Players.TryAdd(obj.Name, obj);
                    }
                }
                catch
                {
                    _logger.LogError("Can't operate with registered players file");
                }
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
