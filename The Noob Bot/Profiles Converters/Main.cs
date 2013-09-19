using System;
using System.Windows.Forms;
using Profiles_Converters;
using nManager.Helpful;
using nManager.Products;

public class Main : IProduct
{
    #region IProduct Members

    private formMain formMain;

    public void Initialize()
    {
        try
        {
            Others.ProductStatusLog(Products.ProductName, 1);
        }
        catch (Exception e)
        {
            Logging.WriteError("Profiles Converters > Main > Initialize(): " + e);
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
            Logging.WriteError("Profiles Converters > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            formMain = new formMain();
            formMain.Show();
            _isStarted = true;
            Others.ProductStatusLog(Products.ProductName, 4);
        }
        catch (Exception e)
        {
            Logging.WriteError("Profiles Converters > Main > Start(): " + e);
        }
    }

    public void Stop()
    {
        try
        {
            if (formMain != null)
                formMain.Dispose();
            _isStarted = false;
            Others.ProductStatusLog(Products.ProductName, 6);
        }
        catch (Exception e)
        {
            Logging.WriteError("Profiles Converters > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.No_setting_for_this_product) + ".");
            Others.ProductStatusLog(Products.ProductName, 7);
        }
        catch (Exception e)
        {
            Logging.WriteError("Profiles Converters > Main > Settings(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    private bool _isStarted;

    #endregion
}