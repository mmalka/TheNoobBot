using System;
using System.Windows.Forms;
using Flying_To_Ground_Profiles_Converter;
using nManager;
using nManager.Helpful;
using nManager.Products;

public class Main : IProduct
{
    #region IProduct Members

    private WelcomeForm _welcomeForm;

    public void Initialize()
    {
        try
        {
            Others.ProductStatusLog(Products.ProductName, 1);
        }
        catch (Exception e)
        {
            Logging.WriteError("Flying To Ground Profiles Converter > Main > Initialize(): " + e);
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
            Logging.WriteError("Flying To Ground Profiles Converter > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            _welcomeForm = new WelcomeForm();
            _welcomeForm.Show();
            IsStarted = true;
            Others.ProductStatusLog(Products.ProductName, 4);
        }
        catch (Exception e)
        {
            Logging.WriteError("Flying To Ground Profiles Converter > Main > Start(): " + e);
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
            if (_welcomeForm != null)
                _welcomeForm.Dispose();
            IsStarted = false;
            Others.ProductStatusLog(Products.ProductName, 6);
        }
        catch (Exception e)
        {
            Logging.WriteError("Flying To Ground Profiles Converter > Main > Stop(): " + e);
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
            Logging.WriteError("Flying To Ground Profiles Converter > Main > Settings(): " + e);
        }
    }

    public bool IsStarted { get; private set; }

    #endregion
}