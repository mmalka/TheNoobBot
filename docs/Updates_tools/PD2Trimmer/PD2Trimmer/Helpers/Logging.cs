using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;

namespace PD2Trimmer.Helpers
{
    /// <summary>
    /// This class handles all logging done for OpenBot. From simple output, to writing to files, to logging exceptions
    /// and user messages. It is event based, and thread safe.
    /// </summary>
    /// <example>Logging.WriteDebug("Oh noes! We failed again!");</example>
    /// 1/14/2009 7:31 PM
    public static class Logging
    {
        #region Delegates

        /// <summary>
        /// 
        /// </summary>
        public delegate void DebugDelegate(string message, Color col);

        /// <summary>
        /// 
        /// </summary>
        public delegate void WriteDelegate(string message, Color col);

        #endregion

        private static readonly Queue<Color> DebugColorQueue = new Queue<Color>();
        private static readonly Queue<string> DebugQueue = new Queue<string>();

        private static readonly Queue<string> LogQueue = new Queue<string>();
        private static readonly Thread QueueThread;
        private static readonly Queue<Color> WriteColorQueue = new Queue<Color>();
        private static readonly Queue<string> WriteQueue = new Queue<string>();

        static Logging()
        {
            LogOnWrite = true;
            QueueThread = new Thread(_WriteQueue) {IsBackground = true};
            QueueThread.Start();
        }

        /// <summary>
        /// Gets or sets a value indicating whether every Write action should also be logged.
        /// </summary>
        /// <value><c>true</c> if [log on write]; otherwise, <c>false</c>.</value>
        /// 1/14/2009 7:40 PM
        public static bool LogOnWrite { get; set; }

        private static string TimeStamp { get { return string.Format("[{0}] ", DateTime.Now.ToLongTimeString()); } }
        private static string logDate { get { return DateTime.Now.ToShortDateString().Replace("/", "-"); } }
        public static string ApplicationPath { get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); } }

        /// <summary>
        /// Occurs when [on write].
        /// </summary>
        public static event WriteDelegate OnWrite;

        /// <summary>
        /// Occurs when [on debug].
        /// </summary>
        public static event DebugDelegate OnDebug;

        private static void InvokeOnWrite(string message, Color col)
        {
            if (OnWrite != null)
            {
                OnWrite(message, col);
            }
        }

        private static void InvokeOnDebug(string message, Color col)
        {
            if (OnDebug != null)
            {
                OnDebug(message, col);
            }
        }

        private static void _WriteQueue()
        {
            if (!Directory.Exists(string.Format("{0}\\Logs", ApplicationPath)))
            {
                Directory.CreateDirectory(string.Format("{0}\\Logs", ApplicationPath));
            }
            while (true)
            {
                try
                {
                    using (
                        TextWriter tw =
                            new StreamWriter(string.Format("{0}\\Logs\\{1} Log.txt", ApplicationPath, logDate), true))
                    {
                        while (LogQueue.Count != 0)
                        {
                            tw.WriteLine(LogQueue.Dequeue());
                        }
                        string[] tmp = WriteQueue.ToArray();
                        Color[] ctmp = WriteColorQueue.ToArray();
                        for (int i = 0; i < tmp.Length; i++)
                        {
                            InvokeOnWrite(tmp[i], ctmp[i]);
                            WriteQueue.Dequeue();
                            WriteColorQueue.Dequeue();
                        }
                        tmp = DebugQueue.ToArray();
                        ctmp = DebugColorQueue.ToArray();
                        for (int i = 0; i < tmp.Length; i++)
                        {
                            InvokeOnDebug(tmp[i], ctmp[i]);
                            DebugQueue.Dequeue();
                            DebugColorQueue.Dequeue();
                        }
                    }
                    Thread.Sleep(500);
                }
                catch
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Writes the specified message to the message queue.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        /// 1/14/2009 7:33 PM
        public static void Write(string format, params object[] args)
        {
            Write(Color.Black, format, args);
        }

        /// <summary>
        /// Writes the specified message to the message queue.
        /// </summary>
        /// <param name="color">The color to write the message in.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        /// 1/14/2009 7:33 PM
        public static void Write(Color color, string format, params object[] args)
        {
            string s = args.Length > 0 ? string.Format(format, args) : format;
            
            WriteQueue.Enqueue(s);
            WriteColorQueue.Enqueue(color);
            if (LogOnWrite)
            {
                LogQueue.Enqueue(s);
            }
        }

        /// <summary>
        /// Writes the specified debug message to the message queue. [Default Color: Red]
        /// Debug messages are not shown to the end user by default. (They can be turned on via settings)
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        /// 1/14/2009 7:40 PM
        public static void WriteDebug(string format, params object[] args)
        {
            WriteDebug(Color.Red, format, args);
        }

        /// <summary>
        /// Writes the debug message in the specific color.
        /// Debug messages are not shown to the end user by default. (They can be turned on via settings)
        /// </summary>
        /// <param name="color">The color to write in.</param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WriteDebug(Color color, string format, params object[] args)
        {
            string s = /*TimeStamp + */ string.Format(format, args);
            DebugQueue.Enqueue(s);
            DebugColorQueue.Enqueue(color);
            if (LogOnWrite)
            {
                LogQueue.Enqueue(s);
            }
        }

        /// <summary>
        /// Writes the specified exception to the message queue. [Default Color: Red]
        /// Debug messages are not shown to the end user by default. (They can be turned on via settings)
        /// </summary>
        /// <param name="ex">The exception that will be logged.</param>
        public static void WriteException(Exception ex)
        {
            WriteException(Color.Blue, ex);
        }

        /// <summary>
        /// Writes the specified exception to the message queue.
        /// Debug messages are not shown to the end user by default. (They can be turned on via settings)
        /// </summary>
        /// <param name="color">The color to write the message in.</param>
        /// <param name="ex">The exception that will be logged.</param>
        public static void WriteException(Color color, Exception ex)
        {
            string s = string.Format("{0}{1} ({5}) - {4} - From: {2}\n{3}", TimeStamp, ex.Message, ex.Source, ex.StackTrace,
                                     ex.InnerException, ex.GetType());

            DebugQueue.Enqueue(s);
            DebugColorQueue.Enqueue(color);
            if (LogOnWrite)
            {
                LogQueue.Enqueue(s);
            }
        }

        /// <summary>
        /// Logs the specified message to the logging queue. This will not fire any
        /// message events. It will go straight to the log file without question.
        /// </summary>
        /// <param name="format">The format param of string.Format</param>
        /// <param name="args">The args param of string.Format</param>
        public static void LogMessage(string format, params object[] args)
        {
            string s = TimeStamp + string.Format(format, args);
            LogQueue.Enqueue(s);
        }

        /// <summary>
        /// Logs the specified exception into the log queue.
        /// </summary>
        /// <param name="ex">The exception to be logged.</param>
        public static void LogException(Exception ex)
        {
            string s = string.Format("{0}{1} - {4} - From: {2}\n{3}", TimeStamp, ex.Message, ex.Source, ex.StackTrace,
                                     ex.InnerException);
            LogQueue.Enqueue(s);
        }
    }
}