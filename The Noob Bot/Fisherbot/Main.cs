using System;
using System.IO;
using System.Windows.Forms;
using Fisherbot;
using Fisherbot.Bot;
using nManager.Helpful;
using nManager.Products;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using nManager.Wow.Patchables;

public class Main : IProduct
{
    #region IProduct Members

    public void Initialize()
    {
        try
        {
            Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\Fisherbot\\");
            FisherbotSetting.Load();

            if (string.IsNullOrWhiteSpace(FisherbotSetting.CurrentSetting.FishingPoleName))
                FisherbotSetting.CurrentSetting.FishingPoleName = Fishing.FishingPolesName();
            if (string.IsNullOrWhiteSpace(FisherbotSetting.CurrentSetting.WeaponName))
                FisherbotSetting.CurrentSetting.WeaponName =
                    ItemsManager.GetItemNameById(
                        (int) ObjectManager.Me.GetDescriptor<uint>(Descriptors.PlayerFields.VisibleItems + 15*2));

            Logging.Status = "Initialize Fisherbot Complete";
            Logging.Write("Initialize Fisherbot Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Fisherbot > Main > Initialize(): " + e);
        }
    }

    public void Dispose()
    {
        try
        {
            Stop();
            Logging.Status = "Dispose Fisherbot Complete";
            Logging.Write("Dispose Fisherbot Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Fisherbot > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            if (Bot.Pulse())
            {
                _isStarted = true;
                Logging.Status = "Start Fisherbot Complete";
                Logging.Write("Start Fisherbot Complete");
            }
            else
            {
                Logging.Status = "Start Fisherbot failed";
                Logging.Write("Start Fisherbot failed");
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("Fisherbot > Main > Start(): " + e);
        }
    }

    public void Stop()
    {
        try
        {
            Bot.Dispose();
            _isStarted = false;
            Logging.Status = "Stop Fisherbot Complete";
            Logging.Write("Stop Fisherbot Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Fisherbot > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            SettingsFisherbotForm f = new SettingsFisherbotForm();
            f.ShowDialog();
            Logging.Status = "Settings Fisherbot Complete";
            Logging.Write("Settings Fisherbot Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Fisherbot > Main > Settings(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    private bool _isStarted;

    #endregion
}