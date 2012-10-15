// ReSharper disable CheckNamespace

using System;
using System.IO;
using System.Windows.Forms;
using Battlegrounder;
using Battlegrounder.Bot;
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
            Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\Battlegrounder\\");
            BattlegrounderSetting.Load();
            Logging.Status = "Initialize Battlegrounder Complete";
            Logging.Write("Initialize Battlegrounder Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Battlegrounder > Main > Initialize(): " + e);
        }
    }

    public void Dispose()
    {
        try
        {
            Stop();
            Logging.Status = "Dispose Battlegrounder Complete";
            Logging.Write("Dispose Battlegrounder Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Battlegrounder > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            if (Bot.Pulse())
            {
                _isStarted = true;
                Logging.Status = "Start Battlegrounder Complete";
                Logging.Write("Start Battlegrounder Complete");
            }
            else
            {
                Logging.Status = "Start Battlegrounder failed";
                Logging.Write("Start Battlegrounder failed");
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("Battlegrounder > Main > Start(): " + e);
        }
    }

    public void Stop()
    {
        try
        {
            Bot.Dispose();
            _isStarted = false;
            Logging.Status = "Stop Battlegrounder Complete";
            Logging.Write("Stop Battlegrounder Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Battlegrounder > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            var f = new SettingsBattlegrounderForm();
            f.ShowDialog();
            Logging.Status = "Battlegrounder Settings Complete";
            Logging.Write("Battlegrounder Settings Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Battlegrounder > Main > Settings(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    private bool _isStarted;

    #endregion
}