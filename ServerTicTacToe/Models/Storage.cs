using System.Collections.Concurrent;
using ServerTicTacToe.Services;
using TicTacToeLibrary.Models;

namespace ServerTicTacToe.Models
{
    public static class Storage
    {
        public static readonly ConcurrentDictionary<string, Player> Players = new();
        public static readonly ConcurrentDictionary<string, Player> PlayersOnline = new();
        public static readonly ConcurrentDictionary<string, int> FailedLogins = new();
        public static readonly ConcurrentDictionary<string, FailedLogin> TimerOnFailedLogin = new();
        public static readonly ConcurrentDictionary<Guid, Room> Rooms = new();
        public static readonly ConcurrentDictionary<Guid, TrainingRoom> TrainingRooms = new();
    }
}
