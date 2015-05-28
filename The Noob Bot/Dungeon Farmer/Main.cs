using System;
using System.Windows.Forms;
using DungeonFarmer;
using DungeonFarmer.Bot;
using nManager;
using nManager.Helpful;
using nManager.Products;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.ObjectManager;

public class Main : IProduct
{
    #region IProduct Members

    private string _looting;
    private string _mindistground;
    private string _usefly;
    private string _useground;

    public void Initialize()
    {
        try
        {
            DungeonFarmerSetting.Load();
            Others.ProductStatusLog(Products.ProductName, 1);
            GetProductTipOff();
        }
        catch (Exception e)
        {
            Logging.WriteError("DungeonFarmer > Main > Initialize(): " + e);
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
            Logging.WriteError("DungeonFarmer > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            Others.ProductStatusLog(Products.ProductName, 3);
            if (Bot.Pulse())
            {
                IsStarted = true;
                Others.ProductStatusLog(Products.ProductName, 4);
            }
            else
            {
                Others.ProductStatusLog(Products.ProductName, 5);
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("DungeonFarmer > Main > Start(): " + e);
        }
    }

    public void Stop()
    {
        try
        {
            Bot.Dispose();
            IsStarted = false;
            Others.ProductStatusLog(Products.ProductName, 6);
        }
        catch (Exception e)
        {
            Logging.WriteError("DungeonFarmer > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            var f = new DungeonFarmerSettingsFrame();
            f.ShowDialog();
            Others.ProductStatusLog(Products.ProductName, 7);
        }
        catch (Exception e)
        {
            Logging.WriteError("DungeonFarmer > Main > Settings(): " + e);
        }
    }

    public bool IsStarted { get; private set; }

    private void GetProductTipOff()
    {
        try
        {
            return; // Disable ProductTipOff for now.

            if (ObjectManager.Me.Level < 90 &&
                nManagerSetting.CurrentSetting.ActivateMonsterLooting)
                _looting = "\n" + Translate.Get(Translate.Id.TipOffLootingOffArchaeologist);
            else if (ObjectManager.Me.Level == 90 &&
                     !nManagerSetting.CurrentSetting.ActivateMonsterLooting)
                _looting = "\n" + Translate.Get(Translate.Id.TipOffLootingOnArchaeologist);
            if (MountTask.GetMountCapacity() >= MountCapacity.Ground)
            {
                if (!nManagerSetting.CurrentSetting.UseGroundMount)
                    _useground = "\n" + Translate.Get(Translate.Id.TipOffUseGroundMountOn);
                else if (nManagerSetting.CurrentSetting.UseGroundMount &&
                         string.IsNullOrEmpty(nManagerSetting.CurrentSetting.GroundMountName))
                    _useground = "\n" + Translate.Get(Translate.Id.TipOffEmptyGroundMount);
                if (nManagerSetting.CurrentSetting.MinimumDistanceToUseMount < 27 || nManagerSetting.CurrentSetting.MinimumDistanceToUseMount > 33)
                    _mindistground = "\n" + Translate.Get(Translate.Id.TipOffMinimumDistanceToUseGroundMount);
            }
            if (MountTask.GetMountCapacity() == MountCapacity.Fly)
            {
                if (string.IsNullOrEmpty(nManagerSetting.CurrentSetting.FlyingMountName))
                    _usefly = "\n" + Translate.Get(Translate.Id.TipOffEmptyFlyingMount);
            }
            if (_looting != null || _useground != null || _usefly != null)
            {
                MessageBox.Show(
                    string.Format("{0}\n{1}{2}{3}{4}", Translate.Get(Translate.Id.ArchaeologistTipOffMessage), _looting,
                        _useground, _mindistground, _usefly), Translate.Get(Translate.Id.ArchaeologistTipOffTitle));
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("DungeonFarmer > Main > GetProductTipOff(): " + e);
        }
    }

    #endregion
}