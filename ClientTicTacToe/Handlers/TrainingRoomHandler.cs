using System.Net;
using Microsoft.Extensions.Logging;
using TicTacToeLibrary;
using TicTacToeLibrary.Models;
using static ClientTicTacToe.Handlers.Handler;

namespace ClientTicTacToe.Handlers
{
    public class TrainingRoomHandler
    {
        private readonly ILogger<TrainingRoomHandler> _logger;

        public TrainingRoomHandler(ILogger<TrainingRoomHandler> logger)
        {
            _logger = logger;
        }

        public async Task<TrainingRoom?> CreateTraining(Player player)
        {
            var roomPost = new TrainingRoom { Id = Guid.NewGuid(), Player1 = player };
            var (_, result) = await Http.PostAsync<TrainingRoom, TrainingRoom>(Server.TrainingRoomCreate, roomPost);

            if (result is not null)
            {
                _logger.LogInformation("A new TrainingRoom was created");
            }
            else
            {
                _logger.LogWarning("TrainingRoom creation error");
            }

            return result;
        }

        public async Task<TrainingRoom?> GetRoom(Guid id)
        {
            var (_, content) = await Http.GetAsync(Server.TrainingRoomGet + id);

            var room = content.Deserialize<TrainingRoom>();
            return room;
        }

        public void CloseRoom(Guid id)
        {
            _ = Http.GetAsync(Server.TrainingRoomClose + id);
        }

        public async Task<HttpStatusCode> Move(int cell, int number, TrainingRoom room)
        {
            var status = await Http.PostAsync(Server.TrainingRoomMove + $"{cell}/{number}", room);
            return status;
        }
    }
}
