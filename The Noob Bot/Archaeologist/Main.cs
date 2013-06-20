using System;
using System.Windows.Forms;
using Archaeologist;
using Archaeologist.Bot;
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
                ArchaeologistSetting.Load();
                Logging.Status = "Initialize Archaeologist Complete";
                Logging.Write("Initialize Archaeologist Complete");
                GetProductTipOff();
            }
            catch (Exception e)
            {
                Logging.WriteError("Archaeologist > Main > Initialize(): " + e);
            }
        }

        public void Dispose()
        {
            try
            {
                Stop();
                Logging.Status = "Dispose Archaeologist Complete";
                Logging.Write("Dispose Archaeologist Complete");
            }
            catch (Exception e)
            {
                Logging.WriteError("Archaeologist > Main > Dispose(): " + e);
            }
        }

        public void Start()
        {
            try
            {
                if (Bot.Pulse())
                {
                    _isStarted = true;
                    Logging.Status = "Start Archaeologist Complete";
                    Logging.Write("Start Archaeologist Complete");
                }
                else
                {
                    Logging.Status = "Start Archaeologist failed";
                    Logging.Write("Start Archaeologist failed");
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Archaeologist > Main > Start(): " + e);
            }
        }

        public void Stop()
        {
            try
            {
                Bot.Dispose();
                _isStarted = false;
                Logging.Status = "Stop Archaeologist Complete";
                Logging.Write("Stop Archaeologist Complete");
            }
            catch (Exception e)
            {
                Logging.WriteError("Archaeologist > Main > Stop(): " + e);
            }
        }

        public void Settings()
        {
            try
            {
                DigSitesListManagement f = new DigSitesListManagement();
                f.ShowDialog();
                Logging.Status = "Settings Archaeologist Complete";
                Logging.Write("Settings Archaeologist Complete");
            }
            catch (Exception e)
            {
                Logging.WriteError("Archaeologist > Main > Settings(): " + e);
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