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

            Others.ProductStatusLog(Products.ProductName, 1);
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
            Others.ProductStatusLog(Products.ProductName, 2);
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
            Logging.WriteError("Fisherbot > Main > Start(): " + e);
        }
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
            Logging.WriteError("Fisherbot > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            SettingsFisherbotForm f = new SettingsFisherbotForm();
            f.ShowDialog();
            Others.ProductStatusLog(Products.ProductName, 7);
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