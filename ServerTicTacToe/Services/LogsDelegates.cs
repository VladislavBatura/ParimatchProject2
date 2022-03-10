namespace ServerTicTacToe.Services
{
    public static class LogsDelegates
    {
        private static readonly Action<ILogger, Exception?> _hubIndex;
        private static readonly Action<ILogger, string, Exception?> _hubRegisterStart;
        private static readonly Action<ILogger, string, Exception?> _hubRegisterModelError;
        private static readonly Action<ILogger, string, Exception?> _hubRegisterCreateNewAccount;
        private static readonly Action<ILogger, string, Exception?> _hubRegisterAccountExists;
        private static readonly Action<ILogger, string, Exception?> _hubRegisterError;
        private static readonly Action<ILogger, string, Exception?> _hubRegisterSuccess;
        private static readonly Action<ILogger, string, Exception?> _hubLoginModelError;
        private static readonly Action<ILogger, string, Exception?> _hubLoginError;
        private static readonly Action<ILogger, string, Exception?> _hubLoginBlocked;

        static LogsDelegates()
        {
            _hubIndex = LoggerMessage.Define(
                LogLevel.Information,
                1,
                "Player entered 'Index' action");
            _hubRegisterStart = LoggerMessage.Define<string>(
                LogLevel.Information,
                2,
                "Player with login {PlayerName} trying to register");
            _hubRegisterModelError = LoggerMessage.Define<string>(
                LogLevel.Information,
                2,
                "Player with login {PlayerName} failed to register due to modal exception");
            _hubRegisterCreateNewAccount = LoggerMessage.Define<string>(
                LogLevel.Information,
                2,
                "Player with login {PlayerPost} created new account");
            _hubRegisterAccountExists = LoggerMessage.Define<string>(
                LogLevel.Information,
                2,
                "Player with login {PlayerPost} already exists");
            _hubRegisterError = LoggerMessage.Define<string>(
                LogLevel.Information,
                2,
                "Player with login {PlayerPost} can't be added to server due unexpected error");
            _hubRegisterSuccess = LoggerMessage.Define<string>(
                LogLevel.Information,
                2,
                "Player with login {PlayerPost} successfully added to server");
            _hubLoginModelError = LoggerMessage.Define<string>(
                LogLevel.Information,
                3,
                "Player with login {PlayerPost} model error");
            _hubLoginError = LoggerMessage.Define<string>(
                LogLevel.Information,
                3,
                "Player with login {PlayerPost} error on login");
            _hubLoginBlocked = LoggerMessage.Define<string>(
                LogLevel.Information,
                3,
                "Player with login {PlayerPost} was blocked");
        }

        #region HubControllerMethods
        public static void HubIndex(this ILogger logger)
        {
            _hubIndex(logger, null);
        }

        public static void HubRegisterStart(this ILogger logger, string player)
        {
            _hubRegisterStart(logger, player, null);
        }

        public static void HubRegisterModelError(this ILogger logger, string player)
        {
            _hubRegisterModelError(logger, player, null);
        }

        public static void HubRegisterCreateNewAccount(this ILogger logger, string player)
        {
            _hubRegisterCreateNewAccount(logger, player, null);
        }

        public static void HubRegisterAccountExists(this ILogger logger, string player)
        {
            _hubRegisterAccountExists(logger, player, null);
        }

        public static void HubRegisterError(this ILogger logger, string player)
        {
            _hubRegisterError(logger, player, null);
        }

        public static void HubRegisterSuccess(this ILogger logger, string player)
        {
            _hubRegisterSuccess(logger, player, null);
        }

        public static void HubLoginModelError(this ILogger logger, string player)
        {
            _hubLoginModelError(logger, player, null);
        }

        public static void HubLoginError(this ILogger logger, string player)
        {
            _hubLoginError(logger, player, null);
        }

        public static void HubLoginBlocked(this ILogger logger, string player)
        {
            _hubLoginBlocked(logger, player, null);
        }
        #endregion
    }
}
