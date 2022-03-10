namespace TicTacToeLibrary.Models
{
    public abstract class RoomBase
    {
        public Guid Id { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsDraw { get; set; }
        public bool IsSeriesClose { get; set; }
        public Player? Player1 { get; set; }
        public Player? Player2 { get; set; }
        public string? Winner { get; set; }
        public int RoundTime { get; set; }
        public int SeriesTime { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual int PlayerMove { get; set; } = 1;
        public int?[] Field { get; set; } = new int?[9];
        public int[] PlayersMoves { get; set; } = new int[9];

        public bool TryPlaceNumber(int cell, int number)
        {
            if (Field.All(x => !x.HasValue))
            {
                UpdatedAt = DateTime.UtcNow;
            }

            if (Winner is not null || IsDraw)
                return false;

            if (cell is < 0 or > 8 || number is < 0 or > 9)
                return false;

            if (!Field[cell].HasValue)
            {
                Field[cell] = number;
            }
            else
            {
                if (PlayersMoves[cell] == PlayerMove || Field[cell]!.Value >= number)
                {
                    return false;
                }

                Field[cell] = number;
            }
            PlayersMoves[cell] = PlayerMove;
            CheckMoveTime();

            return true;
        }

        public void ViewField()
        {
            for (var i = 0; i < Field.Length; i++)
            {
                if (!Field[i].HasValue)
                {
                    Console.ResetColor();
                    Console.Write(' ');
                    PrintBorder(i);
                    continue;
                }

                if (PlayersMoves[i] == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(Field[i]!.Value);
                }

                if (PlayersMoves[i] == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(Field[i]!.Value);
                }

                PrintBorder(i);
            }

            Console.ResetColor();
        }

        private void CheckMoveTime()
        {
            var moveTime = MoveTime();

            if (moveTime is ExpectationResult.SeriesClosing)
            {
                IsSeriesClose = true;
            }

            if (moveTime is not ExpectationResult.Success)
            {
                Winner = PlayerMove == 1 ? Player1?.Name : Player2?.Name;
            }
            else
            {
                ChangeMove();
                CheckWinner();
            }
        }

        private ExpectationResult MoveTime()
        {
            var moveDate = DateTime.UtcNow;
            var totalSeconds = (int)(moveDate - UpdatedAt).TotalSeconds;

            return totalSeconds > SeriesTime / 1000
                ? ExpectationResult.SeriesClosing
                : totalSeconds > RoundTime / 1000
                ? ExpectationResult.RoundCompletion : ExpectationResult.Success;
        }

        private void CheckWinner()
        {
            if (Field.All(x => x.HasValue))
            {
                IsDraw = true;
                return;
            }

            if (Winner is not null)
            {
                return;
            }

            //1|2|3
            //4|5|6
            //7|8|9
            for (var i = 0; i < 3; i++)
            {
                if ((PlayersMoves[i * 3] != 0 &&
                    PlayersMoves[i * 3] == PlayersMoves[(i * 3) + 1] &&
                    PlayersMoves[i * 3] == PlayersMoves[(i * 3) + 2]) ||
                    (PlayersMoves[i] != 0 &&
                    PlayersMoves[i] == PlayersMoves[i + 3] &&
                    PlayersMoves[i] == PlayersMoves[i + 6]))
                {
                    AssignWinner();
                }
            }

            // Manually check for these combinations 0,4,8 || 2,4,6
            if ((PlayersMoves[0] != 0 &&
                PlayersMoves[0] == PlayersMoves[4] &&
                PlayersMoves[0] == PlayersMoves[8]) ||
                (PlayersMoves[2] != 0 &&
                PlayersMoves[2] == PlayersMoves[4] &&
                PlayersMoves[2] == PlayersMoves[6]))
            {
                AssignWinner();
            }
        }

        private void PrintBorder(int i)
        {
            if (i is 2 or 5 or 8)
            {
                Console.WriteLine();
            }
            else
            {
                Console.ResetColor();
                Console.Write('|');
            }
        }

        private void ChangeMove()
        {
            UpdatedAt = DateTime.UtcNow;
            PlayerMove = PlayerMove == 1 ? 2 : 1;
        }

        private void AssignWinner()
        {
            switch (PlayerMove)
            {
                case 1:
                    Winner = Player1?.Name;
                    break;
                case 2:
                    Winner = Player2?.Name;
                    break;
                default:
                    break;
            }
        }
    }
}
