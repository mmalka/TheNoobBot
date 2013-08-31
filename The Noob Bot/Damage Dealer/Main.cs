using System;
using System.Windows.Forms;
using nManager;
using Damage_Dealer.Bot;
using nManager.Helpful;
using nManager.Products;

public class Main : IProduct
{
    #region IProduct Members

    private bool _isStarted;

    public void Initialize()
    {
        try
        {
            Logging.Status = "Initialize Damage Dealer Complete";
            Logging.Write("Initialize Damage Dealer Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Damage Dealer > Main > Initialize(): " + e);
        }
    }

    public void Dispose()
    {
        try
        {
            Stop();
            Logging.Status = "Dispose Damage Dealer Complete";
            Logging.Write("Dispose Damage Dealer Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Damage Dealer > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            if (Bot.Pulse())
            {
                _isStarted = true;
                Logging.Status = "Damage Dealer started";
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("Damage Dealer > Main > Start(): " + e);
        }
    }

    public void Stop()
    {
        try
        {
            Bot.Dispose();
            _isStarted = false;
            Logging.Status = "Damage Dealer stoped";
        }
        catch (Exception e)
        {
            Logging.WriteError("Damage Dealer > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            MessageBox.Show(Translate.Get(Translate.Id.No_setting_for_this_product));
            Logging.Status = "Settings Damage Dealer Complete";
            Logging.Write("Settings Damage Dealer Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Damage Dealer > Main > Settings(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    #endregion
}