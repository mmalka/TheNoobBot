// ReSharper disable CheckNamespace

using System;
using System.IO;
using System.Windows.Forms;
using Gatherer.Bot;
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
            Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\Gatherer\\");
            GathererSetting.Load();
            Logging.Status = "Initialize Gatherer Complete";
            Logging.Write("Initialize Gatherer Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Gatherer > Main > Initialize(): " + e);
        }
    }

    public void Dispose()
    {
        try
        {
            Stop();
            Logging.Status = "Dispose Gatherer Complete";
            Logging.Write("Dispose Gatherer Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Gatherer > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            if (Bot.Pulse())
            {
                _isStarted = true;
                Logging.Status = "Start Gatherer Complete";
                Logging.Write("Start Gatherer Complete");
            }
            else
            {
                Logging.Status = "Start Gatherer failed";
                Logging.Write("Start Gatherer failed");
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("Gatherer > Main > Start(): " + e);
        }
    }

    public void Stop()
    {
        try
        {
            Bot.Dispose();
            _isStarted = false;
            Logging.Status = "Stop Gatherer Complete";
            Logging.Write("Stop Gatherer Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Gatherer > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            GathererSetting.CurrentSetting.ToForm();
            GathererSetting.CurrentSetting.Save();
            Logging.Status = "Settings Gatherer Complete";
            Logging.Write("Settings Gatherer Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Gatherer > Main > Settings(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    private bool _isStarted;

    #endregion
}