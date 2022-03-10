using System.Text.Json;
using TicTacToeLibrary.Models;

namespace ServerTicTacToe.Services
{
    public class JsonHandler
    {
        private readonly JsonSerializerOptions _options;
        public JsonHandler()
        {
            _options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public byte[] Serialize(Player player)
        {
            return JsonSerializer.SerializeToUtf8Bytes(player, _options);
        }

        public object Deserialize(Stream dataToDeserialize)
        {
            return JsonSerializer.Deserialize(dataToDeserialize, typeof(ICollection<Player>), _options)!;
        }

        public byte[] SerializeCollection(ICollection<Player> players)
        {
            return JsonSerializer.SerializeToUtf8Bytes(players, _options);
        }
    }
}
