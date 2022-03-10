using System.Net;
using Microsoft.Extensions.Logging;
using TicTacToeLibrary;
using TicTacToeLibrary.Models;

using static ClientTicTacToe.Handlers.Handler;

namespace ClientTicTacToe.Handlers
{
    public class RoomHandler
    {
        private readonly ILogger<RoomHandler> _logger;

        public RoomHandler(ILogger<RoomHandler> logger)
        {
            _logger = logger;
        }

        public async Task<Room?> CreateRoom(Player player, bool isPrivate = false)
        {
            var roomPost = new Room { Id = Guid.NewGuid(), Player1 = player, IsPrivate = isPrivate };
            var (_, result) = await Http.PostAsync<Room, Room>(Server.RoomCreate, roomPost);

            if (result is not null)
            {
                _logger.LogInformation("A new room was created");
            }
            else
            {
                _logger.LogWarning("Room creation error");
            }

            return result;
        }

        public async Task<(HttpStatusCode status, Room? result)> PublicConnect(Player player)
        {
            var response = await Http.PostAsync<Room, Player>(Server.PublicConnect, player);
            return response;
        }

        public async Task<(HttpStatusCode status, Room? result)> PrivateConnect(Guid id, Player player)
        {
            var response = await Http.PostAsync<Room, Player>(Server.PrivateConnect + id, player);
            return response;
        }

        public async Task<Room?> GetRoom(Guid id)
        {
            var (_, content) = await Http.GetAsync(Server.RoomGet + id);

            var room = content.Deserialize<Room>();
            return room;
        }

        public void CloseRoom(Guid id)
        {
            _ = Http.GetAsync(Server.RoomClose + id);
        }

        public async Task<HttpStatusCode> Move(int cell, int number, Room room)
        {
            var status = await Http.PostAsync(Server.RoomMove + $"{cell}/{number}", room);
            return status;
        }

        public async Task<HttpStatusCode> Update(Room room)
        {
            var status = await Http.PostAsync(Server.RoomUpdate, room);
            return status;
        }
    }
}
