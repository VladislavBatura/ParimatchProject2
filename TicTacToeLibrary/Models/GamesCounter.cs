using System.Globalization;

namespace TicTacToeLibrary.Models
{
    public class GamesCounter
    {
        public int Wins { get; set; }
        public int Loses { get; set; }
        public int TotalGames => Wins + Loses;
        public string WinLoseRatio => string.Format(CultureInfo.InvariantCulture, "{0:0.00}", Wins / (double)Loses);
    }
}
