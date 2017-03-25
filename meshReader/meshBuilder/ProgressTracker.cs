using System;

namespace meshBuilder
{
    public abstract class ProgressTracker
    {
        public event EventHandler<ProgressEvent> OnProgress;

        protected int TotalWork;
        protected int CompletedWork;

        protected void CompleteWorkUnit()
        {
            CompleteWork(1);
        }

        protected void CompleteWork(int units)
        {
            CompletedWork += units;
            if (CompletedWork > TotalWork)
                CompletedWork = TotalWork;
            if (OnProgress != null)
                OnProgress(this, new ProgressEvent(CompletedWork, TotalWork));
        }

        protected void InitializeProgress(int total)
        {
            TotalWork = total;
            CompletedWork = 0;
        }
    }
}