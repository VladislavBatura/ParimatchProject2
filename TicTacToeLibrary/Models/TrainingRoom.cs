namespace TicTacToeLibrary.Models
{
    public sealed class TrainingRoom : RoomBase
    {
        private int _playerMove = 1;

        public override int PlayerMove
        {
            get => _playerMove;
            set
            {
                _playerMove = value;
                if (_playerMove == 2)
                {
                    BotMove();
                }
            }
        }

        private void BotMove()
        {
            var goodMove = false;
            while (!goodMove)
            {
                goodMove = TryPlaceNumber(Random.Shared.Next(9), Random.Shared.Next(10));
            }

            _playerMove = 1;
        }
    }
}
