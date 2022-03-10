using Microsoft.AspNetCore.Mvc;
using ServerTicTacToe.Services;
using TicTacToeLibrary.Models;
using ServerTicTacToe.Models;
using ServerTicTacToe.Interfaces;
using TicTacToeLibrary;

namespace ServerTicTacToe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HubController : ControllerBase
    {
        private readonly ILogger<HubController> _logger;
        private readonly IFileSystemProvider _fileSystemProvider;
        private readonly JsonHandler _jsonHandler;
        private readonly IWebHostEnvironment _env;

        public HubController(ILogger<HubController> logger,
            IFileSystemProvider fileSystemProvider,
            JsonHandler json,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _fileSystemProvider = fileSystemProvider;
            _jsonHandler = json;
            _env = env;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            _logger.HubIndex();
            return Ok();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(Player playerPost)
        {
            _logger.HubRegisterStart(playerPost.Name);
            if (!ModelState.IsValid)
            {
                _logger.HubRegisterModelError(playerPost.Name);
                ModelState.AddModelError(nameof(Player), "Invalid player credentials");
                return BadRequest(ModelState);
            }

            Player? player = null;
            if (Storage.Players.TryGetValue(playerPost.Name, out var value))
            {
                player = value;
            }

            if (player == null)
            {
                player = new Player
                {
                    Name = playerPost.Name,
                    Password = playerPost.Password,
                    RegisterTime = DateTime.UtcNow,
                    Id = Guid.NewGuid(),
                    //GamesCounter = new()
                };
                _logger.HubRegisterCreateNewAccount(player.Name);
            }
            else
            {
                _logger.HubRegisterAccountExists(playerPost.Name);
                ModelState.AddModelError(nameof(Player), "This player already exists");
                return BadRequest(ModelState);
            }

            if (!Storage.Players.TryAdd(player.Name, player))
            {
                _logger.HubRegisterError(playerPost.Name);
                ModelState.AddModelError(nameof(Player), "Can't register player");
                return BadRequest(ModelState);
            }

            UpdatePlayers();

            _logger.HubRegisterSuccess(playerPost.Name);

            return Ok(player);
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(Player playerPost)
        {
            if (!ModelState.IsValid)
            {
                _logger.HubLoginModelError(playerPost.Name);
                return BadRequest();
            }

            if (Storage.PlayersOnline.ContainsKey(playerPost.Name))
            {
                _logger.HubLoginError(playerPost.Name);
                _ = Storage.PlayersOnline.TryRemove(playerPost.Name, out _);
                return BadRequest("You've already in system, kicking you out from system");
            }

            if (Storage.TimerOnFailedLogin.ContainsKey(playerPost.Name))
            {
                _logger.HubLoginBlocked(playerPost.Name);
                return BadRequest("Your account has been blocked for 5 minutes");
            }

            if (Storage.FailedLogins.TryGetValue(playerPost.Name, out var attempts) && attempts > 2)
            {
                _logger.HubLoginBlocked(playerPost.Name);
                var failTimer = new FailedLogin(playerPost.Name, _logger);
                failTimer.Start();
                _ = Storage.TimerOnFailedLogin.TryAdd(playerPost.Name, failTimer);
                return BadRequest();
            }

            Player? player = null;
            if (Storage.Players.TryGetValue(playerPost.Name, out var value))
            {
                player = value;
            }

            if (player == null)
            {
                return NotFound();
            }
            if (!player.Password.Equals(playerPost.Password, StringComparison.Ordinal))
            {
                _ = Storage.FailedLogins.AddOrUpdate(playerPost.Name, 1, (name, value) => value + 1);
                ModelState.AddModelError(nameof(playerPost), "Wrong password");
                return NotFound(ModelState);
            }

            return !Storage.PlayersOnline.TryAdd(player.Name, player) ? BadRequest() : Ok(player);
        }

        [HttpPost]
        [Route("Sessions/{id}")]
        public IActionResult Sessions(Guid id, Series series)
        {
            var player = Storage.Players.Values.SingleOrDefault(x => x.Id == id);

            if (player is null)
            {
                return BadRequest();
            }

            var updatedPlayer = player;
            updatedPlayer.Series.Add(series);

            _ = Storage.Players.TryUpdate(player.Name, updatedPlayer, player);

            UpdatePlayers();
            _logger.LogInformation($"Player {id} series list updated");

            return Ok();
        }

        [HttpGet]
        [Route("GetPlayer/{id}")]
        public IActionResult GetPlayer(Guid id)
        {
            var player = Storage.Players.Values.SingleOrDefault(x => x.Id == id);

            if (player is null)
            {
                return BadRequest();
            }

            return Ok(player);
        }

        [HttpPost]
        [Route("Logout")]
        public IActionResult LogOut(Player player)
        {
            return !Storage.PlayersOnline.TryRemove(player.Name, out _) ? NotFound() : Ok();
        }

        [HttpGet]
        [Route("Top")]
        public IActionResult Top()
        {
            var list = Storage.Players.Values
                .OrderByDescending(x => x.Total)
                .ThenByDescending(x => x.Wins)
                .Where(x => x.Total > 10)
                .Take(10)
                .Select(x => new string(x.Name + " - games: " + x.Total))
                .ToList();

            return list is null || list.Count == 0 ? Ok("There is no top") : Ok(list);
        }

        private void UpdatePlayers()
        {
            var data = _jsonHandler.SerializeCollection(Storage.Players.Values.ToList());
            using var str = new MemoryStream(data);
            var path = Path.Combine(_env.WebRootPath, Server.RegisteredPlayers);
            _fileSystemProvider.WriteAsync(path, str).Wait();
        }
    }
}
