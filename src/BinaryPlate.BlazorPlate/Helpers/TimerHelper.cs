using System.Timers;
using Timer = System.Timers.Timer;

namespace BinaryPlate.BlazorPlate.Helpers
{
    public class TimerHelper
    {
        private Timer _timer;

        public void SetTimer(double interval)
        {
            _timer = new Timer(interval);
            _timer.Elapsed += NotifyTimerElapsed;
            _timer.AutoReset = false;
            _timer.Enabled = true;
        }
        public void Dispose()
        {
            _timer.Dispose();
            _timer.Elapsed -= NotifyTimerElapsed;
        }
        private void NotifyTimerElapsed(object sender, ElapsedEventArgs e)
        {
            OnElapsed?.Invoke();

        }
        public void StartTimer()
        {
            _timer.Start();
        }
        public void StopTimer()
        {
            _timer.Stop();
        }
        public event Action OnElapsed;
    }
}
