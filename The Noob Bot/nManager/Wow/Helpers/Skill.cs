using System;
using nManager.Helpful;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class Skill
    {
        private static uint GetBaseSkill(Enums.SkillLine skill)
        {
            try
            {
                uint descriptorsArray =
                    Memory.WowMemory.Memory.ReadUInt(ObjectManager.ObjectManager.Me.GetBaseAddress +
                                                     Descriptors.startDescriptors);
                uint addressGD = descriptorsArray + ((uint) Descriptors.PlayerFields.Skill*Descriptors.multiplicator);
                uint v2 = 0;
                uint id = 0;
                do
                {
                    var value = Memory.WowMemory.Memory.ReadShort(addressGD + v2);

                    if (value == (int) skill)
                    {
                        break;
                    }
                    ++id;
                    v2 += 2;
                } while (v2 <= 0x200);

                return id;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetBaseSkill(Enums.SkillLine skill): " + exception);
            }
            return 0;
        }

        public static int GetValue(Enums.SkillLine skill)
        {
            try
            {
                uint id = GetBaseSkill(skill);

                if (id > 0)
                {
                    uint descriptorsArray =
                        Memory.WowMemory.Memory.ReadUInt(ObjectManager.ObjectManager.Me.GetBaseAddress +
                                                         Descriptors.startDescriptors);
                    uint addressGD = descriptorsArray +
                                     ((uint) Descriptors.PlayerFields.Skill*Descriptors.multiplicator);
                    var result = Memory.WowMemory.Memory.ReadShort(id*0x2 + addressGD + (uint) Addresses.Player.SkillValue);
                    return result;
                }
                return 0;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetValue(Enums.SkillLine skill): " + exception);
            }
            return 0;
        }

        public static int GetMaxValue(Enums.SkillLine skill)
        {
            try
            {
                uint id = GetBaseSkill(skill);

                if (id > 0)
                {
                    uint descriptorsArray =
                        Memory.WowMemory.Memory.ReadUInt(ObjectManager.ObjectManager.Me.GetBaseAddress +
                                                         Descriptors.startDescriptors);
                    uint addressGD = descriptorsArray +
                                     ((uint) Descriptors.PlayerFields.Skill*Descriptors.multiplicator);
                    return Memory.WowMemory.Memory.ReadShort(id*0x2 + addressGD + (uint) Addresses.Player.SkillMaxValue);
                }
                return 0;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetMaxValue(Enums.SkillLine skill): " + exception);
            }
            return 0;
        }
    }
}