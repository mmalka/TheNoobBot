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
            Logging.Status = "Initialize Profiles Converters Complete";
            Logging.Write("Initialize Profiles Converters Complete");
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
            Logging.Status = "Dispose Profiles Converters Complete";
            Logging.Write("Dispose Profiles Converters Complete");
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
            Logging.Status = "Profiles Converters started";
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
            Logging.Status = "Profiles Converters stoped";
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
            Logging.Status = "Settings Profiles Converters Complete";
            Logging.Write("Settings Profiles Converters Complete");
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