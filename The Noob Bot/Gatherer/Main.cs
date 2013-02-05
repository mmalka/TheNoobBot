// ReSharper disable CheckNamespace

using System;
using System.IO;
using System.Windows.Forms;
using Gatherer.Bot;
using nManager;
using nManager.Helpful;
using nManager.Products;

public class Main : IProduct
// ReSharper restore CheckNamespace
{
    #region IProduct Members

    public void Initialize()
    {
        try
        {
            Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\Gatherer\\");
            GathererSetting.Load();
            Logging.Status = "Initialize Gatherer Complete";
            Logging.Write("Initialize Gatherer Complete");
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
            Logging.Status = "Dispose Gatherer Complete";
            Logging.Write("Dispose Gatherer Complete");
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
            if (Bot.Pulse())
            {
                _isStarted = true;
                Logging.Status = "Start Gatherer Complete";
                Logging.Write("Start Gatherer Complete");
            }
            else
            {
                Logging.Status = "Start Gatherer failed";
                Logging.Write("Start Gatherer failed");
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
            Logging.Status = "Stop Gatherer Complete";
            Logging.Write("Stop Gatherer Complete");
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
            Logging.Status = "Settings Gatherer Complete";
            Logging.Write("Settings Gatherer Complete");
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