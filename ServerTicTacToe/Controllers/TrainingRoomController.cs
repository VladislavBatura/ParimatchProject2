using Microsoft.AspNetCore.Mvc;
using ServerTicTacToe.Models;
using TicTacToeLibrary.Models;

namespace ServerTicTacToe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrainingRoomController : ControllerBase
    {

        private readonly ILogger<RoomController> _logger;
        private readonly GameSettings _settings;

        public TrainingRoomController(ILogger<RoomController> logger, GameSettings setting)
        {
            _logger = logger;
            _settings = setting;
        }

        [HttpPost]
        [Route("Move/{cell}/{num}")]
        public IActionResult MakeAMove(int cell, int num, [FromBody] TrainingRoom room)
        {
            if (!Storage.TrainingRooms.TryGetValue(room.Id, out var comparisonValue))
            {
                return BadRequest();
            }

            if (!room.TryPlaceNumber(cell, num))
            {
                return BadRequest();
            }

            _ = Storage.TrainingRooms.TryUpdate(room.Id, room, comparisonValue);
            return Ok();
        }

        [HttpPost]
        [Route("Create-Training")]
        public IActionResult CreateBotGame(TrainingRoom room)
        {
            if (room.Player1 is null)
            {
                return BadRequest();
            }

            room.Player1.NumberInGame = 1;
            room.Player2 = new Player {Id = Guid.NewGuid(), Name = "Bot", NumberInGame = 2};
            room.RoundTime = _settings.RoundTime;
            room.SeriesTime = _settings.InactiveTime;
            room.IsPlaying = true;
            _logger.LogInformation("A new training room was created");

            _ = Storage.TrainingRooms.TryAdd(room.Id, room);

            return Ok(room);
        }

        [HttpGet]
        [Route("Get/{id}")]
        public IActionResult GetRoom(Guid id)
        {
            _ = Storage.TrainingRooms.TryGetValue(id, out var room);

            return room is not null ? Ok(room) : BadRequest();
        }


        [HttpGet]
        [Route("Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            if (Storage.TrainingRooms.TryRemove(id, out _))
            {
                _logger.LogInformation($"Room with id {id} was deleted");
                return Ok();
            }

            return BadRequest();
        }
    }
}
