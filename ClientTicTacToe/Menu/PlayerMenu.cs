using ClientTicTacToe.Handlers;
using ClientTicTacToe.Services;
using TicTacToeLibrary.Models;

namespace ClientTicTacToe.Menu
{
    public class PlayerMenu : IMenu
    {
        private readonly PlayerFinder _playerFinder = new();
        private Player _player;
        private Game? _game;
        private TrainingGame _trainingGame;

        public void RegisterPlayer(Player player)
        {
            Console.WriteLine($"Hi {player.Name}!");
            _player = player;
        }

        public void Start()
        {
            var alive = true;
            while (alive)
            {
                "1. Player search\n2. Create private room\n3. Connect to the private room\n4. Statistics\n5. Training\n6. Logout".ColoredMessage(ConsoleColor.DarkYellow);
                Console.Write("Enter number: ");
                switch (Console.ReadLine())
                {
                    case "1":
                    {
                        var room = _playerFinder.FindPublic(_player).Result;
                        _ = TryStartGame(room);
                        break;
                    }
                    case "2":
                    {
                        var room = _playerFinder.FindPrivate(_player).Result;
                        _ = TryStartGame(room);
                        break;
                    }
                    case "3":
                    {
                        var room = _playerFinder.ConnectToPrivate(_player).Result;
                        _ = TryStartGame(room);
                        break;
                    }
                    case "4":
                        WriteStatistics();
                        break;
                    case "5":
                    {
                        var room = Handler.TrainingRoom.CreateTraining(_player).Result;
                        _ = TryStartTraining(room);

                        break;
                    }
                    case "6":
                        _ = Handler.Hub.Logout(_player);
                        alive = false;
                        break;
                    default:
                        Console.WriteLine("Invalid action");
                        break;
                }
            }
        }

        private bool TryStartGame(Room? room)
        {
            if (room?.Player2 == null)
            {
                return false;
            }

            _game = new Game(room, _player.Name == room.Player1?.Name ? room.Player1 : room.Player2);
            _game.Play();
            return true;
        }

        private bool TryStartTraining(TrainingRoom? room)
        {
            if (room is null)
            {
                return false;
            }

            _trainingGame = new TrainingGame(room, _player);
            _trainingGame.Play();
            return true;
        }

        private void WriteStatistics()
        {
            var updatedPlayer = Handler.Hub.GetPlayer(_player.Id).Result;

            if (updatedPlayer is not null)
            {
                _player = updatedPlayer;
                Console.WriteLine(_player.ToString());
            }
            else
            {
                Console.WriteLine("An error has occurred");
            }
        }
    }
}
