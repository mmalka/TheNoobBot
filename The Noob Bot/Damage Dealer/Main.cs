using System;
using Damage_Dealer;
using Damage_Dealer.Bot;
using nManager.Helpful;
using nManager.Products;
using nManager.Wow.Helpers;

public class Main : IProduct
{
    #region IProduct Members

    private bool _isStarted;

    public void Initialize()
    {
        try
        {
            DamageDealerSettings.Load();
            Others.ProductStatusLog(Products.ProductName, 1);
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
            Others.ProductStatusLog(Products.ProductName, 2);
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
            if (!DamageDealerSettings.CurrentSetting.ActivateMovements)
                ConfigWowForThisBot.StartStopClickToMove(false);
            Others.ProductStatusLog(Products.ProductName, 3);
            if (Bot.Pulse())
            {
                _isStarted = true;
                Others.ProductStatusLog(Products.ProductName, 4);
            }
            else
            {
                Others.ProductStatusLog(Products.ProductName, 5);
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("Damage Dealer > Main > Start(): " + e);
        }
    }

    public void RemoteStart(string[] args)
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        try
        {
            ConfigWowForThisBot.StartStopClickToMove();
            Bot.Dispose();
            _isStarted = false;
            Others.ProductStatusLog(Products.ProductName, 6);
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
            var f = new DamageDealerSettingsForm();
            f.ShowDialog();
            Others.ProductStatusLog(Products.ProductName, 7);
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