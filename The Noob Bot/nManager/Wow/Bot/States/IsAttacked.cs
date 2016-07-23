using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Bot.States
{
    public class IsAttacked : State
    {
        private WoWUnit _unit;
        private WoWUnit _unitToPull;
        private Thread _StrikeBackThread;

        public override string DisplayName
        {
            get { return "IsAttacked"; }
        }

        public void StrikeBack()
        {
            while (Products.Products.IsStarted)
            {
                _unitToPull = ObjectManager.ObjectManager.GetUnitInAggroRange();
                Thread.Sleep(100);
            }
        }

        public override int Priority { get; set; }

        private int _i = 0;

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame ||
                    Usefuls.IsLoading ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    (ObjectManager.ObjectManager.Me.IsMounted &&
                     (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying)) ||
                    !Products.Products.IsStarted)
                    return false;

                if (CustomProfile.GetSetIgnoreFight || Quest.GetSetIgnoreFight)
                    return false;

                /* RANDOM SECURITY CHECK */

                if (_i == 2500)
                {
                    // We are actually checking if script.php contains the get "hack" and if it's == "ruined".
                    // This random check here is a chance to protect us from crack using local server.
                    var l = Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cDovL3RlY2gudGhlbm9vYmJvdC5jb20vc2NyaXB0LnBocA=="));
                    var w = Encoding.UTF8.GetString(Convert.FromBase64String("aGFjaw=="));
                    string s = Others.GetRequest(l, w);
                    if (s != Encoding.UTF8.GetString(Convert.FromBase64String("cnVpbmVk")))
                    {
                        // I'll need to consider how bad is it to close the bot at the first fail, given the possibility 
                        // that the user simply has no internet for few seconds. Which is rare to happend considering the fact
                        // that we check only every 10000 times this function is called, which is arround 20minutes or so.
                        Application.Exit();
                    }
                }
                _i++;
                if (_i > 10000)
                {
                    if (File.Exists(Encoding.UTF8.GetString(Convert.FromBase64String("bk1hbmFnZXItY2xlYW5lZC5kbGw="))))
                    {
                        Application.Exit(); // checks nManager-cleaned.dll
                        // We don't want to check this too early else the developer would understand too fast.
                    }
                    _i = 0;
                }

                /* SECURITY */

                // Get if is attacked:
                _unit = null;

                if (ObjectManager.ObjectManager.GetNumberAttackPlayer() > 0)
                    _unit = ObjectManager.ObjectManager.GetNearestWoWUnit(ObjectManager.ObjectManager.GetHostileUnitAttackingPlayer());

                if (_unit != null && _unit.IsValid)
                    return true;

                if (!nManagerSetting.CurrentSetting.DontPullMonsters)
                {
                    if (_StrikeBackThread == null || !_StrikeBackThread.IsAlive)
                    {
                        _StrikeBackThread = new Thread(StrikeBack);
                        _StrikeBackThread.Start();
                    }
                    _unit = _unitToPull;
                    if (_unit != null)
                    {
                        Logging.Write("Pulling " + _unit.Name);
                        return true;
                    }
                }
                return false;
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

        public override void Run()
        {
            MountTask.DismountMount();
            Logging.Write("Player Attacked by " + _unit.Name + " (lvl " + _unit.Level + ")");
            UInt128 unkillableMob = Fight.StartFight(_unit.Guid);
            if (!_unit.IsDead && unkillableMob != 0 && _unit.HealthPercent == 100.0f)
            {
                Logging.Write("Blacklisting " + _unit.Name);
                nManagerSetting.AddBlackList(unkillableMob, 2*60*1000); // 2 minutes
            }
            else if (_unit.IsDead)
            {
                Statistics.Kills++;
                if (Products.Products.ProductName == "Quester" && (!_unit.IsTapped || (_unit.IsTapped && _unit.IsTappedByMe)))
                    Quest.KilledMobsToCount.Add(_unit.Entry); // we may update a quest requiring killing this unit

                if (ObjectManager.ObjectManager.GetNumberAttackPlayer() <= 0)
                    Thread.Sleep(Usefuls.Latency + 500);

                while (!ObjectManager.ObjectManager.Me.IsMounted && ObjectManager.ObjectManager.Me.InCombat && ObjectManager.ObjectManager.GetNumberAttackPlayer() <= 0)
                {
                    Thread.Sleep(150);
                }
                Fight.StopFight();
            }
        }
    }
}