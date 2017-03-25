using System;

namespace meshBuilder
{
    public class ProgressEvent : EventArgs
    {
        public int CompletedWork { get; private set; }
        public int TotalWork { get; private set; }

        public float Completion
        {
            get { return TotalWork > 0 ? (CompletedWork / (float) TotalWork) : 1f; }
        }

        public ProgressEvent(int completed, int total)
        {
            CompletedWork = completed;
            TotalWork = total;
        }
    }
}