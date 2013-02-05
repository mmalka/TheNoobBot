// ReSharper disable CheckNamespace

using System;
using System.IO;
using System.Windows.Forms;
using Grinder.Bot;
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
            Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\Grinder\\");
            GrinderSetting.Load();
            Logging.Status = "Initialize Grinder Complete";
            Logging.Write("Initialize Grinder Complete");
            GetProductTipOff();
        }
        catch (Exception e)
        {
            Logging.WriteError("Grinder > Main > Initialize(): " + e);
        }
    }

    public void Dispose()
    {
        try
        {
            Stop();
            Logging.Status = "Dispose Grinder Complete";
            Logging.Write("Dispose Grinder Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Grinder > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            if (Bot.Pulse())
            {
                _isStarted = true;
                Logging.Status = "Start Grinder Complete";
                Logging.Write("Start Grinder Complete");
            }
            else
            {
                Logging.Status = "Start Grinder failed";
                Logging.Write("Start Grinder failed");
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("Grinder > Main > Start(): " + e);
        }
    }

    public void Stop()
    {
        try
        {
            Bot.Dispose();
            _isStarted = false;
            Logging.Status = "Stop Grinder Complete";
            Logging.Write("Stop Grinder Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Grinder > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            MessageBox.Show(Translate.Get(Translate.Id.No_setting_for_this_product));
            Logging.Status = "Settings Grinder Complete";
            Logging.Write("Settings Grinder Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Grinder > Main > Settings(): " + e);
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
            if (!nManagerSetting.CurrentSetting.ActivateMonsterLooting)
                _looting = "\n" + Translate.Get(Translate.Id.TipOffLootingOn);
            if (nManagerSetting.CurrentSetting.GatheringSearchRadius > 30)
                _radius = "\n" + Translate.Get(Translate.Id.TipOffRadiusLow);
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
                    string.Format("{0}\n{1}{2}{3}{4}", Translate.Get(Translate.Id.GrinderTipOffMessage), _looting,
                                  _radius, _useground, _usefly), Translate.Get(Translate.Id.GrinderTipOffTitle));
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