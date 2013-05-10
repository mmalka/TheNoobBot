using System;
using System.IO;
using System.Windows.Forms;
using Quester.Bot;
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
    private string _selling;
    private string _usefly;
    private string _useground;

    public void Initialize()
    {
        try
        {
            Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\Quester\\");
            Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\Quester\\Grouped\\");
            QuesterSettings.Load();
            Logging.Status = "Initialize Quester Complete";
            Logging.Write("Initialize Quester Complete");
            GetProductTipOff();
        }
        catch (Exception e)
        {
            Logging.WriteError("Quester > Main > Initialize(): " + e);
        }
    }

    public void Dispose()
    {
        try
        {
            Stop();
            Logging.Status = "Dispose Quester Complete";
            Logging.Write("Dispose Quester Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Quester > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            if (Bot.Pulse())
            {
                _isStarted = true;
                Logging.Status = "Start Quester Complete";
                Logging.Write("Start Quester Complete");
            }
            else
            {
                Logging.Status = "Start Quester failed";
                Logging.Write("Start Quester failed");
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("Quester > Main > Start(): " + e);
        }
    }

    public void Stop()
    {
        try
        {
            Bot.Dispose();
            _isStarted = false;
            Logging.Status = "Stop Quester Complete";
            Logging.Write("Stop Quester Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Quester > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            MessageBox.Show(Translate.Get(Translate.Id.No_setting_for_this_product));
            Logging.Status = "Settings Quester Complete";
            Logging.Write("Settings Quester Complete");
        }
        catch (Exception e)
        {
            Logging.WriteError("Quester > Main > Settings(): " + e);
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
            if (!nManagerSetting.CurrentSetting.ActivateMonsterLooting)
                _looting = "\n" + Translate.Get(Translate.Id.TipOffLootingOn);
            if (!nManagerSetting.CurrentSetting.ActivateAutoSellingFeature)
                _selling = "\n" + Translate.Get(Translate.Id.TipOffSellingOnQuester);
            if (nManagerSetting.CurrentSetting.GatheringSearchRadius < 100)
                _radius = "\n" + Translate.Get(Translate.Id.TipOffRadiusHigh);
            if (ObjectManager.Me.Level >= 20 &&
                ObjectManager.Me.Level < 60)
            {
                if (!nManagerSetting.CurrentSetting.UseGroundMount)
                    _useground = "\n" + Translate.Get(Translate.Id.TipOffUseGroundMountOn);
                else if (nManagerSetting.CurrentSetting.UseGroundMount &&
                         string.IsNullOrEmpty(nManagerSetting.CurrentSetting.GroundMountName))
                    _useground = "\n" + Translate.Get(Translate.Id.TipOffEmptyGroundMount);
            }
            else if (ObjectManager.Me.Level >= 60)
            {
                if (nManagerSetting.CurrentSetting.UseGroundMount)
                    _useground = "\n" + Translate.Get(Translate.Id.TipOffUseGroundMountOff);
                if (string.IsNullOrEmpty(nManagerSetting.CurrentSetting.FlyingMountName))
                    _usefly = "\n" + Translate.Get(Translate.Id.TipOffEmptyFlyingMount);
            }

            if (_looting != null || _radius != null || _selling != null || _usefly != null || _useground != null)
            {
                MessageBox.Show(
                    string.Format("{0}\n{1}{2}{3}{4}{5}", Translate.Get(Translate.Id.QuesterTipOffMessage), _selling, _looting,
                                  _radius, _useground, _usefly), Translate.Get(Translate.Id.QuesterTipOffTitle));
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("Quester > Main > GetProductTipOff(): " + e);
        }
    }

    #endregion
}