// ReSharper disable CheckNamespace

using System;
using System.IO;
using System.Windows.Forms;
using Test_Product;
using nManager.Helpful;
using nManager.Products;

public class Main : IProduct
// ReSharper restore CheckNamespace
{
    #region IProduct Members

    private bool _isStarted;

    public void Initialize()
    {
        try
        {
            Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\Test_Product\\");
            Logging.Status = "Initialize Test_Product Complete";
            Logging.Write("Initialize Test_Product Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Test_Product > Main > Initialize(): " + e);
        }
    }

    public void Dispose()
    {
        try
        {
            Stop();
            Logging.Status = "Dispose Test_Product Complete";
            Logging.Write("Dispose Test_Product Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Test_Product > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            if (Bot.Pulse())
            {
                _isStarted = true;
                Logging.Status = "Start Test_Product Complete";
                Logging.Write("Start Test_Product Complete");
            }
            else
            {
                Logging.Status = "Start Test_Product failed";
                Logging.Write("Start Test_Product failed");
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("Test_Product > Main > Start(): " + e);
        }
    }

    public void Stop()
    {
        try
        {
            Bot.Dispose();
            _isStarted = false;
            Logging.Status = "Stop Test_Product Complete";
            Logging.Write("Stop Test_Product Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Test_Product > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            MessageBox.Show("No setting for this product.");
            Logging.Status = "Settings Test_Product Complete";
            Logging.Write("Settings Test_Product Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Test_Product > Main > Settings(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    #endregion
}