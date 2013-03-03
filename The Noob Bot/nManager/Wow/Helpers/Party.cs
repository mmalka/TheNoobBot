using System;
using System.Collections.Generic;
using nManager.Helpful;
using nManager.Wow.Enums;
using PartyType = nManager.Wow.Enums.PartyEnums.PartyType;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public static class Party
    {
        public static ulong GetPartyLeaderGUID()
        {
            try
            {
                uint party = GetPartyPointer(ObjectManager.ObjectManager.Me.GetCurrentPartyType);
                if (party > 0)
                {
                    uint numGroupMembers = GetPartyNumberPlayers();

                    for (uint i = 0; i < numGroupMembers; i++)
                    {
                        uint partyPlayer = Memory.WowMemory.Memory.ReadUInt(party + 4*i, false);

                        if (partyPlayer <= 0 || Memory.WowMemory.Memory.ReadUInt(partyPlayer + 4, false) != 2)
                            continue;
                        return Memory.WowMemory.Memory.ReadUInt64(partyPlayer + (uint) Addresses.Party.PlayerGuid,
                                                                  false);
                    }
                }
            }
            catch (Exception arg)
            {
                Logging.WriteError(
                    "Party > GetPartyLeaderGUID(): " +
                    arg);
            }

            return 0;
        }

        public static List<UInt64> GetPartyPlayersGUID()
        {
            var partyPlayersGUID = new List<UInt64>();
            try
            {
                uint party = GetPartyPointer(ObjectManager.ObjectManager.Me.GetCurrentPartyType);
                if (party > 0)
                {
                    uint numGroupMembers = GetPartyNumberPlayers();
                    for (uint i = 0; i < numGroupMembers; i++)
                    {
                        uint partyPlayer = Memory.WowMemory.Memory.ReadUInt(party + 4*i, false);
                        if (partyPlayer <= 0) continue;
                        ulong currentPlayerGUID =
                            Memory.WowMemory.Memory.ReadUInt64(
                                partyPlayer + (uint) Addresses.Party.PlayerGuid, false);
                        if (currentPlayerGUID > 0)
                        {
                            partyPlayersGUID.Add(currentPlayerGUID);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Party > GetPartyGUID(): " + e);
                partyPlayersGUID = new List<UInt64>();
            }
            return partyPlayersGUID;
        }

        public static uint GetPartyNumberPlayers()
        {
            try
            {
                uint party = GetPartyPointer(ObjectManager.ObjectManager.Me.GetCurrentPartyType);
                return party > 0
                           ? Memory.WowMemory.Memory.ReadUInt(party + (uint) Addresses.Party.NumOfPlayers, false)
                           : 0;
            }
            catch (Exception e)
            {
                Logging.WriteError("Party > GetPartyNumberPlayers(): " + e);
            }
            return 0;
        }

        public static bool CurrentPlayerIsLeader()
        {
            try
            {
                return ObjectManager.ObjectManager.Me.GetCurrentPartyType == PartyType.Home
                           ? ObjectManager.ObjectManager.Me.IsHomePartyLeader
                           : ObjectManager.ObjectManager.Me.IsInstancePartyLeader;
            }
            catch (Exception e)
            {
                Logging.WriteError("Party > CurrentPlayerIsLeader(): " + e);
            }
            return false;
        }

        public static UInt32 GetPartyPointer(PartyType partyType = PartyType.Home)
        {
            try
            {
                if (partyType == PartyType.None || !IsInGroup())
                    return 0;
                return Memory.WowMemory.Memory.ReadUInt((uint)((PartyType)(uint)Addresses.Party.PartyOffset + (partyType - PartyType.Home) * (int)(PartyType)4), true);
                }
            catch (Exception e)
            {
                Logging.WriteError("Party > GetPartyPointer(): " + e);
            }
            return 0;
        }

        public static bool IsInGroup()
        {
            try
            {
                return Memory.WowMemory.Memory.ReadUInt((uint) Addresses.Party.PartyOffset, true) > 0;
            }
            catch (Exception e)
            {
                Logging.WriteError("Party > IsInGroup(): " + e);
            }
            return false;
        }
    }
}