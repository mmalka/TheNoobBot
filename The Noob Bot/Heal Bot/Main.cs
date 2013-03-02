// ReSharper disable CheckNamespace

using System;
using System.Windows.Forms;
using HealBot.Bot;
using nManager;
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
            Logging.Status = "Initialize Heal Bot Complete";
            Logging.Write("Initialize Heal Bot Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Heal Bot > Main > Initialize(): " + e);
        }
    }

    public void Dispose()
    {
        try
        {
            Stop();
            Logging.Status = "Dispose Heal Bot Complete";
            Logging.Write("Dispose Heal Bot Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Heal Bot > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            if (Bot.Pulse())
            {
                _isStarted = true;
                Logging.Status = "Heal Bot started";
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("Heal Bot > Main > Start(): " + e);
        }
    }

    public void Stop()
    {
        try
        {
            Bot.Dispose();
            _isStarted = false;
            Logging.Status = "Heal Bot stoped";
        }
        catch (Exception e)
        {
            Logging.WriteError("Heal Bot > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            MessageBox.Show(Translate.Get(Translate.Id.No_setting_for_this_product));
            Logging.Status = "Settings Heal Bot Complete";
            Logging.Write("Settings Heal Bot Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Heal Bot > Main > Settings(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    #endregion
}