using System;

namespace nManager.Helpful
{
    public class Timer
    {
        #region -- Private Variables --

        /// <summary>
        /// Holds the value of the Frequency property.
        /// </summary>
        private readonly double _countDowntime;

        private bool _varforceReady;

        #endregion

        /// <summary>
        /// Initializes a new instance of the StopWatch class.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// The system does not have a high-resolution 
        /// performance counter.
        /// </exception>
        /// 
        public Timer()
        {
            _countDowntime = 0;
            _varforceReady = false;
            Reset();
        }

        public Timer(double countDowntime)
        {
            try
            {
                _countDowntime = countDowntime;
                _varforceReady = false;
                Reset();
            }
            catch (Exception e)
            {
                Logging.WriteError("Timer(double countDowntime): " + e);
            }
        }

        /// <summary>
        /// Returns true if the timer is ready
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// The system does not have a high-resolution 
        /// performance counter.
        /// </exception>
        /// 
        public bool IsReady
        {
            get
            {
                try
                {
                    if (_varforceReady)
                        return true;

                    if (Peek() > _countDowntime)
                        return true;
                }
                catch (Exception e)
                {
                    Logging.WriteError("Timer > IsReady: " + e);
                }
                return false;
            }
        }

        /// <summary>
        /// Resets the stopwatch. This method should be called 
        /// when you start measuring.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// The system does not have a high-resolution 
        /// performance counter.
        /// </exception>
        public void Reset()
        {
            try
            {
                StartTime = GetValue();
                _varforceReady = false;
            }
            catch (Exception e)
            {
                Logging.WriteError("Timer > Reset(): " + e);
            }
        }

        /// <summary>
        /// Returns the time that has passed since the Reset() 
        /// method was called.
        /// </summary>
        /// <remarks>
        /// The time is returned in tenths-of-a-millisecond. 
        /// If the Peek method returns '10000', it means the interval 
        /// took exactely one second.
        /// </remarks>
        /// <returns>
        /// A long that contains the time that has passed 
        /// since the Reset() method was called.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// The system does not have a high-resolution performance counter.
        /// </exception>
        public long Peek()
        {
            try
            {
                return GetValue() - StartTime;
            }
            catch (Exception e)
            {
                Logging.WriteError("Timer > Peek(): " + e);
            }
            return 0;
        }

        /// <summary>
        /// Retrieves the current value of the high-resolution 
        /// performance counter.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// The system does not have a high-resolution 
        /// performance counter.
        /// </exception>
        /// <returns>
        /// A long that contains the current performance-counter 
        /// value, in counts.
        /// </returns>
        private long GetValue()
        {
            try
            {
                return Environment.TickCount;
            }
            catch (Exception e)
            {
                Logging.WriteError("Timer > GetValue(): " + e);
            }
            return 0;
        }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// A long that holds the start time.
        /// </value>
        private long StartTime { get; set; }

        public void ForceReady()
        {
            _varforceReady = true;
        }
    }
}