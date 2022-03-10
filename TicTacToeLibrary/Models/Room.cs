namespace TicTacToeLibrary.Models
{
    public sealed class Room : RoomBase
    {
        public bool IsBusy { get; set; }
        public bool IsPrivate { get; set; }
    }
}
