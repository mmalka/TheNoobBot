using System;
using System.Linq;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Patchables;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using Quaternion = nManager.Wow.Class.Quaternion;

namespace nManager.Wow.ObjectManager
{
    public class WoWAreaTrigger : WoWObject
    {
        public WoWAreaTrigger(uint address)
            : base(address)
        {
        }

        public new bool IsValid
        {
            get
            {
                try
                {
                    return SpellID > 0 /*&& Radius > 0*/&& base.IsValid;
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > IsValid: " + e);
                    return false;
                }
            }
        }

        public UInt128 Caster
        {
            get
            {
                try
                {
                    return GetDescriptor<UInt128>(Descriptors.AreaTriggerFields.Caster);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > Caster: " + e);
                }
                return 0;
            }
        }

        public uint Duration
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.AreaTriggerFields.Duration); // or base +120
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > Duration: " + e);
                }
                return 0;
            }
        }

        public uint TimeToTarget
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.AreaTriggerFields.TimeToTarget);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > TimeToTarget: " + e);
                }
                return 0;
            }
        }

        public uint TimeToTargetScale
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.AreaTriggerFields.TimeToTargetScale);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > TimeToTargetScale: " + e);
                }
                return 0;
            }
        }

        public uint TimeToTargetExtraScale
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.AreaTriggerFields.TimeToTargetExtraScale);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > TimeToTargetExtraScale: " + e);
                }
                return 0;
            }
        }

        private Spell _spell;

        public Spell Spell
        {
            get
            {
                if (_spell != null)
                    return _spell;
                if (Entry > 0)
                {
                    _spell = new Spell((uint) Entry);
                    return _spell;
                }
                return new Spell(0);
            }
        }

        public uint SpellID
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadUInt(BaseAddress + 136);
                    return GetDescriptor<uint>(Descriptors.AreaTriggerFields.SpellID); // or base+136
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > SpellID: " + e);
                }
                return 0;
            }
        }

        public uint SpellXSpellVisualID
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.AreaTriggerFields.SpellXSpellVisualID);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > SpellVisualID: " + e);
                }
                return 0;
            }
        }

        public float[] BoundsRadius2D
        {
            get
            {
                try
                {
                    return new[] {GetDescriptor<float>(Descriptors.AreaTriggerFields.BoundsRadius2D), GetDescriptor<float>(Descriptors.AreaTriggerFields.BoundsRadius2D + 4)};
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > Radius: " + e);
                }
                return new[] {0f, 0f};
            }
        }

        public uint DecalPropertiesID
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.AreaTriggerFields.DecalPropertiesID);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > DecalPropertiesID: " + e);
                }
                return 0;
            }
        }

        private uint _faction;

        public uint Faction
        {
            get
            {
                try
                {
                    if (_faction == 0)
                    {
                        WoWObject objCaster = ObjectManager.GetObjectByGuid(Caster);
                        if (objCaster.Type == WoWObjectType.Unit && objCaster is WoWUnit)
                            _faction = (objCaster as WoWUnit).Faction;
                        if (objCaster.Type == WoWObjectType.GameObject && objCaster is WoWGameObject)
                            _faction = (objCaster as WoWGameObject).Faction;
                    }
                    return _faction;
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > Faction: " + e);
                    return 0;
                }
            }
        }

        public override Point Position
        {
            get
            {
                try
                {
                    return
                        new Point(
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress +
                                                              (uint) Addresses.GameObject.GAMEOBJECT_FIELD_X),
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress +
                                                              (uint) Addresses.GameObject.GAMEOBJECT_FIELD_Y),
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress +
                                                              (uint) Addresses.GameObject.GAMEOBJECT_FIELD_Z));
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > Position: " + e);
                }
                return new Point();
            }
        }

        public float Rotation
        {
            get
            {
                try
                {
                    Memory.WowMemory.Memory.ReadFloat(BaseAddress + (uint) Addresses.GameObject.GAMEOBJECT_FIELD_R);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > Rotation: " + e);
                }
                return 0f;
            }
        }

        public override string Name
        {
            get
            {
                try
                {
                    return Spell.NameInGame;
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > Name: " + e);
                }
                return "";
            }
        }

        public T GetDescriptor<T>(Descriptors.AreaTriggerFields field) where T : struct
        {
            try
            {
                return GetDescriptor<T>((uint) field);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetDescriptor<T>(Descriptors.AreaTriggerFields field): " + e);
                return default(T);
            }
        }

        public override string ToString()
        {
            string retString = String.Format("AreaTrigger: {0}BaseAddress: {1}, {0}Caster: {2}, {0}OverrideScaleCurve: {3}, {0}ExtraScaleCurve: {4}, {0}Duration: {5}, {0}TimeToTarget: {6}, {0}" +
                                             "{0}TimeToTargetScale: {7}, {0}TimeToTargetExtraScale: {8}, {0}SpellID: {9}, {0}SpellVisualID: {10}, {0}BoundsRadius2D: {11}, {0}Faction: {12}, {0}Name: {13}, {0}Entry: {14}",
                Environment.NewLine, BaseAddress, Caster, "", "", Duration, TimeToTarget, TimeToTargetScale, TimeToTargetExtraScale, SpellID, SpellXSpellVisualID, BoundsRadius2D
                , Faction, Name, Entry);
            return retString;
        }
    }
}