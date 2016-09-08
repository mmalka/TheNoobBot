using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Fisherbot;
using Fisherbot.Bot;
using nManager.Helpful;
using nManager.Products;
using nManager.Wow.Enums;
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
            {
                FisherbotSetting.CurrentSetting.WeaponName = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_WEAPONMAINHAND).Name;
                FisherbotSetting.CurrentSetting.ShieldName = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_SHIELD).Name;
                if (FisherbotSetting.CurrentSetting.WeaponName == FisherbotSetting.CurrentSetting.ShieldName)
                    FisherbotSetting.CurrentSetting.ShieldName = "";
            }

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

    public void RemoteStart(string[] args)
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        try
        {
            if (ObjectManager.Me.IsCasting)
                MovementManager.MicroMove();
            // Cancel cast if we were fishing else the weapon wont be replaced.
            ItemsManager.EquipItemByName(FisherbotSetting.CurrentSetting.WeaponName);
            if (!string.IsNullOrEmpty(FisherbotSetting.CurrentSetting.ShieldName))
                ItemsManager.EquipItemByName(FisherbotSetting.CurrentSetting.ShieldName);
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