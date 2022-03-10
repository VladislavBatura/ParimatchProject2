using System.Net;
using ClientTicTacToe.Handlers;
using TicTacToeLibrary.Models;

namespace ClientTicTacToe.Services
{
    internal class PlayerFinder
    {
        private Timer? _timer;
        private Room? _room;

        //set from configuration
        private readonly int _millisecondsTimeout = 30_000;

        public async Task<Room?> FindPublic(Player player)
        {
            _room = null;
            var (status, result) = await Handler.Room.PublicConnect(player);

            _room = status is HttpStatusCode.BadRequest
                ? await Handler.Room.CreateRoom(player) : result;

            WaitPlayer(_millisecondsTimeout);
            return _room;
        }

        public async Task<Room?> FindPrivate(Player player)
        {
            _room = await Handler.Room.CreateRoom(player, true);
            Console.WriteLine($"Your key: {_room?.Id}");

            WaitPlayer(60_000);
            return _room;
        }

        public async Task<Room?> ConnectToPrivate(Player player)
        {
            _room = null;
            Console.Write("Enter key: ");
            var input = Console.ReadLine();

            if (!Guid.TryParse(input, out var id))
            {
                Console.WriteLine("Invalid key format");
                return null;
            }

            _room = await Handler.Room.GetRoom(id);

            if (_room is null || Equals(_room?.Id, Guid.Empty))
            {
                Console.WriteLine("There is no room with this key");
                return null;
            }

            var (_, result) = await Handler.Room.PrivateConnect(id, player);
            _room = result;

            _ = IsPlayerFound(_room?.Player1 is not null && _room.Player2 is not null);
            return result;
        }

        private void WaitPlayer(int millisecondsTimeout)
        {
            _timer = new Timer(Callback, null, 0, 1_000);

            Console.WriteLine("Waiting for the players...");
            var playerFound = SpinWait.SpinUntil(() => _room?.Player2 is not null, millisecondsTimeout);

            _timer.Dispose();

            _ = IsPlayerFound(playerFound);
        }

        private bool IsPlayerFound(bool condition)
        {
            if (condition)
            {
                Console.WriteLine("Player found");
                Console.WriteLine($"Players:\n1 - {_room?.Player1?.Name}\n2 - {_room?.Player2?.Name}");
                return true;
            }

            Handler.Room.CloseRoom(_room.Id);
            Console.WriteLine("There are currently no active players ;(");
            return false;
        }

        private void Callback(object? o)
        {
            _room = Handler.Room.GetRoom(_room!.Id).Result;
        }
    }
}
