using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;

namespace nManager.FiniteStateMachine
{
    /// <summary>
    /// FSM Engine
    /// </summary>
    public class Engine
    {
        /// <summary>
        /// CurrentState
        /// </summary>
        public String CurrentState { get; private set; }

        private Thread _workerThread;
        private readonly bool _showStateInStatus;
        private System.Diagnostics.Stopwatch _stopWatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        public Engine(bool showStateInStatus = true)
        {
            try
            {
                _showStateInStatus = showStateInStatus;
                States = new List<State>();

                // Remember: We implemented the IComparer, and IComparable
                // interfaces on the State class!
                States.Sort();
            }
            catch (Exception e)
            {
                Logging.WriteError("Engine > Engine(): " + e);
            }
        }

        /// <summary>
        /// Gets the states.
        /// </summary>
        public List<State> States { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Engine"/> is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if running; otherwise, <c>false</c>.
        /// </value>
        public bool Running { get; private set; }

        /// <summary>
        /// Pulses this instance.
        /// </summary>
        public virtual void Pulse()
        {
            try
            {
                // This starts at the highest priority state,
                // and iterates its way to the lowest priority.
                foreach (State state in States)
                {
                    if (RunState(state))
                        break;
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Engine > Pulse(): " + e);
            }
        }

        private bool RunState(State state)
        {
            try
            {
                if (state.NeedToRun)
                {
                    CurrentState = state.DisplayName;
                    if (_showStateInStatus)
                        Logging.Status = "Bot > " + state.DisplayName;

                    try
                    {
                        foreach (State beforeState in state.BeforeStates)
                        {
                            RunState(beforeState);
                        }
                        state.Run();
                        foreach (State nextState in state.NextStates)
                        {
                            RunState(nextState);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.WriteError("RunState(State state): > State " + state.DisplayName + " - " + ex);
                    }
                    // Break out of the iteration,
                    // as we found a state that has run.
                    // We don't want to run any more states
                    // this time around.
                    return true;
                }
            }
            catch (Exception e)
            {
                try
                {
                    Logging.WriteError("Engine > RunState(State state) > State: " + state.DisplayName + " - " + e);
                }
                catch (Exception ex)
                {
                    Logging.WriteError("Engine > RunState(State state): " + ex);
                }
            }
            return false;
        }

        /// <summary>
        /// Starts the engine.
        /// </summary>
        /// <param name="framesPerSecond">The frames per second.</param>
        /// <param name="fsmCustomName">The names of the Thread to recognize it in Thread window.</param>
        public void StartEngine(byte framesPerSecond, string fsmCustomName = "FSM nManager")
        {
            try
            {
                // We want to round a bit here.
                int sleepTime = 1000/framesPerSecond;
                _stopWatch = new System.Diagnostics.Stopwatch();

                Running = true;

                _workerThread = new Thread(Run) {IsBackground = true, Name = fsmCustomName};
                _workerThread.Start(sleepTime);
            }
            catch (Exception e)
            {
                Logging.WriteError("Engine > StartEngine(byte framesPerSecond): " + e);
            }
        }

        private void Run(object sleepTime)
        {
            try
            {
                while (Running)
                {
                    _stopWatch.Start();
                    Pulse();
                    // Sleep for a 'frame'
                    if (_stopWatch.ElapsedMilliseconds < (int) sleepTime)
                    {
                        int wait = (int) sleepTime - (int) _stopWatch.ElapsedMilliseconds;
                        if (wait > 0)
                            Thread.Sleep(wait);
                    }
                    _stopWatch.Reset();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Engine > Run(object sleepTime): " + ex);
            }
            finally
            {
                CurrentState = "Stopped";
                Running = false;
            }
        }

        /// <summary>
        /// Stops the engine.
        /// </summary>
        public void StopEngine()
        {
            try
            {
                if (_workerThread == null || !Running)
                {
                    return;
                }
                if (_workerThread.IsAlive)
                {
                    _workerThread.Abort();
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Engine > StopEngine(): " + e);
            }
            finally
            {
                // Clear out the thread object.
                _workerThread = null;
                // Make sure we let everyone know, we're not running anymore!
                Running = false;
                CurrentState = "Stopped";
            }
        }

        /// <summary>
        /// Adds the state.
        /// </summary>
        /// <param name="tempState">State of the temp.</param>
        public void AddState(State tempState)
        {
            try
            {
                if (!States.Contains(tempState))
                    States.Add(tempState);
            }
            catch (Exception e)
            {
                Logging.WriteError("Engine > AddState(State tempState): " + e);
            }
        }

        /// <summary>
        /// Removes state by name.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <returns></returns>
        public Boolean RemoveStateByName(String displayName)
        {
            try
            {
                for (int i = 0; i < States.Count; i++)
                {
                    if (States[i].DisplayName == displayName)
                    {
                        States.RemoveAt(i);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Engine > RemoveStateByName(String displayName): " + e);
            }
            return false;
        }
    }
}