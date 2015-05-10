using System;
using System.Windows.Forms;
using DungeonFarmer;
using DungeonFarmer.Bot;
using nManager;
using nManager.Helpful;
using nManager.Products;
using nManager.Wow.Bot.Tasks;

public class Main : IProduct
{
    #region IProduct Members

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
            Logging.WriteError("DungeonFarmer > Main > Start(): " + e);
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
            Logging.WriteError("DungeonFarmer > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            DungeonFarmerSettingsFrame f = new DungeonFarmerSettingsFrame();
            f.ShowDialog();
            Others.ProductStatusLog(Products.ProductName, 7);
        }
        catch (Exception e)
        {
            Logging.WriteError("DungeonFarmer > Main > Settings(): " + e);
        }
    }

    private string _looting;
    private string _useground;
    private string _mindistground;
    private string _usefly;

    private void GetProductTipOff()
    {
        try
        {
            if (nManager.Wow.ObjectManager.ObjectManager.Me.Level < 90 &&
                nManagerSetting.CurrentSetting.ActivateMonsterLooting)
                _looting = "\n" + Translate.Get(Translate.Id.TipOffLootingOffArchaeologist);
            else if (nManager.Wow.ObjectManager.ObjectManager.Me.Level == 90 &&
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
            Logging.WriteError("Battlegrounder > Main > GetProductTipOff(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    private bool _isStarted;

    #endregion
}