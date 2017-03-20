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
    private string _sellingQuality;
    private string _usefly;
    private string _useground;

    public void Initialize()
    {
        try
        {
            Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\Quester\\");
            Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\Quester\\Grouped\\");
            QuesterSettings.Load();
            Others.ProductStatusLog(Products.ProductName, 1);
            if (nManagerSetting.ActivateProductTipOff)
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
            Others.ProductStatusLog(Products.ProductName, 2);
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
            Logging.WriteError("Quester > Main > Start(): " + e);
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
            Bot.Dispose();
            _isStarted = false;
            Others.ProductStatusLog(Products.ProductName, 6);
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
            QuesterSettings.CurrentSettings.ToForm();
            QuesterSettings.CurrentSettings.Save();
            Others.ProductStatusLog(Products.ProductName, 7);
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
            {
                _selling = "\n" + Translate.Get(Translate.Id.TipOffSellingOnQuester);
                _sellingQuality = "\n" + Translate.Get(Translate.Id.TipOffSellingQualityQuester);
            }
            if (nManagerSetting.CurrentSetting.GatheringSearchRadius < 70)
                _radius = "\n" + Translate.Get(Translate.Id.TipOffRadiusHigh);
            if (!nManagerSetting.CurrentSetting.UseGroundMount)
                _useground = "\n" + Translate.Get(Translate.Id.TipOffUseGroundMountOn);
            else if (nManagerSetting.CurrentSetting.UseGroundMount && string.IsNullOrEmpty(nManagerSetting.CurrentSetting.GroundMountName))
                _useground = "\n" + Translate.Get(Translate.Id.TipOffEmptyGroundMount);
            if (_looting != null || _radius != null || _selling != null || _usefly != null || _useground != null)
            {
                MessageBox.Show(
                    string.Format("{0}\n{1}{2}{3}{4}{5}{6}", Translate.Get(Translate.Id.QuesterTipOffMessage), _selling, _sellingQuality, _looting,
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