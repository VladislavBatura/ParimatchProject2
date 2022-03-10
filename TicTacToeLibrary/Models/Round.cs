namespace TicTacToeLibrary.Models
{
    public class Round
    {
        public string? Winner { get; set; }
        public string? Loser { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return $"Winner - {Winner} | Loser - {Loser} | Date - {Date}";
        }
    }
}
