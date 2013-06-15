using System;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;
using nManager.Products;

namespace Flying_To_Ground_Profiles_Converter
{
    public class Main : IProduct
    {
        #region IProduct Members

        private WelcomeForm _welcomeForm;

        public void Initialize()
        {
            try
            {
                Logging.Status = "Initialize Flying To Ground Profiles Converter Complete";
                Logging.Write("Initialize Flying To Ground Profiles Converter Complete");
            }
            catch (Exception e)
            {
                Logging.WriteError("Flying To Ground Profiles Converter > Main > Initialize(): " + e);
            }
        }

        public void Dispose()
        {
            try
            {
                Stop();
                Logging.Status = "Dispose Flying To Ground Profiles Converter Complete";
                Logging.Write("Dispose Flying To Ground Profiles Converter Complete");
            }
            catch (Exception e)
            {
                Logging.WriteError("Flying To Ground Profiles Converter > Main > Dispose(): " + e);
            }
        }

        public void Start()
        {
            try
            {
                _welcomeForm = new WelcomeForm();
                _welcomeForm.Show();
                IsStarted = true;
                Logging.Status = "Flying To Ground Profiles Converter started";
            }
            catch (Exception e)
            {
                Logging.WriteError("Flying To Ground Profiles Converter > Main > Start(): " + e);
            }
        }

        public void Stop()
        {
            try
            {
                if (_welcomeForm != null)
                    _welcomeForm.Dispose();
                IsStarted = false;
                Logging.Status = "Flying To Ground Profiles Converter stoped";
            }
            catch (Exception e)
            {
                Logging.WriteError("Flying To Ground Profiles Converter > Main > Stop(): " + e);
            }
        }

        public void Settings()
        {
            try
            {
                MessageBox.Show(string.Format("{0}.", Translate.Get(Translate.Id.No_setting_for_this_product)));
                Logging.Status = "Settings Flying To Ground Profiles Converter Complete";
                Logging.Write("Settings Flying To Ground Profiles Converter Complete");
            }
            catch (Exception e)
            {
                Logging.WriteError("Flying To Ground Profiles Converter > Main > Settings(): " + e);
            }
        }

        public bool IsStarted { get; private set; }

        #endregion
    }
}