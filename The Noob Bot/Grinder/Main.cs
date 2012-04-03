// ReSharper disable CheckNamespace

using System;
using System.IO;
using System.Windows.Forms;
using Grinder.Bot;
using nManager;
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
            Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\Grinder\\");
            GrinderSetting.Load();
            Logging.Status = "Initialize Grinder Complete";
            Logging.Write("Initialize Grinder Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Grinder > Main > Initialize(): " + e);
        }
    }

    public void Dispose()
    {
        try
        {
            Stop();
            Logging.Status = "Dispose Grinder Complete";
            Logging.Write("Dispose Grinder Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Grinder > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            if (Bot.Pulse())
            {
                _isStarted = true;
                Logging.Status = "Start Grinder Complete";
                Logging.Write("Start Grinder Complete");
            }
            else
            {
                Logging.Status = "Start Grinder failed";
                Logging.Write("Start Grinder failed");
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("Grinder > Main > Start(): " + e);
        }
    }

    public void Stop()
    {
        try
        {
            Bot.Dispose();
            _isStarted = false;
            Logging.Status = "Stop Grinder Complete";
            Logging.Write("Stop Grinder Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Grinder > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            MessageBox.Show(Translate.Get(Translate.Id.No_setting_for_this_product));
            Logging.Status = "Settings Grinder Complete";
            Logging.Write("Settings Grinder Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Grinder > Main > Settings(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    private bool _isStarted;

    #endregion
}