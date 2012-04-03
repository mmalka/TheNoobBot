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
            Logging.Status = "Initialize Smelting Complete";
            Logging.Write("Initialize Smelting Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Smelting > Main > Initialize(): " + e);
        }
    }

    public void Dispose()
    {
        try
        {
            Stop();
            Logging.Status = "Dispose Smelting Complete";
            Logging.Write("Dispose Smelting Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Smelting > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            Smelting.Smelting.Pulse();
            _isStarted = true;
            Logging.Status = "Smelting started";
        }
        catch (Exception e)
        {
            Logging.WriteError("Smelting > Main > Start(): " + e);
        }
    }

    public void Stop()
    {
        try
        {
            _isStarted = false;
            Logging.Status = "Smelting stoped";
        }
        catch (Exception e)
        {
            Logging.WriteError("Smelting > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.No_setting_for_this_product));
            Logging.Status = "Settings Smelting Complete";
            Logging.Write("Settings Smelting Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Smelting > Main > Settings(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    private bool _isStarted;

    #endregion
}