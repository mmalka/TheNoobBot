using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Helpers;
using Point = nManager.Wow.Class.Point;

namespace nManager.Wow.Bot.States
{
    class StopBotIf : State
    {
        public override string DisplayName
        {
            get { return "StopBotIf"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;

        public override bool NeedToRun
        {
            get
            {
                if (!Products.Products.IsStarted)
                    return false;

                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting)
                    return false;

                return true;
            }
        }

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        private Point _lastPos;
        private uint _startedLevel;
        private int _startedTime;
        private bool _inPause;
        private Channel _whisperChannel = new Channel();
        private int _numberWhisper;
        public override void Run()
        {
            // If Bag Full
            if (nManagerSetting.CurrentSetting.StopTNBIfBagAreFull)
            {
               if (Usefuls.GetContainerNumFreeSlots <= 0 && Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
               {
                   Thread.Sleep(800);
                   if (Usefuls.GetContainerNumFreeSlots <= 0 && Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                   {
                       closeWow(Translate.Get(Translate.Id.Bag_is_full));
                       return;
                   }
               }
            }

            // If 4000 honor
            if (nManagerSetting.CurrentSetting.StopTNBIfHonorPointsLimitReached)
            {
                if (Usefuls.GetHonorPoint >= 4000 && Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                {
                    Thread.Sleep(800);
                    if (Usefuls.GetHonorPoint >= 4000 && Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                    {
                        closeWow(Translate.Get(Translate.Id.Reached_4000_Honor_Points));
                        return;
                    }
                }
            }

            // If player teleported
            if (nManagerSetting.CurrentSetting.StopTNBIfPlayerHaveBeenTeleported)
            {
                if (_lastPos == null && Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                    _lastPos = ObjectManager.ObjectManager.Me.Position;
                
                if (ObjectManager.ObjectManager.Me.Position.DistanceTo(_lastPos) >= 450 && !ObjectManager.ObjectManager.Me.IsDeadMe)
                {
                    closeWow(Translate.Get(Translate.Id.Player_Teleported));
                    return;
                }
                if (Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                    _lastPos = ObjectManager.ObjectManager.Me.Position;
            }

            // After X level
            if (_startedLevel == 0 && Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                _startedLevel = ObjectManager.ObjectManager.Me.Level;
            if ((int)(ObjectManager.ObjectManager.Me.Level - _startedLevel) >= nManagerSetting.CurrentSetting.StopTNBAfterXLevelup && Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
            {
                closeWow(Translate.Get(Translate.Id.Your_player_is_now_level) + " " + ObjectManager.ObjectManager.Me.Level + " (+" + (ObjectManager.ObjectManager.Me.Level - _startedLevel) + " " + Translate.Get(Translate.Id.level_upper) + ")");
                return;
            }

            // After X blockages
            if (Statistics.Stucks >= nManagerSetting.CurrentSetting.StopTNBAfterXStucks)
            {
                closeWow(Statistics.Stucks + " " + Translate.Get(Translate.Id.Blockages));
                return;
            }

            // After X min
            if (_startedTime == 0)
                _startedTime = Others.Times;
            if (_startedTime + (nManagerSetting.CurrentSetting.StopTNBAfterXMinutes * 60 * 1000) < Others.Times)
            {
                closeWow(Translate.Get(Translate.Id.tnb_started_since) + " " + nManagerSetting.CurrentSetting.StopTNBAfterXMinutes + " " + Translate.Get(Translate.Id.min));
                return;
            }

            // Pause bot if player near
            if (nManagerSetting.CurrentSetting.PauseTNBIfNearByPlayer && Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
            {
                if (!_inPause && !Products.Products.InPause)
                {
                    if (ObjectManager.ObjectManager.GetObjectWoWPlayer().Count >= 1 && Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                    {
                        _inPause = true;
                        Products.Products.InPause = true;
                        Logging.Write("Player Nerby, pause bot");
                    }
                }
                else if(_inPause)
                {
                    Thread.Sleep(800);
                    if (ObjectManager.ObjectManager.GetObjectWoWPlayer().Count <= 0 && Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                    {
                        _inPause = false;
                        Products.Products.InPause = false;
                        Logging.Write("No Player Nerby, unpause bot");
                    }
                }
            }


            // Channel
            while (_whisperChannel.ActuelRead < _whisperChannel.GetMsgActuelInWow)
            {
                var msg = _whisperChannel.ReadWhisperChannel();
                if (!String.IsNullOrWhiteSpace(msg))
                {
                    _numberWhisper++;
                    if (nManagerSetting.CurrentSetting.RecordWhispsInLogFiles)
                        Logging.Write(msg, Logging.LogType.Normal, Color.BlueViolet);
                    if (_numberWhisper >= nManagerSetting.CurrentSetting.StopTNBIfReceivedAtMostXWhispers)
                        closeWow(Translate.Get(Translate.Id.Whisper_Egal_at) + " " + _numberWhisper);
                    if (nManagerSetting.CurrentSetting.PlayASongIfNewWhispReceived)
                    {
                        var t = new Thread(ThreadSoundNewWhisper) {Name = "Sound alarm", IsBackground = true};
                        t.Start();

                        _msgNewWhisper = msg;
                        var t2 = new Thread(ThreadMessageBoxNewWhisper) { Name = "Messsage Box New Whisper" };
                        t2.Start();
                    }

                }
            }
        }

        private bool _threadSound;
        private string _msgNewWhisper;
        void ThreadSoundNewWhisper()
        {
            try
            {
                _threadSound = true;
                var myPlayer = new SoundPlayer
                {
                    SoundLocation = Application.StartupPath + "\\Data\\newWhisper.wav"
                };
                while (_threadSound)
                {
                    myPlayer.PlaySync();
                }
                myPlayer.Stop();
            }
            catch
            {
            }
        }
        void ThreadMessageBoxNewWhisper()
        {
            MessageBox.Show(Translate.Get(Translate.Id.New_whisper) + ": " + _msgNewWhisper, Translate.Get(Translate.Id.New_whisper), MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
            _threadSound = false;

        }

        void closeWow(string reason)
        {
            Logging.Write(reason);
            Memory.WowProcess.KillWowProcess();
            MessageBox.Show(reason, Translate.Get(Translate.Id.Stop_tnb_if), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Process.GetCurrentProcess().Kill();
        }
    }
}
