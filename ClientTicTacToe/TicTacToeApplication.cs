using ClientTicTacToe.Handlers;
using ClientTicTacToe.Menu;
using Microsoft.Extensions.Logging;

namespace ClientTicTacToe
{
    public class TicTacToeApplication
    {
        private readonly MainMenu _mainMenu;

        public TicTacToeApplication()
        {
            _mainMenu = new MainMenu();
        }

        public int Run()
        {
            try
            {
                _mainMenu.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }

            return 0;
        }
    }
}
