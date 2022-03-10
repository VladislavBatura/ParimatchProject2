using System.Net;
using ClientTicTacToe.Handlers;
using TicTacToeLibrary.Models;

namespace ClientTicTacToe.Services
{
    internal class Game
    {
        private Room _room;
        private Timer _timer;
        private readonly Series _series = new() { Date = DateTime.UtcNow };
        private readonly Player _player;

        public Game(Room room, Player player)
        {
            _room = room;
            _player = player;
        }

        public void Play()
        {
            while (true)
            {
                GameStart();
                var round = CreateRound();

                if (!_room.IsSeriesClose && PlayAgain())
                {
                    _room = Handler.Room.GetRoom(_room.Id).Result!;

                    if (!_room.IsPlaying)
                    {
                        Console.WriteLine("Opponent left the game");
                        _series.Rounds.Add(round);
                        break;
                    }

                    NewGame();
                    _series.Rounds.Add(round);
                    continue;
                }

                _series.Rounds.Add(round);
                break;
            }

            Handler.Hub.UpdateSessions(_player.Id, _series);

            if (_player.Name != _room.Winner)
            {
                Handler.Room.CloseRoom(_room.Id);
            }
        }

        private void GameStart()
        {
            Console.WriteLine("Game on!");
            while (_room.Winner is null && !_room.IsDraw)
            {
                _room.ViewField();

                if (_room.PlayerMove == _player.NumberInGame)
                {
                    if (!Move())
                        continue;
                }
                else
                {
                    Wait();
                }

                Console.Clear();
            }

            GameResult();
        }

        private bool Move()
        {
            Console.WriteLine("Your move: ");
            Console.Write("Enter cell: ");
            var cell = GetValidNumber() - 1;

            Console.Write("Enter number: ");
            var number = GetValidNumber();

            if (_room.Winner is not null)
            {
                return false;
            }

            var status = Handler.Room.Move(cell, number, _room).Result;
            _room = Handler.Room.GetRoom(_room.Id).Result!;

            return status != HttpStatusCode.BadRequest;
        }

        private void Wait()
        {
            Console.WriteLine("Opponent move...");
            _timer = new Timer(Callback, null, 0, 1_000);
            SpinWait.SpinUntil(() => _room.PlayerMove == _player.NumberInGame || _room.Winner is not null || _room.IsDraw);

            _timer.Dispose();
        }

        private void Callback(object? o)
        {
            _room = Handler.Room.GetRoom(_room.Id).Result!;
        }

        private int GetValidNumber()
        {
            int n;
            while (!int.TryParse(Console.ReadLine(), out n))
            {
                Console.Write("Invalid number. Try again: ");
            }

            return n;
        }

        private bool PlayAgain()
        {
            Console.WriteLine("Do you want play again?\n\"yes\" or \"no\"");

            while (true)
            {
                switch (Console.ReadLine()?.ToLowerInvariant())
                {
                    case "yes":
                        return true;
                    case "no":
                        _room.IsPlaying = false;
                        _ = Handler.Room.Update(_room);
                        return false;
                    default:
                        Console.WriteLine("Enter \"yes\" or \"no\"");
                        break;
                }
            }
        }

        private void GameResult()
        {
            if (_room.IsDraw)
            {
                Console.WriteLine("Draw");
            }
            else
            {
                Console.WriteLine("Winner: " + _room.Winner);
            }
        }

        private Round CreateRound()
        {
            if (_room.IsDraw)
            {
                return new Round {Winner = null, Loser = null, Date = DateTime.UtcNow};
            }

            var loser = _room.Winner == _room.Player1?.Name ? _room.Player2?.Name : _room.Player1?.Name;
            return new Round {Winner = _room.Winner!, Loser = loser, Date = DateTime.UtcNow};
        }

        private void NewGame()
        {
            _room.Winner = null;
            _room.IsDraw = false;
            _room.Field = new int?[9];
            _room.PlayersMoves = new int[9];

            _ = Handler.Room.Update(_room).Result;
        }
    }
}
