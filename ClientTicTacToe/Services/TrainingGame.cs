using System.Net;
using ClientTicTacToe.Handlers;
using TicTacToeLibrary.Models;

namespace ClientTicTacToe.Services
{
    public class TrainingGame
    {
        private TrainingRoom _room;
        private readonly Player _player;

        public TrainingGame(TrainingRoom room, Player player)
        {
            _room = room;
            _player = player;
        }

        public void Play()
        {
            GameStart();

            Handler.TrainingRoom.CloseRoom(_room.Id);
        }

        private void GameStart()
        {
            Console.WriteLine("Game on!");
            while (_room.Winner is null && !_room.IsDraw)
            {
                _room.ViewField();

                if (!Move())
                    continue;

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

            var status = Handler.TrainingRoom.Move(cell, number, _room).Result;
            Console.WriteLine("Bot move...");
            Thread.Sleep(2000);

            _room = Handler.TrainingRoom.GetRoom(_room.Id).Result!;

            return status != HttpStatusCode.BadRequest;
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
    }
}
