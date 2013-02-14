using System;
using RecastLayer;

namespace meshBuilder
{

    public class ConsoleLog : BaseLog
    {
        public override void ResetLog()
        {
        }

        public override void Log(LogCategory category, string msg)
        {
            Console.WriteLine("[Recast:" + category + "] " + msg);
        }

        protected override void OnTimerStopped(TimerLabel label)
        {
            Console.WriteLine("[Recast] Timer stopped: " + label + " (Elapsed: " + GetElapsedTime(label) + ")");
        }

        protected override void OnTimerStarted(TimerLabel label)
        {
            Console.WriteLine("[Recast] Timer started: " + label);
        }
    }

}