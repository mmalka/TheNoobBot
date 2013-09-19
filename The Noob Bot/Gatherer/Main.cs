using System;
using System.IO;
using System.Windows.Forms;
using Gatherer.Bot;
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
            Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\Gatherer\\");
            GathererSetting.Load();
            Others.ProductStatusLog(Products.ProductName, 1);
            GetProductTipOff();
        }
        catch (Exception e)
        {
            Logging.WriteError("Gatherer > Main > Initialize(): " + e);
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
            Logging.WriteError("Gatherer > Main > Dispose(): " + e);
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
            Logging.WriteError("Gatherer > Main > Start(): " + e);
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
            Logging.WriteError("Gatherer > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            GathererSetting.CurrentSetting.ToForm();
            GathererSetting.CurrentSetting.Save();
            Others.ProductStatusLog(Products.ProductName, 7);
        }
        catch (Exception e)
        {
            Logging.WriteError("Gatherer > Main > Settings(): " + e);
        }
    }

    private string _looting;
    private string _radius;
    private string _useground;
    private string _usefly;

    private void GetProductTipOff()
    {
        try
        {
            if (nManagerSetting.CurrentSetting.ActivateMonsterLooting)
                _looting = "\n" + Translate.Get(Translate.Id.TipOffLootingOff);
            if (nManagerSetting.CurrentSetting.GatheringSearchRadius < 100)
                _radius = "\n" + Translate.Get(Translate.Id.TipOffRadiusHigh);
            if (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 20 &&
                nManager.Wow.ObjectManager.ObjectManager.Me.Level < 60)
            {
                if (!nManagerSetting.CurrentSetting.UseGroundMount)
                    _useground = "\n" + Translate.Get(Translate.Id.TipOffUseGroundMountOn);
                else if (nManagerSetting.CurrentSetting.UseGroundMount &&
                         string.IsNullOrEmpty(nManagerSetting.CurrentSetting.GroundMountName))
                    _useground = "\n" + Translate.Get(Translate.Id.TipOffEmptyGroundMount);
            }
            else if (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 60)
            {
                if (nManagerSetting.CurrentSetting.UseGroundMount)
                    _useground = "\n" + Translate.Get(Translate.Id.TipOffUseGroundMountOff);
                if (string.IsNullOrEmpty(nManagerSetting.CurrentSetting.FlyingMountName))
                    _usefly = "\n" + Translate.Get(Translate.Id.TipOffEmptyFlyingMount);
            }
            if (_radius != null || _looting != null || _useground != null || _usefly != null)
            {
                MessageBox.Show(
                    string.Format("{0}\n{1}{2}{3}{4}", Translate.Get(Translate.Id.GathererTipOffMessage), _looting,
                                  _radius, _useground, _usefly), Translate.Get(Translate.Id.GathererTipOffTitle));
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("Gatherer > Main > GetProductTipOff(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    private bool _isStarted;

    #endregion
}