using System;
using System.Collections.Generic;
using nManager.Helpful;

namespace nManager.FiniteStateMachine
{
    /// <summary>
    /// State FSM
    /// </summary>
    public abstract class State : IComparable<State>, IComparer<State>
    {
        /// <summary>
        /// Gets the priority.
        /// </summary>
        public abstract int Priority { get; set; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public abstract String DisplayName { get; }

        /// <summary>
        /// Gets a value indicating whether [need to run].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [need to run]; otherwise, <c>false</c>.
        /// </value>
        public abstract bool NeedToRun { get; }

        /// <summary>
        /// Launch this state after (if NeedToRun == true), empty if null.
        /// </summary>
        public abstract List<State> NextStates { get; }

        /// <summary>
        /// Launch this state before (if NeedToRun == true), empty if null.
        /// </summary>
        public abstract List<State> BeforeStates { get; }

        #region IComparable<State> Members

        public int CompareTo(State other)
        {
            // We want the highest first.
            // int, by default, chooses the lowest to be sorted
            // at the bottom of the list. We want the opposite.
            try
            {
                return -Priority.CompareTo(other.Priority);
            }
            catch (Exception e)
            {
                Logging.WriteError("State > CompareTo(State other): " + e);
            }
            return 0;
        }

        #endregion

        #region IComparer<State> Members

        public int Compare(State x, State y)
        {
            try
            {
                return -x.Priority.CompareTo(y.Priority);
            }
            catch (Exception e)
            {
                Logging.WriteError("State > Compare(State x, State y): " + e);
            }
            return 0;
        }

        #endregion

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public abstract void Run();
    }
}