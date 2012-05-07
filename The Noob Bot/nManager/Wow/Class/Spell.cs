using System;
using System.Collections.Generic;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Helpers;
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
        public string IsFunnel = "";
        public float MaxRange;
        public float MinRange;
        public string Name = "";
        public string NameInGame = "";
        public string PowerType = "";
        public string Rank = "";

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

                    var dbclient = DBCReading.GetWoWClientDBByAddress((uint)Addresses.DBC.spell);
                    var baseAdressSpell = DBCReading.GetAddressByIndex((int)spellId, dbclient);

                    if (baseAdressSpell > 0)
                    {
                        if (spellId == Memory.WowMemory.Memory.ReadUInt(baseAdressSpell))
                        {
                            var castTimeIndex = Memory.WowMemory.Memory.ReadUInt(baseAdressSpell + 0x30);
                            var dbclientCastTime =
                                DBCReading.GetWoWClientDBByAddress((uint)Addresses.DBC.SpellCastTimes);
                            var baseCastTime = DBCReading.GetAddressByIndex((int)castTimeIndex,
                                                                                    dbclientCastTime);
                            try
                            {

                                CastTime = Convert.ToSingle(Memory.WowMemory.Memory.ReadInt(baseCastTime + 0x4)) /
                                           1000;
                            }
                            catch
                            {
                                CastTime = 0;
                            }

                            var rangeIndex = Memory.WowMemory.Memory.ReadUInt(baseAdressSpell + 0x3C);
                            var dbclientRange =
                                DBCReading.GetWoWClientDBByAddress((uint)Addresses.DBC.SpellRange);
                            var baseRange = DBCReading.GetAddressByIndex((int)rangeIndex, dbclientRange);

                            try
                            {
                                var tMaxRange = Memory.WowMemory.Memory.ReadFloat(baseRange + 0x10);
                                MaxRange = Memory.WowMemory.Memory.ReadFloat(baseRange + 0xC);
                                if (tMaxRange > MaxRange)
                                    MaxRange = tMaxRange;
                            }
                            catch
                            {
                                MaxRange = 0;
                            }
                            try
                            {
                                var tMinRange = Memory.WowMemory.Memory.ReadFloat(baseRange + 0x8);
                                MinRange = Memory.WowMemory.Memory.ReadFloat(baseRange + 0x4);
                                if (tMinRange > MinRange)
                                    MinRange = tMinRange;
                            }
                            catch
                            {
                                MinRange = 0;
                            }
                            Name = SpellManager.SpellListManager.SpellNameById(spellId);
                            try
                            {
                                NameInGame =
                                    Memory.WowMemory.Memory.ReadUTF8String(
                                        Memory.WowMemory.Memory.ReadUInt(baseAdressSpell + 84));
                                // Script_GetSpellInfo
                            }
                            catch
                            {
                                NameInGame = "";
                            }

                            if (MaxRange < 3.6f)
                                MaxRange = 3.6f;
                            KnownSpell = SpellManager.ExistSpellBookLUA(NameInGame);
                            Ids.AddRange(SpellManager.SpellListManager.SpellIdByName(Name));
                            Ids.Add(Id);
                            return;
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell(uint spellId): " + exception);
                }
                CastTime = 0;
                MaxRange = 3.6f;
                MinRange = 0;
                NameInGame = "";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Spell"/> class. This class management an spell of your wow player.
        /// </summary>
        /// <param name="spellName">Name of the spell.</param>
        public Spell(string spellName)
        {
            Spell tSpell = null;
            try
            {
                uint.TryParse(spellName, out Id);
                if (Id > 0)
                {
                    tSpell = SpellManager.GetSpellInfoLUA(spellName);
                    if (tSpell == null)
                    {
                        tSpell = new Spell(SpellManager.SpellListManager.SpellIdByName(spellName)[0]); // en
                    }
                }
                else
                {
                    foreach (Spell s in SpellManager.SpellBook())
                    {
                        if (s.Name == spellName)
                        {
                            tSpell = s; // Check à partir du nom en anglais, version string de Spell
                            break; // On sort du foreach si on a un résultat.
                        }
                    }
                }
                //Logging.WriteDebug("Spell(string spellName): spellName=" + spellName + " Id found: " + tSpell.Id + " Name found: " + tSpell.Name + " NameInGame found: " + tSpell.NameInGame);
                if (tSpell == null)
                {
                    Logging.WriteDebug("Spell(string spellName): spellName=" + spellName + " => Failed");
                    return;
                }
                else
                    Logging.WriteDebug("Spell(string spellName): spellName=" + spellName + ", Id found: " + tSpell.Id + ", Name found: " + tSpell.Name + ", NameInGame found: " + tSpell.NameInGame);
                Id = tSpell.Id;
                CastTime = tSpell.CastTime;
                Cost = tSpell.Cost;
                Icon = tSpell.Icon;
                IsFunnel = tSpell.IsFunnel;
                MaxRange = tSpell.MaxRange;
                MinRange = tSpell.MinRange;
                Name = tSpell.Name;
                NameInGame = tSpell.NameInGame;
                PowerType = tSpell.PowerType;
                Rank = tSpell.Rank;
                if (MaxRange < 3.6f)
                    MaxRange = 3.6f;
                KnownSpell = SpellManager.ExistSpellBookLUA(NameInGame);
                Ids.AddRange(SpellManager.SpellListManager.SpellIdByName(Name)); // check de tout le fichier Spell.txt
                Ids.Add(Id);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Spell(string spellName): " + exception);
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets if the player can use this spell.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if spell usable; otherwise, <c>false</c>.
        /// </value>
        public bool IsSpellUsable
        {
            get
            {
                try { return SpellManager.SpellUsableLUA(NameInGame); }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > IsSpellUsable: " + exception);
                }
                return false;
            }
        }

        /// <summary>
        /// Gets if the player distance to cast spell is good.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the distance good; otherwise, <c>false</c>.
        /// </value>
        public bool IsDistanceGood
        {
            get
            {
                try
                {
                    if (MaxRange < 4.5f)
                        MaxRange = 4.5f;

                    if (ObjectManager.ObjectManager.Target.GetDistance <= MaxRange && (ObjectManager.ObjectManager.Target.GetDistance >= MinRange))
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > IsDistanceGood: " + exception);
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
                { return ObjectManager.ObjectManager.Me.BuffStack(Ids); }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > BuffStack: " + exception);
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets target buff Stack.
        /// </summary>
        public int TargetBuffStack
        {
            get
            {
                try
                { return ObjectManager.ObjectManager.Target.BuffStack(Ids); }
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
                { return ObjectManager.ObjectManager.Me.HaveBuff(Ids); }
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
        /// Cast Spell.
        /// </summary>
        public void Launch()
        {
            try
            {
                Launch(CastTime == 0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Spell > Launch(): " + exception);
            }
        }

        /// <summary>
        /// Cast Spell.
        /// </summary>
        /// <param name="dontStopMove">if set to <c>true</c> [Don't move during cast].</param>
        /// <param name="waitIsCast">if set to <c>true</c> [Wait the cast end].</param>
        /// <param name="ignoreIfCast"> </param>
        public void Launch(bool dontStopMove, bool waitIsCast = true, bool ignoreIfCast = false)
        {
            try
            {
                int t = 200;
                while (ObjectManager.ObjectManager.Me.IsCast && !ignoreIfCast)
                {
                    Thread.Sleep(5);
                    t--;
                    if (t < 0) return;
                }
                Logging.WriteFight("Cast " + NameInGame);
                if (!dontStopMove)
                {
                    if (ObjectManager.ObjectManager.Me.GetMove)
                        MovementManager.StopMoveTo();
                }
                SpellManager.CastSpellByNameLUA(NameInGame);
                Thread.Sleep(600);
                while (ObjectManager.ObjectManager.Me.IsCast && waitIsCast)
                {
                    Thread.Sleep(5);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Spell > Launch(bool dontStopMove, bool waitIsCast = true, bool ignoreIfCast = false): " + exception);
            }
        }

        internal void Update()
        {
            try
            {
                KnownSpell = SpellManager.ExistSpellBookLUA(NameInGame);
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
