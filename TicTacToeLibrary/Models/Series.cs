using System.Text;

namespace TicTacToeLibrary.Models
{
    public class Series
    {
        public List<Round> Rounds { get; set; } = new();
        public DateTime Date { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            _ = sb.Append("Session:\nDate: " + Date + "\n");

            foreach (var round in Rounds)
            {
                _ = sb.Append(round + "\n");
            }

            return sb.ToString();
        }
    }
}
