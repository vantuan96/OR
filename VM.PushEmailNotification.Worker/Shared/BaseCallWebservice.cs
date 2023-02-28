using System;
using System.Configuration;
using System.Threading;
namespace OREmailNoti.WindowService.Shared
{
    public class BaseCallWebservice : IDisposable
    {
        protected System.Threading.Thread[] _Thread;
        public event onComplectedEventHandler onComplected;
        public delegate void onComplectedEventHandler(object sender, EventHandleWebservice e);

        public static int TotalThread = 20;
        public static int CurThread = 0;
        protected BaseCallWebservice()
        {
            TotalThread = Int32.Parse(ConfigurationManager.AppSettings["SlThread"] ?? "5");
            _Thread = new Thread[TotalThread];

        }
        #region "Abort"
        public void Abort(int i)
        {
            if ((_Thread != null && _Thread[i] != null))
            {
                if (_Thread[i].ThreadState == System.Threading.ThreadState.Running)
                {
                    _Thread[i].Abort();
                }
            }
            _Thread[i] = null;
        }
        #endregion

        #region "IDisposable Support"
        // To detect redundant calls
        private bool disposedValue;



        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this._Thread[CurThread].Abort();
                    // TODO: dispose managed state (managed objects).
                }
                GC.Collect();
                // TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                // TODO: set large fields to null.
            }
            this.disposedValue = true;
        }
        // TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        //Protected Overrides Sub Finalize()
        //    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        //    Dispose(False)
        //    MyBase.Finalize()
        //End Sub

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region "Properties"
        public System.Threading.Thread CurrentThread
        {
            get { return this._Thread[CurThread]; }
        }

        #endregion

        #region "onCompleted"
        protected virtual void RaiseEventComplected(object sender, EventHandleWebservice e)
        {
            if (onComplected != null)
            {
                onComplected(sender, e);
            }
        }
        #endregion

    }

    public class EventHandleWebservice
    {
        private object _ReponseData;
        public EventHandleWebservice(object reponseData)
            : base()
        {
            this._ReponseData = reponseData;
        }
        public T GetReponse<T>()
        {
            return (T)_ReponseData;
        }
    }
}
