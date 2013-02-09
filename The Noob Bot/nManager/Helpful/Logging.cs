using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace nManager.Helpful
{
    public class Logging
    {
        private static List<Log> _log = new List<Log>();
        private static string _logFileName = "";

        private static string _status = "";

        public static string Status
        {
            get { return _status; }
            set
            {
                try
                {
                    _status = value;
                    var e = new StatusChangeEventArgs {Status = value};
                    if (OnChangedStatus != null)
                        OnChangedStatus(Status, e);
                }
                catch (Exception ex)
                {
                    WriteError("Loggin > Status > Set: " + ex);
                }
            }
        }

        public static Log ReadLast(LogType logType)
        {
            try
            {
                for (var i = _log.Count - 1; i > 0; i--)
                {
                    if ((_log[i].LogType & logType) == _log[i].LogType)
                        return _log[i];
                }
            }
            catch (Exception exception)
            {
                WriteError("ReadLast(LogType logType): " + exception);
            }
            return new Log();
        }

        public static string ReadLastString(LogType logType)
        {
            try
            {
                return ReadLast(logType).ToString();
            }
            catch (Exception exception)
            {
                WriteError("ReadLastString(LogType logType): " + exception);
            }
            return "";
        }

        public static List<Log> List
        {
            get { return _log; }
        }

        public static List<Log> ReadList()
        {
            return _log;
        }

        public static List<Log> ReadList(LogType logType)
        {
            try
            {
                return _log.Where(l => (l.LogType & logType) == l.LogType).ToList();
            }
            catch (Exception exception)
            {
                WriteError("ReadList(LogType logType): " + exception);
            }
            return new List<Log>();
        }

        public static List<string> ReadListString(LogType logType)
        {
            try
            {
                return
                    (from l in ReadList(logType) where (l.LogType & logType) == l.LogType select l.ToString()).ToList();
            }
            catch (Exception exception)
            {
                WriteError("ReadListString(LogType logType): " + exception);
            }
            return new List<string>();
        }

        public static int CountNumberInQueue
        {
            get { return LogQueue.Count; }
        }

        private static readonly List<Log> LogQueue = new List<Log>();
        private static Thread _worker;

        public static void Write(Log log)
        {
            try
            {
                LogQueue.Add(log);

                lock (typeof (Logging))
                {
                    if (_worker == null)
                    {
                        _worker = new Thread(AddLog) {Name = "Logging"};
                        _worker.Start();
                    }
                }
            }
            catch (Exception exception)
            {
                WriteError("Write(Log log): " + exception);
            }
        }

        public static void Write(string text, LogType logType, Color color)
        {
            Write(new Log(text, logType, color));
        }

        public static void Write(string text)
        {
            Write(text, LogType.Normal, Color.Black);
        }

        public static void WriteDebug(string text)
        {
            Write(text, LogType.Debug, Color.MediumVioletRed);
        }

        public static void WriteNavigator(string text)
        {
            Write(text, LogType.Navigator, Color.Blue);
        }

        public static void WriteFight(string text)
        {
            Write(text, LogType.Fight, Color.Green);
        }

        public static void WriteFileOnly(string text)
        {
            Write(text, LogType.FileOnly, Color.Gray);
        }

        public static void WriteError(string text, bool skipThreadAbortExceptionError = true)
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (text.Contains("System.Threading.ThreadAbortException") && skipThreadAbortExceptionError)
                return;

            Write(text, LogType.Error, Color.Red);
        }

        private static void AddLog()
        {
            while (true)
            {
                try
                {
                    if (LogQueue.Count > 0)
                    {
                        if (_logFileName == "")
                            NewFile();

                        Console.WriteLine(LogQueue[0].ToString());
                        _log.Add(LogQueue[0]);

                        if (_log.Count > 300)
                            _log.RemoveAt(0);

                        if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                            Directory.CreateDirectory(Application.StartupPath + "\\Logs");

                        var sw = new StreamWriter(Application.StartupPath + "\\Logs\\" + _logFileName, true,
                                                  Encoding.UTF8);
                        sw.Write("<font color=\"" + ColorTranslator.ToHtml(LogQueue[0].Color) + "\">" +
                                 LogQueue[0].ToString().Replace(Environment.NewLine, "<br> ") + "</font><br>");
                        sw.Close();

                        try
                        {
                            var e = new LoggingChangeEventArgs
                                {
                                    Log = new Log
                                        {
                                            Color = LogQueue[0].Color,
                                            DateTime = LogQueue[0].DateTime,
                                            LogType = LogQueue[0].LogType,
                                            Text = LogQueue[0].Text
                                        }
                                };
                            if (OnChanged != null)
                                OnChanged(null, e);
                        }
                        catch
                        {
                        }
                        LogQueue.RemoveAt(0);
                    }
                    else
                        Thread.Sleep(100);
                }
                catch (Exception exception)
                {
                    WriteError("AddLog(): " + exception);
                }
                Thread.Sleep(30);
            }
            // ReSharper disable FunctionNeverReturns
        }

        // ReSharper restore FunctionNeverReturns

        public static void NewFile()
        {
            try
            {
                _log = new List<Log>();
                _logFileName = DateTime.Now.ToString("d MMM yyyy HH") + "H" + DateTime.Now.ToString("mm") + ".log.html";
                if (File.Exists(Application.StartupPath + "\\Logs\\" + _logFileName))
                    _logFileName = DateTime.Now.ToString("d MMM yyyy HH") + "H" + DateTime.Now.ToString("mm") + " - " +
                                   Others.GetRandomString(Others.Random(4, 7)) + ".log.html";
                LogQueue.Insert(0, new Log("Log file created: " + _logFileName, LogType.Debug, Color.MediumVioletRed));
            }
            catch (Exception exception)
            {
                WriteError("NewFile(): " + exception);
            }
        }

        [Flags]
        public enum LogType
        {
            None = 0x0,
            Normal = 0x1,
            Debug = 0x2,
            Error = 0x4,
            Navigator = 0x8,
            Fight = 0x10,
            FileOnly = 0x20,
        }

        public class Log
        {
            public Log()
            {
                Text = "";
                LogType = LogType.Normal;
                Color = Color.Black;
                DateTime = DateTime.Now;
            }

            public Log(string text, LogType logType, Color color)
            {
                try
                {
                    Text = text;
                    LogType = logType;
                    Color = color;
                    DateTime = DateTime.Now;
                }
                catch (Exception exception)
                {
                    WriteError("Log(string text, LogType logType, Color color): " + exception);
                }
            }

            public override string ToString()
            {
                try
                {
                    return "[" + LogType + "] " + DateTime.ToString("HH:mm") + " - " + Text;
                }
                catch (Exception exception)
                {
                    WriteError("ToString(): " + exception);
                }
                return "";
            }

            public DateTime DateTime { get; set; }
            public string Text { get; set; }
            public LogType LogType { get; set; }

            public Color Color { get; set; }
        }

        public delegate void LoggingChangeEventHandler(object sender, LoggingChangeEventArgs e);

        public static event LoggingChangeEventHandler OnChanged;

        public class LoggingChangeEventArgs : EventArgs
        {
            public Log Log { get; set; }
        }

        public delegate void StatusChangeEventHandler(object sender, StatusChangeEventArgs e);

        public static event StatusChangeEventHandler OnChangedStatus;

        public class StatusChangeEventArgs : EventArgs
        {
            public string Status { get; set; }
        }
    }
}