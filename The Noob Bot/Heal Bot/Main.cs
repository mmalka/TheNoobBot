using System;
using Heal_Bot;
using Heal_Bot.Bot;
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
            HealBotSettings.Load();
            Others.ProductStatusLog(Products.ProductName, 1);
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
            Others.ProductStatusLog(Products.ProductName, 2);
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
            Logging.WriteError("Heal Bot > Main > Start(): " + e);
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
            Bot.Dispose();
            _isStarted = false;
            Others.ProductStatusLog(Products.ProductName, 6);
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
            var f = new HealBotSettingsForm();
            f.ShowDialog();
            Others.ProductStatusLog(Products.ProductName, 7);
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