using Microsoft.AspNetCore.Mvc;
using ServerTicTacToe.Models;
using TicTacToeLibrary.Models;

namespace ServerTicTacToe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly GameSettings _settings;

        public RoomController(ILogger<RoomController> logger, GameSettings setting)
        {
            _logger = logger;
            _settings = setting;
        }

        [HttpPost]
        [Route("Move/{cell}/{num}")]
        public IActionResult MakeAMove(int cell, int num, [FromBody] Room room)
        {
            if (!Storage.Rooms.TryGetValue(room.Id, out var comparisonValue))
            {
                return BadRequest();
            }

            if (!room.TryPlaceNumber(cell, num))
            {
                return BadRequest();
            }

            _ = Storage.Rooms.TryUpdate(room.Id, room, comparisonValue);
            return Ok();
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult AddRoom(Room room)
        {
            if (room.Player1 is not null)
            {
                room.Player1.NumberInGame = 1;
                room.RoundTime = _settings.RoundTime;
                room.SeriesTime = _settings.InactiveTime;
                _logger.LogInformation("A new room was created");
                _ = Storage.Rooms.TryAdd(room.Id, room);

                return Ok(room);
            }

            _logger.LogWarning("Room creation error");
            return BadRequest();
        }

        [HttpPost]
        [Route("Public-Connect")]
        public IActionResult ConnectPublic(Player player)
        {
            _logger.LogInformation($"Player {player.Id} trying to connect to a public room...");
            var (key, room) = Storage.Rooms.SingleOrDefault(x => !x.Value.IsPrivate && x.Value.Player1 is not null
                                                                 && x.Value.Player2 is null && !x.Value.IsBusy);

            if (Equals(key, Guid.Empty))
            {
                _logger.LogWarning("Rooms do not exist or are occupied");
                return BadRequest();
            }

            if (room.Player1.Name == player.Name)
            {
                return NotFound();
            }

            AddPlayerToRoom(player, room);

            return Ok(room);
        }

        [HttpPost]
        [Route("Private-Connect/{id}")]
        public IActionResult ConnectPrivate(Guid id, [FromBody] Player player)
        {
            _logger.LogInformation($"Player {player.Id} trying to connect to a private room...");
            _ = Storage.Rooms.TryGetValue(id, out var room);

            if (room is null)
            {
                return BadRequest();
            }

            AddPlayerToRoom(player, room);

            return Ok(room);
        }

        [HttpGet]
        [Route("Get/{id}")]
        public IActionResult GetRoom(Guid id)
        {
            _ = Storage.Rooms.TryGetValue(id, out var room);

            return room is not null ? Ok(room) : BadRequest();
        }


        [HttpGet]
        [Route("Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            if (Storage.Rooms.TryRemove(id, out _))
            {
                _logger.LogInformation($"Room with id {id} was deleted");
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("Update")]
        public IActionResult Update(Room room)
        {
            if (!Storage.Rooms.TryGetValue(room.Id, out var comparisonValue))
            {
                return BadRequest();
            }

            return Storage.Rooms.TryUpdate(room.Id, room, comparisonValue) ? Ok() : BadRequest();
        }

        private void AddPlayerToRoom(Player player, Room room)
        {
            _logger.LogInformation("Room found");

            player.NumberInGame = 2;
            room.IsBusy = true;
            room.IsPlaying = true;
            room.Player2 = player;

            _ = Storage.Rooms.TryUpdate(room.Id, room, room);
        }
    }
}
