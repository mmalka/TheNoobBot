// ReSharper disable CheckNamespace

using System;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Products;

public class Main : IProduct
// ReSharper restore CheckNamespace
{
    #region IProduct Members

    public void Initialize()
    {
        try
        {
            Logging.Status = "Initialize Disenchanting Complete";
            Logging.Write("Initialize Disenchanting Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Disenchanting > Main > Initialize(): " + e);
        }
    }

    public void Dispose()
    {
        try
        {
            Stop();
            Logging.Status = "Dispose Disenchanting Complete";
            Logging.Write("Dispose Disenchanting Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Disenchanting > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            Disenchanting.Disenchanting.Pulse();
            _isStarted = true;
            Logging.Status = "Disenchanting started";
        }
        catch (Exception e)
        {
            Logging.WriteError("Disenchanting > Main > Start(): " + e);
        }
    }

    public void Stop()
    {
        try
        {
            _isStarted = false;
            Logging.Status = "Disenchanting stoped";
        }
        catch (Exception e)
        {
            Logging.WriteError("Disenchanting > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.No_setting_for_this_product));
            Logging.Status = "Settings Disenchanting Complete";
            Logging.Write("Settings Disenchanting Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Disenchanting > Main > Settings(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    private bool _isStarted;

    #endregion
}