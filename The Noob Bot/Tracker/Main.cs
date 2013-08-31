using System;
using System.Windows.Forms;
using Tracker;
using nManager.Helpful;
using nManager.Products;

public class Main : IProduct
{
    #region IProduct Members

    private FormTracker _formTracker;

    public void Initialize()
    {
        try
        {
            Logging.Status = "Initialize Tracker Complete";
            Logging.Write("Initialize Tracker Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Tracker > Main > Initialize(): " + e);
        }
    }

    public void Dispose()
    {
        try
        {
            Stop();
            Logging.Status = "Dispose Tracker Complete";
            Logging.Write("Dispose Tracker Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Tracker > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            _formTracker = new FormTracker();
            _formTracker.Show();
            _isStarted = true;
            Logging.Status = "Tracker started";
        }
        catch (Exception e)
        {
            Logging.WriteError("Tracker > Main > Start(): " + e);
        }
    }

    public void Stop()
    {
        try
        {
            _formTracker.Dispose();
            _isStarted = false;
            Logging.Status = "Tracker stoped";
        }
        catch (Exception e)
        {
            Logging.WriteError("Tracker > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.No_setting_for_this_product));
            Logging.Status = "Settings Tracker Complete";
            Logging.Write("Settings Tracker Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Tracker > Main > Settings(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    private bool _isStarted;

    #endregion
}