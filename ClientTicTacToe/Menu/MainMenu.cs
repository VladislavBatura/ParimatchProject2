using static ClientTicTacToe.Handlers.Handler;

namespace ClientTicTacToe.Menu
{
    internal class MainMenu : IMenu
    {
        private readonly PlayerMenu _playerMenu;

        public MainMenu()
        {
            _playerMenu = new PlayerMenu();
        }

        public void Start()
        {
            if (!Hub.Health())
            {
                return;
            }

            var alive = true;
            while (alive)
            {
                "1. Register\n2. Login\n3. Top players\n4. Exit".ColoredMessage(ConsoleColor.DarkGreen);
                Console.Write("Enter Action: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Hub.Register().Wait();
                        break;
                    case "2":
                    {
                        var player = Hub.Login().Result;

                        if (player is not null)
                        {
                            _playerMenu.RegisterPlayer(player);
                            _playerMenu.Start();
                        }

                        break;
                    }
                    case "3":
                        Hub.ViewTop().Wait();
                        break;
                    case "4":
                        alive = false;
                        break;
                    default:
                        Console.WriteLine("Invalid action");
                        break;
                }
            }
        }
    }
}
