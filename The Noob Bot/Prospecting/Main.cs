using System;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Products;

public class Main : IProduct
{
    #region IProduct Members

    public void Initialize()
    {
        try
        {
            Logging.Status = "Initialize Prospecting Complete";
            Logging.Write("Initialize Prospecting Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Prospecting > Main > Initialize(): " + e);
        }
    }

    public void Dispose()
    {
        try
        {
            Stop();
            Logging.Status = "Dispose Prospecting Complete";
            Logging.Write("Dispose Prospecting Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Prospecting > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            Prospecting.Prospecting.Pulse();
            _isStarted = true;
            Logging.Status = "Prospecting started";
        }
        catch (Exception e)
        {
            Logging.WriteError("Prospecting > Main > Start(): " + e);
        }
    }

    public void Stop()
    {
        try
        {
            _isStarted = false;
            Logging.Status = "Prospecting stoped";
        }
        catch (Exception e)
        {
            Logging.WriteError("Prospecting > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.No_setting_for_this_product));
            Logging.Status = "Settings Prospecting Complete";
            Logging.Write("Settings Prospecting Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Prospecting > Main > Settings(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    private bool _isStarted;

    #endregion
}