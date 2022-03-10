using System.Text;
using System.Text.Json;

namespace ClientTicTacToe
{
    public static class Extensions
    {
        public static void ColoredMessage(this string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static StringContent ToStringContent<T>(this T obj)
        {
            var data = JsonSerializer.Serialize(obj);
            return new StringContent(data, Encoding.UTF8, "application/json");
        }

        public static T? Deserialize<T>(this string content)
        {
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
