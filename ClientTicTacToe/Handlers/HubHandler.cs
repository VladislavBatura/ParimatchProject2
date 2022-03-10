using System.Net;
using Microsoft.Extensions.Logging;
using TicTacToeLibrary;
using TicTacToeLibrary.Models;
using static ClientTicTacToe.Handlers.Handler;

namespace ClientTicTacToe.Handlers
{
    public class HubHandler
    {
        private readonly ILogger<HubHandler> _logger;
        private readonly HttpClient _httpClient = new();

        public HubHandler(ILogger<HubHandler> logger)
        {
            _logger = logger;
        }

        public bool Health()
        {
            //Need refactoring
            while (true)
            {
                try
                {
                    var response = _httpClient.GetAsync(Server.HubIndex).Result;

                    if (response.IsSuccessStatusCode)
                        return true;
                }
                catch (AggregateException)
                {
                    "Server is not available.".ColoredMessage(ConsoleColor.DarkRed);
                    "1. Restart\nAny key. Exit".ColoredMessage(ConsoleColor.DarkYellow);
                    Console.Write("Enter Action: ");

                    if (Console.ReadLine() != "1")
                    {
                        return false;
                    }
                }
            }
        }

        public async Task Register()
        {
            var playerPost = PlayerForm();
            var (status, _) = await Http.PostAsync<Player, Player>(Server.HubRegister, playerPost);

            switch (status)
            {
                case HttpStatusCode.OK:
                    Console.WriteLine("Registration is successful");
                    _logger.LogInformation($"{playerPost} registration is successful");
                    break;
                case HttpStatusCode.BadRequest:
                    Console.WriteLine("Registration error. User data is invalid");
                    _logger.LogWarning($"{playerPost} registration error. User data is invalid");
                    break;
            }
        }

        public async Task<Player?> Login()
        {
            var playerPost = PlayerForm();
            var (status, result) = await Http.PostAsync<Player, Player>(Server.HubLogin, playerPost);

            switch (status)
            {
                case HttpStatusCode.OK:
                {

                    Console.WriteLine("Login is successful");
                    _logger.LogInformation($"{result} login is successful");
                    return result;
                }
                default:
                {
                    Console.WriteLine("Failed to find user data on the server");
                    _logger.LogWarning("Failed to find user data on the server");

                    return null;
                }
            }
        }

        public async Task Logout(Player player)
        {
            await Http.PostAsync(Server.HubLogout, player);
        }

        public async Task ViewTop()
        {
            var (status, content) = await Http.GetAsync(Server.HubTop);

            switch (status)
            {
                case HttpStatusCode.OK:
                    Console.WriteLine(content);
                    break;
                default:
                    _logger.LogWarning("Top request Error");
                    break;
            }
        }

        public void UpdateSessions(Guid id, Series series)
        {
            _ = Http.PostAsync(Server.HubUpdateSessions + id, series);
        }

        public async Task<Player?> GetPlayer(Guid id)
        {
            var (status, content) = await Http.GetAsync(Server.HubGetPlayer + id);

            if (status == HttpStatusCode.OK)
            {
                return content.Deserialize<Player>();
            }

            return null;
        }

        private Player PlayerForm()
        {
            Console.Write("Enter name: ");
            var name = Console.ReadLine();

            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            return new Player { Name = name, Password = password };
        }
    }
}
