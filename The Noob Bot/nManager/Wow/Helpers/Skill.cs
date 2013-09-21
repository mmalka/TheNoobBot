using System;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.ObjectManager;
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
                                                     Descriptors.StartDescriptors);
                uint addressGD = descriptorsArray + ((uint) Descriptors.PlayerFields.Skill*Descriptors.Multiplicator);
                uint v2 = 0;
                uint id = 0;
                do
                {
                    short value = Memory.WowMemory.Memory.ReadShort(addressGD + v2);

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
                                                         Descriptors.StartDescriptors);
                    uint addressGD = descriptorsArray +
                                     ((uint) Descriptors.PlayerFields.Skill*Descriptors.Multiplicator);
                    short result = Memory.WowMemory.Memory.ReadShort(id*0x2 + addressGD + (uint) Addresses.Player.SkillValue);
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
                                                         Descriptors.StartDescriptors);
                    uint addressGD = descriptorsArray +
                                     ((uint) Descriptors.PlayerFields.Skill*Descriptors.Multiplicator);
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

        public static int GetSkillBonus(SkillLine skill)
        {
            int bonus = 0;

            // Guide if we want to complete: http://www.elsanglin.com/equipment.html

            // Racials bonuses
            switch (ObjectManager.ObjectManager.Me.WowRace)
            {
                case WoWRace.Gnome:
                    if (skill == SkillLine.Engineering)
                        bonus += 15;
                    break;
                case WoWRace.Tauren:
                    if (skill == SkillLine.Herbalism)
                        bonus += 15;
                    break;
                case WoWRace.Draenei:
                    if (skill == SkillLine.Jewelcrafting)
                        bonus += 5;
                    break;
                case WoWRace.BloodElf:
                    if (skill == SkillLine.Enchanting)
                        bonus += 10;
                    break;
                case WoWRace.Worgen:
                    if (skill == SkillLine.Skinning)
                        bonus += 15;
                    break;
                case WoWRace.Goblin:
                    if (skill == SkillLine.Alchemy)
                        bonus += 15;
                    break;
            }

            // Buffs bonuses
            if (skill == SkillLine.Fishing && ObjectManager.ObjectManager.Me.HaveBuff(45694))
                bonus += 10;

            // Enchantments bonuses, seems they wont stack with the basic tools

            // Items bonuses
            switch (skill)
            {
                case SkillLine.Mining:
                    if (ItemsManager.GetItemCount(40772) > 0 || ItemsManager.GetItemCount(2901) > 0)
                        bonus += 10;
                    break;
                case SkillLine.Herbalism:
                    if (ItemsManager.GetItemCount(40772) > 0 || ItemsManager.GetItemCount(85663) > 0)
                        bonus += 10;
                    break;
                case SkillLine.Skinning:
                    if (ItemsManager.GetItemCount(40772) > 0 || ItemsManager.GetItemCount(7005) > 0)
                        bonus += 10;
                    break;
                case SkillLine.Fishing:
                    // There is just too much things in Fishing to take in count, and it's not "that" useful for a bot...
                    // But I will need to add them later to display the skill increases correctly. See guide above.
                    break;
            }

            return bonus;
        }
    }
}