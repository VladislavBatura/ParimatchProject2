using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TicTacToeLibrary.Models
{
    public class Player
    {
        //uniq identifier
        public Guid Id { get; set; }
        [Required]
        //login-name
        public string Name { get; set; } = "";
        //regex for minimum 6 letters, at least 1 uppercase, 1 lowercase, 1 number and special character
        [RegularExpression(@"^(?=(.*[a-z]){1,})(?=(.*[A-Z]){1,})(?=(.*[0-9]){1,})(?=(.*[!@#$%^&*()\-__+.]){1,}).{6,}$",
            ErrorMessage = "Password must be greater, than 6 characters, have 1 uppercase letter," +
            " 1 lowercase letter, 1 number and 1 special character")]
        public string Password { get; set; } = "";
        public int NumberInGame { get; set; }
        public List<Series> Series { get; set; } = new();
        public int Wins => Series.Sum(s => s.Rounds.Count(r => r.Winner == Name));
        public int Loses => Series.Sum(s => s.Rounds.Count(r => r.Loser == Name));
        public int Total => Wins + Loses;
        public DateTime RegisterTime { get; set; }
        //public GamesCounter GamesCounter { get; set; } = new();

        public override string ToString()
        {
            var sb = new StringBuilder();
            var str = $"Name: {Name} | Wins: {Wins} | Loses: {Loses} | Total: {Total}\n";

            _ = sb.AppendLine(str);

            foreach (var s in Series)
            {
                _ = sb.Append(s + "\n");
            }

            return sb.ToString();
        }
    }
}
