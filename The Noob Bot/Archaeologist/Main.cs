// ReSharper disable CheckNamespace

using System;
using Archaeologist;
using Archaeologist.Bot;
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
            ArchaeologistSetting.Load();
            Logging.Status = "Initialize Archaeologist Complete";
            Logging.Write("Initialize Archaeologist Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Archaeologist > Main > Initialize(): " + e);
        }
    }

    public void Dispose()
    {
        try
        {
            Stop();
            Logging.Status = "Dispose Archaeologist Complete";
            Logging.Write("Dispose Archaeologist Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Archaeologist > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            if (Bot.Pulse())
            {
                _isStarted = true;
                Logging.Status = "Start Archaeologist Complete";
                Logging.Write("Start Archaeologist Complete");
            }
            else
            {
                Logging.Status = "Start Archaeologist failed";
                Logging.Write("Start Archaeologist failed");
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("Archaeologist > Main > Start(): " + e);
        }
    }

    public void Stop()
    {
        try
        {
            Bot.Dispose();
            _isStarted = false;
            Logging.Status = "Stop Archaeologist Complete";
            Logging.Write("Stop Archaeologist Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Archaeologist > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            var f = new DigSites_List_Management();
            f.ShowDialog();
            Logging.Status = "Settings Archaeologist Complete";
            Logging.Write("Settings Archaeologist Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Archaeologist > Main > Settings(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    private bool _isStarted;

    #endregion
}