namespace ServerTicTacToe.Models
{
    public class GameSettings
    {
        public int RoundTime { get; private set; }
        public int InactiveTime { get; private set; }

        public GameSettings(int roundTime, int inactiveTime)
        {
            RoundTime = (roundTime < 1 ? 20 : roundTime) * 1000;

            InactiveTime = (inactiveTime < 1 ? 120 : inactiveTime) * 1000;
        }
    }
}
