using System;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Products;

namespace Milling
{
    public class Main : IProduct
    {
        #region IProduct Members

        public void Initialize()
        {
            try
            {
                Logging.Status = "Initialize Milling Complete";
                Logging.Write("Initialize Milling Complete");
            }
            catch (Exception e)
            {
                Logging.WriteError("Milling > Main > Initialize(): " + e);
            }
        }

        public void Dispose()
        {
            try
            {
                Stop();
                Logging.Status = "Dispose Milling Complete";
                Logging.Write("Dispose Milling Complete");
            }
            catch (Exception e)
            {
                Logging.WriteError("Milling > Main > Dispose(): " + e);
            }
        }

        public void Start()
        {
            try
            {
                global::Milling.Milling.Pulse();
                _isStarted = true;
                Logging.Status = "Milling started";
            }
            catch (Exception e)
            {
                Logging.WriteError("Milling > Main > Start(): " + e);
            }
        }

        public void Stop()
        {
            try
            {
                _isStarted = false;
                Logging.Status = "Milling stoped";
            }
            catch (Exception e)
            {
                Logging.WriteError("Milling > Main > Stop(): " + e);
            }
        }

        public void Settings()
        {
            try
            {
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.No_setting_for_this_product));
                Logging.Status = "Settings Milling Complete";
                Logging.Write("Settings Milling Complete");
            }
            catch (Exception e)
            {
                Logging.WriteError("Milling > Main > Settings(): " + e);
            }
        }

        public bool IsStarted
        {
            get { return _isStarted; }
        }

        private bool _isStarted;

        #endregion
    }
}