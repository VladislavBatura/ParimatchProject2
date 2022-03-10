using System.Timers;

namespace ServerTicTacToe.Services
{
    public sealed class RoomTimer : IDisposable
    {
        private readonly int _time;
        private System.Timers.Timer? _timer;

        private readonly ILogger _logger;

        public RoomTimer(int time, ILogger logger)
        {
            _time = time;
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
            _logger.LogInformation("Start timer for room round");
        }

        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            _timer!.Stop();
            //_logger.LogInformation("Block timer has stopped in {Time} for {User}", e.SignalTime, _login);
            Dispose();
        }

        public void Dispose()
        {
            _timer!.Dispose();
        }
    }
}
