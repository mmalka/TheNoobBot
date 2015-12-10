using System;
using System.Collections.Generic;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public static class Party
    {
        public static UInt128 GetPartyLeaderGUID()
        {
            try
            {
                uint party = GetPartyPointer(ObjectManager.ObjectManager.Me.GetCurrentPartyType);
                if (party > 0)
                {
                    uint numGroupMembers = GetPartyNumberPlayers();

                    for (uint i = 0; i < numGroupMembers; i++)
                    {
                        uint partyPlayer = Memory.WowMemory.Memory.ReadUInt(party + 4*i);

                        if (partyPlayer <= 0 || Memory.WowMemory.Memory.ReadUInt(partyPlayer + 4) != 2)
                            continue;
                        return Memory.WowMemory.Memory.ReadUInt128(partyPlayer + (uint) Addresses.Party.PlayerGuid);
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

        public static List<UInt128> GetPartyPlayersGUID()
        {
            List<UInt128> partyPlayersGUID = new List<UInt128>();
            try
            {
                uint party = GetPartyPointer(ObjectManager.ObjectManager.Me.GetCurrentPartyType);
                if (party > 0)
                {
                    //uint numGroupMembers = GetPartyNumberPlayers();
                    for (uint i = 0; i < 40; i++)
                    {
                        uint partyPlayer = Memory.WowMemory.Memory.ReadUInt(party + 4*i);
                        if (partyPlayer <= 0) continue;
                        UInt128 currentPlayerGUID = Memory.WowMemory.Memory.ReadUInt128(partyPlayer + (uint) Addresses.Party.PlayerGuid);
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
                partyPlayersGUID = new List<UInt128>();
            }
            return partyPlayersGUID;
        }

        public static uint GetPartyNumberPlayers()
        {
            try
            {
                uint party = GetPartyPointer(ObjectManager.ObjectManager.Me.GetCurrentPartyType);
                if (party > 0)
                {
                    uint nbr = Memory.WowMemory.Memory.ReadUInt(party + (uint) Addresses.Party.NumOfPlayers);
                    if (nbr > 0 && nbr <= 40)
                        return nbr;
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Party > GetPartyNumberPlayers(): " + e);
            }
            return 0;
        }

        public static uint GetPartyNumberPlayersLUA(PartyEnums.PartyType partyType = PartyEnums.PartyType.LE_PARTY_CATEGORY_HOME)
        {
            try
            {
                string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomStringResult + " = GetNumGroupMembers(\"" + partyType + "\");");
                string sResult = Lua.GetLocalizedText(randomStringResult);
                return Others.ToUInt32(sResult);
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
                return ObjectManager.ObjectManager.Me.GetCurrentPartyType == PartyEnums.PartyType.LE_PARTY_CATEGORY_HOME
                    ? ObjectManager.ObjectManager.Me.IsHomePartyLeader
                    : ObjectManager.ObjectManager.Me.IsInstancePartyLeader;
            }
            catch (Exception e)
            {
                Logging.WriteError("Party > CurrentPlayerIsLeader(): " + e);
            }
            return false;
        }

        public static bool IsInGroup()
        {
            try
            {
                return ObjectManager.ObjectManager.Me.GetCurrentPartyType != PartyEnums.PartyType.None;
            }
            catch (Exception e)
            {
                Logging.WriteError("Party > IsInGroup(): " + e);
            }
            return false;
        }

        public static bool IsInGroupLUA(PartyEnums.PartyType partyType = PartyEnums.PartyType.LE_PARTY_CATEGORY_HOME)
        {
            try
            {
                string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                string randomStringResult2 = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomStringResult2 + " = IsInGroup(\"" + partyType + "\"); if " + randomStringResult2 + " then " + randomStringResult + " = \"1\" else " + randomStringResult + " = \"0\" end");
                string sResult = Lua.GetLocalizedText(randomStringResult);
                return sResult == "1";
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetSpellInfoLUA(string spellNameInGame): " + exception);
            }
            return false;
        }

        public static UInt32 GetPartyPointer(PartyEnums.PartyType partyType = PartyEnums.PartyType.LE_PARTY_CATEGORY_HOME)
        {
            try
            {
                if (partyType == PartyEnums.PartyType.None)
                    return 0;
                return
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule +
                                                     (uint)
                                                         ((PartyEnums.PartyType) (uint) Addresses.Party.PartyOffset +
                                                          (partyType - PartyEnums.PartyType.LE_PARTY_CATEGORY_HOME)*(int) (PartyEnums.PartyType) 4));
            }
            catch (Exception e)
            {
                Logging.WriteError("Party > GetPartyPointer(): " + e);
            }
            return 0;
        }
    }
}