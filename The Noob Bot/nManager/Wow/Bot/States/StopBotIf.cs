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
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Bot.States
{
    internal class StopBotIf : State
    {
        public override string DisplayName
        {
            get { return "StopBotIf"; }
        }

        public override int Priority { get; set; }

        public override bool NeedToRun
        {
            get
            {
                if (!Products.Products.IsStarted)
                    return false;

                if (Products.Products.InManualPause)
                {
                    _lastPos = null;
                    return false;
                }

                if (!Usefuls.InGame || Usefuls.IsLoadingOrConnecting)
                {
                    if (!_inPause && !Products.Products.InAutoPause)
                    {
                        Logging.Write("Game got disconnect or in loading, pausing TheNoobbot, please relog manually or make sure the relogger feature is activated.");
                        _inPause = true;
                        Products.Products.InAutoPause = true;
                        _gameOffline = true;
                    }
                    return false;
                }
                if (_gameOffline)
                {
                    _gameOffline = false;
                    ConfigWowForThisBot.ConfigWow();
                    if (Products.Products.ProductName == "Damage Dealer" && !nManagerSetting.CurrentSetting.ActivateMovementsDamageDealer)
                        ConfigWowForThisBot.StartStopClickToMove(false);
                    if (Products.Products.ProductName == "Heal Bot" && !nManagerSetting.CurrentSetting.ActivateMovementsHealerBot)
                        ConfigWowForThisBot.StartStopClickToMove(false);
                    SpellManager.UpdateSpellBook();
                    Logging.Write("Game is back online, unpausing, reloading SpellBook.");
                    _inPause = false;
                    Products.Products.InAutoPause = false;
                }

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
        private bool _gameOffline;
        private readonly Channel _whisperChannel = new Channel();
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

                if (ObjectManager.ObjectManager.Me.Position.DistanceTo(_lastPos) >= 450 &&
                    !ObjectManager.ObjectManager.Me.IsDeadMe)
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
            if ((int) (ObjectManager.ObjectManager.Me.Level - _startedLevel) >=
                nManagerSetting.CurrentSetting.StopTNBAfterXLevelup && Usefuls.InGame && !Usefuls.IsLoadingOrConnecting &&
                nManagerSetting.CurrentSetting.ActiveStopTNBAfterXLevelup)
            {
                closeWow(Translate.Get(Translate.Id.Your_player_is_now_level) + " " +
                         ObjectManager.ObjectManager.Me.Level + " (+" +
                         (ObjectManager.ObjectManager.Me.Level - _startedLevel) + " " +
                         Translate.Get(Translate.Id.level_upper) + ")");
                return;
            }

            // After X blockages
            if (Statistics.Stucks >= nManagerSetting.CurrentSetting.StopTNBAfterXStucks &&
                nManagerSetting.CurrentSetting.ActiveStopTNBAfterXStucks)
            {
                closeWow(Statistics.Stucks + " " + Translate.Get(Translate.Id.Blockages));
                return;
            }

            // After X min
            if (_startedTime == 0)
                _startedTime = Others.Times;
            if ((_startedTime + (nManagerSetting.CurrentSetting.StopTNBAfterXMinutes*60*1000) < Others.Times) &&
                nManagerSetting.CurrentSetting.ActiveStopTNBAfterXMinutes)
            {
                closeWow(Translate.Get(Translate.Id.tnb_started_since) + " " +
                         nManagerSetting.CurrentSetting.StopTNBAfterXMinutes + " " + Translate.Get(Translate.Id.min));
                return;
            }

            // Pause bot if player near
            if (nManagerSetting.CurrentSetting.PauseTNBIfNearByPlayer && Usefuls.InGame &&
                !Usefuls.IsLoadingOrConnecting)
            {
                if (!_inPause && !Products.Products.InAutoPause)
                {
                    if (ObjectManager.ObjectManager.GetObjectWoWPlayer().Count >= 1 && Usefuls.InGame &&
                        !Usefuls.IsLoadingOrConnecting)
                    {
                        _inPause = true;
                        Products.Products.InAutoPause = true;
                        Logging.Write("Player Nerby, pause bot");
                    }
                }
                else if (_inPause)
                {
                    Thread.Sleep(800);
                    if (ObjectManager.ObjectManager.GetObjectWoWPlayer().Count <= 0 && Usefuls.InGame &&
                        !Usefuls.IsLoadingOrConnecting)
                    {
                        _inPause = false;
                        Products.Products.InAutoPause = false;
                        Logging.Write("No Player Nerby, unpause bot");
                    }
                }
            }


            // Channel
            while (_whisperChannel.CurrentMsg < _whisperChannel.GetCurrentMsgInWow)
            {
                string msg = _whisperChannel.ReadWhisperChannel();
                if (!String.IsNullOrWhiteSpace(msg))
                {
                    _numberWhisper++;
                    if (nManagerSetting.CurrentSetting.RecordWhispsInLogFiles)
                        Logging.Write(msg, Logging.LogType.S, Color.BlueViolet);
                    if (_numberWhisper >= nManagerSetting.CurrentSetting.StopTNBIfReceivedAtMostXWhispers &&
                        nManagerSetting.CurrentSetting.ActiveStopTNBIfReceivedAtMostXWhispers)
                        closeWow(Translate.Get(Translate.Id.Whisper_Egal_at) + " " + _numberWhisper);
                    if (nManagerSetting.CurrentSetting.PlayASongIfNewWhispReceived)
                    {
                        Thread t = new Thread(ThreadSoundNewWhisper) {Name = "Sound alarm", IsBackground = true};
                        t.Start();

                        _msgNewWhisper = msg;
                        Thread t2 = new Thread(ThreadMessageBoxNewWhisper) {Name = "Messsage Box New Whisper"};
                        t2.Start();
                    }
                }
            }
        }

        private bool _threadSound;
        private string _msgNewWhisper;

        private void ThreadSoundNewWhisper()
        {
            try
            {
                _threadSound = true;
                SoundPlayer myPlayer = new SoundPlayer
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

        private void ThreadMessageBoxNewWhisper()
        {
            MessageBox.Show(Translate.Get(Translate.Id.New_whisper) + @": " + _msgNewWhisper,
                Translate.Get(Translate.Id.New_whisper), MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            _threadSound = false;
        }

        private void closeWow(string reason)
        {
            Logging.Write("Closing WoW because: " + reason);
            if (nManagerSetting.CurrentSetting.UseHearthstone)
            {
                Logging.Write("Loading Hearthstone informations");
                if (ItemsManager.GetItemCount(6948) <= 0)
                {
                    Logging.Write(Translate.Get(Translate.Id.HearthstoneNotFound));
                }
                else
                {
                    if (!ItemsManager.IsItemOnCooldown(6948) && ItemsManager.IsItemUsable(6948))
                    {
                        Timer timerHearthstone = new Timer(1000*45);
                        Tasks.MountTask.DismountMount();
                        MovementManager.StopMove();
                        MovementManager.StopMove();
                        timerHearthstone.Reset();
                        Logging.Write("Hearthstone available, using it.");
                        while (!Usefuls.IsLoadingOrConnecting && !timerHearthstone.IsReady)
                        {
                            ItemsManager.UseItem(ItemsManager.GetItemNameById(6948));
                            Thread.Sleep(1000);
                        }
                    }
                    else
                    {
                        Logging.Write("Hearthstone found but on cooldown.");
                    }
                }
            }

            Memory.WowProcess.KillWowProcess();
            MessageBox.Show(reason, Translate.Get(Translate.Id.Stop_tnb_if), MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            Process.GetCurrentProcess().Kill();
        }
    }
}