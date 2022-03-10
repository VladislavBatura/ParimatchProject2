namespace ServerTicTacToe.Services
{
    public static class ArgumentHandler
    {
        public static int TryParseInt(string arg)
        {
            return int.TryParse(arg, out var result) ? result : 0;
        }
    }
}
