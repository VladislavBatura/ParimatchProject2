using Microsoft.Extensions.Logging;

namespace ClientTicTacToe.Handlers
{
    public static class Handler
    {
        public static HttpHandler Http { get; private set; }
        public static HubHandler Hub { get; private set; }
        public static RoomHandler Room { get; private set; }
        public static TrainingRoomHandler TrainingRoom { get; private set;}

        public static void SetHandlers(ILogger<HubHandler> hubLogger, ILogger<RoomHandler> roomLogger, ILogger<TrainingRoomHandler> trainingLogger)
        {
            Http = new HttpHandler();
            Hub = new HubHandler(hubLogger);
            Room = new RoomHandler(roomLogger);
            TrainingRoom = new TrainingRoomHandler(trainingLogger);
        }
    }
}
