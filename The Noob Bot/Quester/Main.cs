using System;
using System.IO;
using System.Windows.Forms;
using Quester.Bot;
using nManager;
using nManager.Helpful;
using nManager.Products;

public class Main : IProduct
{
    #region IProduct Members

    public void Initialize()
    {
        try
        {
            Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\Quester\\");
            Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\Quester\\Grouped\\");
            QuesterSettings.Load();
            Logging.Status = "Initialize Quester Complete";
            Logging.Write("Initialize Quester Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Quester > Main > Initialize(): " + e);
        }
    }

    public void Dispose()
    {
        try
        {
            Stop();
            Logging.Status = "Dispose Quester Complete";
            Logging.Write("Dispose Quester Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Quester > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            if (Bot.Pulse())
            {
                _isStarted = true;
                Logging.Status = "Start Quester Complete";
                Logging.Write("Start Quester Complete");
            }
            else
            {
                Logging.Status = "Start Quester failed";
                Logging.Write("Start Quester failed");
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("Quester > Main > Start(): " + e);
        }
    }

    public void Stop()
    {
        try
        {
            Bot.Dispose();
            _isStarted = false;
            Logging.Status = "Stop Quester Complete";
            Logging.Write("Stop Quester Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Quester > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            MessageBox.Show(Translate.Get(Translate.Id.No_setting_for_this_product));
            Logging.Status = "Settings Quester Complete";
            Logging.Write("Settings Quester Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Quester > Main > Settings(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    private bool _isStarted;

    #endregion
}