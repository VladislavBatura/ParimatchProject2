namespace TicTacToeLibrary
{
    public readonly struct Server
    {
        public const string Url = "http://localhost:5000";
        public const string HubIndex = "http://localhost:5000/Hub/Index";
        public const string HubRegister = "http://localhost:5000/Hub/Register";
        public const string HubLogin = "http://localhost:5000/Hub/Login";
        /// <summary>
        /// Should be called manually, or when client exits application
        /// <param name="playerName">Should send only users login</param>
        /// </summary>
        public const string HubLogout = "http://localhost:5000/Hub/Logout";
        public const string HubTop = "http://localhost:5000/Hub/Top";
        public const string HubUpdateSessions = Url + "/Hub/Sessions/";
        public const string HubGetPlayer = Url + "/Hub/GetPlayer/";

        public const string RoomCreate = Url + "/Room/Create";
        public const string RoomUpdate = Url + "/Room/Update";
        public const string RoomMove = Url + "/Room/Move/";
        public const string PublicConnect = Url + "/Room/Public-Connect";

        /// <summary>
        /// Use this url + room id
        /// </summary>
        public const string PrivateConnect = Url + "/Room/Private-Connect/";

        /// <summary>
        /// Use this url + room id
        /// </summary>
        public const string RoomGet = Url + "/Room/Get/";

        /// <summary>
        /// Use this url + room id
        /// </summary>
        public const string RoomClose = Url + "/Room/Delete/";

        public const string TrainingRoomCreate = Url + "/TrainingRoom/Create-Training";
        public const string TrainingRoomMove = Url + "/TrainingRoom/Move/";

        public const string TrainingRoomGet = Url + "/TrainingRoom/Get/";
        public const string TrainingRoomClose = Url + "/TrainingRoom/Delete/";

        public const string RegisteredPlayers = "RegisteredPlayers.json";
    }
}
