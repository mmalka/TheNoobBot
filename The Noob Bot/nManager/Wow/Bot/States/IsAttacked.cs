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
        private Thread _strikeFirstThread;
        private Spell _stealthSpell;
        private Spell _prowlSpell;

        public override string DisplayName
        {
            get { return "IsAttacked"; }
        }

        public static List<int> IgnoreStrikeBackCreatureList = new List<int>();

        public void StrikeFirst()
        {
            if (_stealthSpell == null)
                _stealthSpell = new Spell("Stealth");
            if (_prowlSpell == null)
                _prowlSpell = new Spell("Prowl");
            if (IgnoreStrikeBackCreatureList.Count <= 0)
            {
                Logging.Write("Loading IgnoreStrikeBackCreatureList...");
                string[] forceLootCreatureList = Others.ReadFileAllLines(Application.StartupPath + "\\Data\\IgnoreStrikeBackCreatureList.txt");
                for (int i = 0; i <= forceLootCreatureList.Length - 1; i++)
                {
                    int creatureId = Others.ToInt32(forceLootCreatureList[i]);
                    if (creatureId > 0 && !IgnoreStrikeBackCreatureList.Contains(creatureId))
                        IgnoreStrikeBackCreatureList.Add(creatureId);
                }
                if (IgnoreStrikeBackCreatureList.Count > 0)
                    Logging.Write("Loaded " + IgnoreStrikeBackCreatureList.Count + " creatures to ignore in Strike Back system.");
            }
            while (Products.Products.IsStarted)
            {
                Thread.Sleep(1500); // no need to spam, this is supposed to be more "human", and human have brainlag anyway.
                if (Fight.InFight)
                    continue;
                if (!Products.Products.IsStarted || !Usefuls.InGame || Usefuls.IsLoading || !ObjectManager.ObjectManager.Me.IsValid)
                    continue;
                if (ObjectManager.ObjectManager.Me.IsDeadMe || (ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying)))
                    continue;
                if (_stealthSpell.HaveBuff || _prowlSpell.HaveBuff)
                    continue;
                if (ObjectManager.ObjectManager.Me.HealthPercent <= 40)
                    continue;
                if (ObjectManager.ObjectManager.Me.GetDurability <= nManagerSetting.CurrentSetting.RepairWhenDurabilityIsUnderPercent)
                    continue;
                WoWUnit unit = ObjectManager.ObjectManager.GetUnitInAggroRange();
                if (unit == null || !unit.IsValid || unit.IsDead || nManagerSetting.IsBlackListedZone(unit.Position))
                    continue;
                if (IgnoreStrikeBackCreatureList.Contains(unit.Entry))
                    continue;
                if (unit.IsElite && System.Math.Abs(ObjectManager.ObjectManager.Me.Level - unit.Level) < 2 || System.Math.Abs(ObjectManager.ObjectManager.Me.Level - unit.Level) < -6)
                {
                    nManagerSetting.AddBlackListZone(unit.Position, 15f);
                    continue; // automatically add potentially dangerous target location to blacklist to avoid suiciding farming near an elite.
                }
                if (unit.Health > (ObjectManager.ObjectManager.Me.Health*15))
                    continue;
                if (TraceLine.TraceLineGo(ObjectManager.ObjectManager.Me.Position, unit.Position))
                    continue;
                if (unit.GetMove) // only decide to attack if the unit move towards us or patrol.
                    _unitToPull = unit;
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

                if (ObjectManager.ObjectManager.Me.InCombatBlizzard)
                {
                    _unit = ObjectManager.ObjectManager.GetNearestWoWUnit(ObjectManager.ObjectManager.GetHostileUnitNearPlayer());
                    if (_unit != null && _unit.IsValid && _unit.Attackable && _unit.GetDistance < 20)
                        return true;
                    // we are in combat blizzard and one hostile unit is fighting nearby, let's kill him.
                    // if it's evading etc, it will get blacklisted properly later anyway.
                    // this should fix bot being attacked from AoE by monster that does not target him.
                }

                if (!nManagerSetting.CurrentSetting.DontPullMonsters)
                {
                    if (_strikeFirstThread == null || !_strikeFirstThread.IsAlive)
                    {
                        _strikeFirstThread = new Thread(StrikeFirst);
                        _strikeFirstThread.Start();
                    }
                    _unit = _unitToPull;
                    if (_unit != null)
                    {
                        if (_unit.IsValid && _unit.IsAlive)
                        {
                            Logging.Write("Pulling " + _unit.Name);
                            return true;
                        }
                        _unitToPull = null;
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
            if (ObjectManager.ObjectManager.Me.IsMounted)
            {
                MovementManager.FindTarget(_unit, CombatClass.GetAggroRange);
                Thread.Sleep(100);
                if (MovementManager.InMovement)
                    return;
                MountTask.DismountMount();
            }
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