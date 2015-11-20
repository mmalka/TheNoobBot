using System;
using System.Windows.Forms;
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
            Others.ProductStatusLog(Products.ProductName, 1);
        }
        catch (Exception e)
        {
            Logging.WriteError("Milling > Main > Initialize(): " + e);
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
            Logging.WriteError("Milling > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            Milling.Milling.Pulse();
            _isStarted = true;
            Others.ProductStatusLog(Products.ProductName, 4);
        }
        catch (Exception e)
        {
            Logging.WriteError("Milling > Main > Start(): " + e);
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
            _isStarted = false;
            Others.ProductStatusLog(Products.ProductName, 6);
        }
        catch (Exception e)
        {
            Logging.WriteError("Milling > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            MessageBox.Show(string.Format("{0}.", Translate.Get(Translate.Id.No_setting_for_this_product)));
            Others.ProductStatusLog(Products.ProductName, 7);
        }
        catch (Exception e)
        {
            Logging.WriteError("Milling > Main > Settings(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    private bool _isStarted;

    #endregion
}