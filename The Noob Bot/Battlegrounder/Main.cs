using System;
using System.IO;
using System.Windows.Forms;
using Battlegrounder;
using Battlegrounder.Bot;
using nManager;
using nManager.Helpful;
using nManager.Products;
using nManager.Wow.ObjectManager;

public class Main : IProduct
{
    #region IProduct Members

    private bool _isStarted;
    private string _looting;
    private string _radius;
    private string _useground;

    public void Initialize()
    {
        try
        {
            Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\Battlegrounder\\");
            Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\Battlegrounder\\ProfileType\\");
            Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\Battlegrounder\\CSharpProfile\\");
            Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\Battlegrounder\\AfkSomewhere\\");
            BattlegrounderSetting.Load();
            Others.ProductStatusLog(Products.ProductName, 1);
            GetProductTipOff();
        }
        catch (Exception e)
        {
            Logging.WriteError("Battlegrounder > Main > Initialize(): " + e);
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
            Logging.WriteError("Battlegrounder > Main > Dispose(): " + e);
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
            Logging.WriteError("Battlegrounder > Main > Start(): " + e);
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
            Logging.WriteError("Battlegrounder > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            SettingsBattlegrounderForm f = new SettingsBattlegrounderForm();
            f.ShowDialog();
            Others.ProductStatusLog(Products.ProductName, 7);
        }
        catch (Exception e)
        {
            Logging.WriteError("Battlegrounder > Main > Settings(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    private void GetProductTipOff()
    {
        try
        {
            if (nManagerSetting.CurrentSetting.ActivateMonsterLooting)
                _looting = "\n" + Translate.Get(Translate.Id.TipOffLootingOff);
            if (nManagerSetting.CurrentSetting.GatheringSearchRadius > 30)
                _radius = "\n" + Translate.Get(Translate.Id.TipOffRadiusLow);
            if (ObjectManager.Me.Level >= 20)
            {
                if (!nManagerSetting.CurrentSetting.UseGroundMount)
                    _useground = "\n" + Translate.Get(Translate.Id.TipOffUseGroundMountOn);
                else if (nManagerSetting.CurrentSetting.UseGroundMount &&
                         string.IsNullOrEmpty(nManagerSetting.CurrentSetting.GroundMountName))
                    _useground = "\n" + Translate.Get(Translate.Id.TipOffEmptyGroundMount);
            }
            if (_radius != null || _looting != null || _useground != null)
            {
                MessageBox.Show(
                    string.Format("{0}\n{1}{2}{3}", Translate.Get(Translate.Id.BattlegrounderTipOffMessage), _looting,
                                  _radius, _useground), Translate.Get(Translate.Id.BattlegrounderTipOffTitle));
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("Battlegrounder > Main > GetProductTipOff(): " + e);
        }
    }

    #endregion
}