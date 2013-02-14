using System.IO;
using System.Text;
using RecastLayer;

namespace meshBuilder
{
    
    public class MemoryLog : BaseLog
    {
        private readonly StringBuilder _data = new StringBuilder();

        public override void ResetLog()
        {
            
        }

        public override void Log(LogCategory cat, string msg)
        {
           // _data.AppendLine("[Recast:" + cat + "] " + msg);
        }

        protected override void OnTimerStopped(TimerLabel label)
        {
          //  _data.AppendLine("[Recast] Timer stopped: " + label + " (Accumulated: " + GetElapsedTime(label) + ")");
        }

        protected override void OnTimerStarted(TimerLabel label)
        {
           // _data.AppendLine("[Recast] Timer started: " + label);
        }

        public void WriteToFile(string path)
        {
          //  _data.AppendLine("Timer Overview");
         //   foreach (var timer in Timers)
          //      _data.AppendLine("\t" + timer.Key + ": " + timer.Value.Elapsed);
          //  File.WriteAllText(path, _data.ToString());
        }
    }

}