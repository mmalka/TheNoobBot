using System;
using System.Collections.Generic;
using System.Diagnostics;
using RecastLayer;

namespace meshBuilder
{

    /// <summary>
    /// This is mostly to handle running the timers
    /// </summary>
    public abstract class BaseLog : BuildContext
    {
        protected readonly Dictionary<TimerLabel, Stopwatch> Timers = new Dictionary<TimerLabel, Stopwatch>();

        public abstract void ResetLog();
        public abstract void Log(LogCategory cat, string msg);

        public TimeSpan GetElapsedTime(TimerLabel label)
        {
            if (!Timers.ContainsKey(label))
                return TimeSpan.Zero;

            return Timers[label].Elapsed;
        }

        public Stopwatch GetTimer(TimerLabel label)
        {
            if (!Timers.ContainsKey(label))
                return null;

            return Timers[label];
        }

        public void ResetTimers()
        {
            Timers.Clear();
        }

        public void StartTimer(TimerLabel label)
        {
            if (!Timers.ContainsKey(label))
                Timers.Add(label, new Stopwatch());
            Timers[label].Start();
            OnTimerStarted(label);
        }

        public void StopTimer(TimerLabel label)
        {
            if (Timers.ContainsKey(label))
            {
                Timers[label].Stop();
                OnTimerStopped(label);
            }
        }

        protected abstract void OnTimerStopped(TimerLabel label);
        protected abstract void OnTimerStarted(TimerLabel label);
    }

}