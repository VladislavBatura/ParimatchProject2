using System.Collections.Concurrent;
using System.Timers;
using ServerTicTacToe.Models;

namespace ServerTicTacToe.Services
{
    public sealed class FailedLogin : IDisposable
    {
        private readonly string _login;
        private System.Timers.Timer? _timer;

        private readonly ILogger _logger;

        public FailedLogin(string loginToBlock, ILogger logger)
        {
            _login = loginToBlock;
            _logger = logger;
        }

        public void Start()
        {
            SetTimer();
        }

        private void SetTimer()
        {
            _timer = new System.Timers.Timer(300_000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = false;
            _timer.Start();
            _logger.LogInformation("Start block timer for {User}", _login);
        }

        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            _ = Storage.FailedLogins.TryRemove(_login, out _);
            _ = Storage.TimerOnFailedLogin.TryRemove(_login, out _);
            _timer!.Stop();
            _logger.LogInformation("Block timer has stopped in {Time} for {User}", e.SignalTime, _login);
            Dispose();
        }

        public void Dispose()
        {
            _timer!.Dispose();
        }
    }
}
