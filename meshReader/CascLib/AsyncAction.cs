using System;
using System.Threading;
using System.Threading.Tasks;

namespace TheNoobViewer
{
    public class AsyncAction
    {
        private CancellationTokenSource cts;
        private Action action;

        public bool IsCancellationRequested
        {
            get { return cts.IsCancellationRequested; }
        }

        public AsyncAction(Action action, Action<int> progressAction = null)
        {
            this.action = action;

            cts = new CancellationTokenSource();
        }

        public void ThrowOnCancel()
        {
            cts.Token.ThrowIfCancellationRequested();
        }

        public void Cancel()
        {
            cts.Cancel();
        }
    }
}
