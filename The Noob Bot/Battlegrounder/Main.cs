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
            BattlegrounderSetting.Load();
            Logging.Status = "Initialize Battlegrounder Complete";
            Logging.Write("Initialize Battlegrounder Complete");
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
            Logging.Status = "Dispose Battlegrounder Complete";
            Logging.Write("Dispose Battlegrounder Complete");
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
            if (Bot.Pulse())
            {
                _isStarted = true;
                Logging.Status = "Start Battlegrounder Complete";
                Logging.Write("Start Battlegrounder Complete");
            }
            else
            {
                Logging.Status = "Start Battlegrounder failed";
                Logging.Write("Start Battlegrounder failed");
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
            Logging.Status = "Stop Battlegrounder Complete";
            Logging.Write("Stop Battlegrounder Complete");
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
            var f = new SettingsBattlegrounderForm();
            f.ShowDialog();
            Logging.Status = "Battlegrounder Settings Complete";
            Logging.Write("Battlegrounder Settings Complete");
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