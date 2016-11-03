using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using nManager.Wow.Patchables;

namespace nManager.Wow.Class
{
    public class Spell
    {
        #region Fields

        public UInt32 Id;
        public List<uint> Ids = new List<uint>();
        public bool KnownSpell;
        public float CastTime;
        public int Cost;
        public string Icon = "";
        public bool IsFunnel;
        public bool SpellHasCharges;
        public float MaxRangeHostile;
        public float MinRangeHostile;
        public float MaxRangeFriend;
        public float MinRangeFriend;
        public string Name = "";
        public string NameInGame = "";
        public PowerType PowerType;
        public string Rank = "";
        public uint CategoryId;
        public uint StartRecoveryCategoryId;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Spell"/> class. This class management an spell of your wow player.
        /// </summary>
        /// <param name="spellId">The spell ID.</param>
        public Spell(uint spellId)
        {
            lock (this)
            {
                try
                {
                    Id = spellId;

                    SpellManager.SpellInfoLua spellInfo = SpellManager.GetSpellInfo(Id);
                    if (spellInfo.ID > 0 && spellId == spellInfo.ID)
                    {
                        CastTime = (float) spellInfo.CastTime/1000;
                        float MinRange = spellInfo.MinRange;
                        float MaxRange = spellInfo.MaxRange;

                        MaxRangeHostile = MaxRange;
                        MinRangeHostile = MinRange;
                        MaxRangeFriend = MaxRange;
                        MinRangeFriend = MinRange;

                        Name = SpellManager.SpellListManager.SpellNameById(spellId);
                        NameInGame = spellInfo.Name;
                        Cost = spellInfo.Cost;
                        Icon = spellInfo.Icon;
                        IsFunnel = spellInfo.IsFunnel;
                        PowerType = spellInfo.PowerType;
                        Rank = spellInfo.Rank;
                        if (MaxRangeHostile < 5.0f)
                            MaxRangeHostile = 5.0f;
                        if (MaxRangeFriend < 5.0f)
                            MaxRangeFriend = 5.0f;
                        KnownSpell = SpellManager.KnownSpell(Id);
                        SpellHasCharges = SpellManager.SpellHasCharges(Id);
                        Ids.Add(Id);
                        Ids.AddRange(SpellManager.SpellListManager.SpellIdByName(Name));
                        CategoryId = WoWSpellCategories.GetSpellCategoryBySpellId(Id);
                        StartRecoveryCategoryId = WoWSpellCategories.GetSpellStartRecoverCategoryBySpellId(Id);
                        return;
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell(uint spellId): " + exception);
                }
                CastTime = 0;
                MaxRangeHostile = 5.0f;
                MinRangeHostile = 0f;
                MaxRangeFriend = 5.0f;
                MinRangeFriend = 0f;
                NameInGame = "";
                CategoryId = 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Spell"/> class. This class manage spells of your wow player.
        /// </summary>
        /// <param name="spellName">English name of the spell.</param>
        public Spell(string spellName)
        {
            Spell tSpell = null;
            try
            {
                uint.TryParse(spellName, out Id);
                if (Id > 0)
                {
                    tSpell = new Spell(Id);
                }
                else
                {
                    foreach (Spell s in SpellManager.SpellBook())
                    {
                        if (s.Name.ToLower().Trim() != spellName.ToLower().Trim() && s.NameInGame.ToLower().Trim() != spellName.ToLower().Trim()) continue;
                        tSpell = s;
                        break;
                    }
                    if (tSpell == null)
                    {
                        tSpell = new Spell(SpellManager.SpellListManager.SpellIdByName(spellName).FirstOrDefault());
                        if (tSpell.Id == 0)
                            tSpell = null;
                    }
                }
                if (tSpell == null)
                {
                    Logging.WriteDebug("Spell(string spellName): spellName=" + spellName + " => Failed");
                    return;
                }
                string notEnglish = "";
                string haveBuff = "";
                if (tSpell.Name != tSpell.NameInGame)
                    notEnglish = "NameClient: " + tSpell.NameInGame + ", ";
                if (tSpell.HaveBuff)
                    haveBuff = ", HaveBuffOrPassive: " + tSpell.HaveBuff;

                Logging.WriteDebug("Spell(string spellName): Id: " + tSpell.Id + ", Name: " + tSpell.Name + ", " + notEnglish + "Known: " + tSpell.KnownSpell + haveBuff);
                Id = tSpell.Id;
                CastTime = tSpell.CastTime;
                Cost = tSpell.Cost;
                Icon = tSpell.Icon;
                IsFunnel = tSpell.IsFunnel;
                MaxRangeHostile = tSpell.MaxRangeHostile;
                MinRangeHostile = tSpell.MinRangeHostile;
                MaxRangeFriend = tSpell.MaxRangeFriend;
                MinRangeFriend = tSpell.MinRangeFriend;
                Name = tSpell.Name;
                NameInGame = tSpell.NameInGame;
                PowerType = tSpell.PowerType;
                Rank = tSpell.Rank;
                KnownSpell = tSpell.KnownSpell;
                SpellHasCharges = tSpell.SpellHasCharges;
                Ids.AddRange(tSpell.Ids);
                Ids.Add(Id);
                CategoryId = tSpell.CategoryId;
                StartRecoveryCategoryId = tSpell.StartRecoveryCategoryId;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Spell(string spellName): " + exception);
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Check if the spell is currently available for use.
        /// </summary>
        /// <value>
        /// Return <c>true</c> if spell usable; otherwise, <c>false</c>.
        /// </value>
        public bool IsSpellUsable
        {
            get
            {
                try
                {
                    return KnownSpell && (!SpellHasCharges || GetSpellCharges > 0) && SpellManager.IsSpellUsableLUA(this);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > IsSpellUsable: " + exception);
                }
                return false;
            }
        }

        /// <summary>
        /// Return the available SpellCharges using LUA.
        /// </summary>
        /// <value>
        /// Return <c>currentCharges</c>.
        /// </value>
        public int GetSpellCharges
        {
            get
            {
                string result = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(result + " = tostring(GetSpellCharges(" + Id + "))");
                return Others.ToInt32(Lua.GetLocalizedText(result));
            }
        }

        /// <summary>
        /// Return true if the player distance from casting a spell to a Hostile unit is good.
        /// Else, return false.
        /// </summary>
        public bool IsHostileDistanceGood
        {
            get
            {
                try
                {
                    return CombatClass.InSpellRange(ObjectManager.ObjectManager.Target, MinRangeHostile, MaxRangeHostile);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > IsHostileDistanceGood: " + exception);
                    return true;
                }
            }
        }

        /// <summary>
        /// Return true if the player distance from casting a spell to a friendly unit is good.
        /// Else, return false.
        /// </summary>
        public bool IsFriendDistanceGood
        {
            get
            {
                try
                {
                    return CombatClass.InSpellRange(ObjectManager.ObjectManager.Target, MinRangeFriend, MaxRangeFriend);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > IsFriendlyDistanceGood: " + exception);
                    return true;
                }
            }
        }

        /// <summary>
        /// Gets player buff Stack.
        /// </summary>
        public int BuffStack
        {
            get
            {
                try
                {
                    return ObjectManager.ObjectManager.Me.BuffStack(Ids);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > BuffStack: " + exception);
                }
                return -1;
            }
        }

        /// <summary>
        /// Gets if the nearest object named like your spell name is your.
        /// </summary>
        /// <value>
        /// return <c>true</c> if [summoned/created by me]; otherwise, <c>false</c>.
        /// </value>
        public bool CreatedBySpell
        {
            get
            {
                try
                {
                    List<WoWUnit> woWUnit = ObjectManager.ObjectManager.GetWoWUnitByName(NameInGame);

                    if (woWUnit.Count > 0)
                    {
                        WoWUnit nearestWoWUnit = ObjectManager.ObjectManager.GetNearestWoWUnit(woWUnit);
                        if (nearestWoWUnit.IsValid && nearestWoWUnit.IsAlive)
                        {
                            if (nearestWoWUnit.SummonedBy == ObjectManager.ObjectManager.Me.Guid ||
                                nearestWoWUnit.CreatedBy == ObjectManager.ObjectManager.Me.Guid)
                            {
                                return true;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Logging.WriteError("Spell > CreatedBySpell: " + e);
                }
                return false;
            }
        }

        /// <summary>
        /// Gets if the nearest object named like your spell name is your.
        /// Check for the max range prior to recast it.
        /// </summary>
        /// <value>
        /// return <c>true</c> if [summoned/created by me]; otherwise, <c>false</c>.
        /// </value>
        public bool CreatedBySpellInRange(uint maxrange = 40)
        {
            try
            {
                List<WoWUnit> woWUnit = ObjectManager.ObjectManager.GetWoWUnitByName(NameInGame);

                if (woWUnit.Count > 0)
                {
                    WoWUnit nearestWoWUnit = ObjectManager.ObjectManager.GetNearestWoWUnit(woWUnit, false, false, true);
                    if (nearestWoWUnit.IsValid && nearestWoWUnit.IsAlive)
                    {
                        if ((nearestWoWUnit.SummonedBy == ObjectManager.ObjectManager.Me.Guid ||
                             nearestWoWUnit.CreatedBy == ObjectManager.ObjectManager.Me.Guid) &&
                            nearestWoWUnit.GetDistance <= maxrange)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Spell > CreatedBySpellInRange: " + e);
            }
            return false;
        }

        /// <summary>
        /// Gets target buff Stack.
        /// </summary>
        public int TargetBuffStack
        {
            get
            {
                try
                {
                    return ObjectManager.ObjectManager.Target.BuffStack(Ids);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > TargetBuffStack: " + exception);
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets if your player have this spell in buff.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [have buff]; otherwise, <c>false</c>.
        /// </value>
        public bool HaveBuff
        {
            get
            {
                try
                {
                    return ObjectManager.ObjectManager.Me.HaveBuff(Ids);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > HaveBuff: " + exception);
                }
                return false;
            }
        }

        /// <summary>
        /// Gets if the target have this spell in buff.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [target have buff]; otherwise, <c>false</c>.
        /// </value>
        public bool TargetHaveBuff
        {
            get
            {
                try
                {
                    return ObjectManager.ObjectManager.Target.HaveBuff(Ids);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > TargetHaveBuff: " + exception);
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets if the target have this buff from me.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [target have buff]; otherwise, <c>false</c>.
        /// </value>
        public bool TargetHaveBuffFromMe
        {
            get
            {
                try
                {
                    Auras.UnitAura targetAura = ObjectManager.ObjectManager.Target.UnitAura(Ids, ObjectManager.ObjectManager.Me.Guid);
                    return targetAura.IsValid;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > TargetHaveBuffFromMe: " + exception);
                    return false;
                }
            }
        }

        /// <summary>
        /// Cast Spell.
        /// </summary>
        public void Launch()
        {
            try
            {
                Launch(CastTime != 0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Spell > Launch(): " + exception);
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
        }

        public void Cast()
        {
            Launch();
        }

        /// <summary>
        /// Cast Spell on specified target (blizz UnitID format: http://wowprogramming.com/docs/api_types#unitID).
        /// </summary>
        public void LaunchOnUnitID(string unitId)
        {
            try
            {
                Launch(CastTime != 0, true, false, unitId);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Spell > Launch(uintId): " + exception);
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
        }

        public void CastOnUnitID(string unitId)
        {
            LaunchOnUnitID(unitId);
        }

        /// <summary>
        /// Cast Spell on Self.
        /// </summary>
        public void LaunchOnSelf()
        {
            try
            {
                Launch(CastTime != 0, true, false, "player");
            }
            catch (Exception exception)
            {
                Logging.WriteError("Spell > LaunchOnSelf():" + exception);
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
        }

        public void CastOnSelf()
        {
            LaunchOnSelf();
        }

        /// <summary>
        /// Cast Spell on Self.
        /// </summary>
        /// <param name="StopMove">if set to <c>true</c> [Don't move during cast].</param>
        /// <param name="waitIsCast">if set to <c>true</c> [Wait the cast end].</param>
        /// <param name="ignoreIfCast"> </param>
        public void LaunchOnSelf(bool StopMove, bool waitIsCast = true, bool ignoreIfCast = false)
        {
            try
            {
                Launch(StopMove, waitIsCast, ignoreIfCast, "player");
            }
            catch (Exception exception)
            {
                Logging.WriteError("Spell > LaunchOnSelf(bool StopMove, bool waitIsCast = true, bool ignoreIfCast = false): " + exception);
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
        }

        public void CastOnSelf(bool StopMove, bool waitIsCast = true, bool ignoreIfCast = false)
        {
            LaunchOnSelf(StopMove, waitIsCast, ignoreIfCast);
        }


        /// <summary>
        /// Cast Spell.
        /// </summary>
        /// <param name="StopMove">if set to <c>true</c> [Don't move during cast].</param>
        /// <param name="waitIsCast">if set to <c>true</c> [Wait the cast end].</param>
        /// <param name="ignoreIfCast"> </param>
        /// <param name="unitId">if set, the spell is casted on this target.</param>
        public void Launch(bool StopMove, bool waitIsCast = true, bool ignoreIfCast = false, string unitId = null)
        {
            try
            {
                // We don't have extended check, we just need to know if we have LineOfSights in usual fight mode.
                if (ObjectManager.ObjectManager.Target.IsValid && (unitId == null || unitId == "target") &&
                    TraceLine.TraceLineGo(ObjectManager.ObjectManager.Target.Position, ObjectManager.ObjectManager.Me.Position, CGWorldFrameHitFlags.HitTestLOS))
                    return;
                Memory.WowMemory.GameFrameUnLock();
                if (StopMove)
                {
                    if (!nManagerSetting.CurrentSetting.ActivateMovementsDamageDealer && Products.Products.ProductName == "Damage Dealer")
                        StopMove = false;
                    if (!nManagerSetting.CurrentSetting.ActivateMovementsHealerBot && Products.Products.ProductName == "Heal Bot")
                        StopMove = false;
                }
                int t = 10;
                while (ObjectManager.ObjectManager.Me.IsCast && !ignoreIfCast)
                {
                    Thread.Sleep(100);
                    t--;
                    if (t < 0) return;
                }
                Logging.WriteFight("Cast " + NameInGame);
                if (StopMove)
                {
                    if (ObjectManager.ObjectManager.Me.GetMove)
                        MovementManager.StopMove();
                }
                if (unitId == null)
                    SpellManager.CastSpellByNameLUA(NameInGame);
                else
                    SpellManager.CastSpellByNameLUA(NameInGame, unitId);
                while (ObjectManager.ObjectManager.Me.IsCast && waitIsCast)
                {
                    Thread.Sleep(100);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError(
                    "Spell > Launch(bool StopMove, bool waitIsCast = true, bool ignoreIfCast = false): " + exception);
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
        }

        public void Cast(bool StopMove, bool waitIsCast = true, bool ignoreIfCast = false, string unitId = null)
        {
            Launch(StopMove, waitIsCast, ignoreIfCast, unitId);
        }

        public void CastAtPosition(Point position)
        {
            SpellManager.CastSpellByIDAndPosition(Id, position);
        }

        internal void Update()
        {
            try
            {
                KnownSpell = SpellManager.KnownSpell(Id);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Spell > Update(): " + exception);
            }
        }

        #endregion Methods
    }

    public class SpellList
    {
        public SpellList()
        {
        }

        public SpellList(uint id, string name = "")
        {
            Id = id;
            Name = name;
        }

        public uint Id { get; set; }
        public string Name { get; set; }
    }
}