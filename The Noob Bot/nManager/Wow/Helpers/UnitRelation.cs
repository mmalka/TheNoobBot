using System;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Helpers
{
    public class UnitRelation
    {
        public static Reaction GetReaction(uint mobFaction)
        {
            try
            {
                return GetReaction(ObjectManager.ObjectManager.Me.Faction, mobFaction);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetReaction(uint mobFaction): " + exception);
            }
            return Reaction.Unknown;
        }

        public static Reaction GetReaction(uint localFaction, uint mobFaction)
        {
            try
            {
                if (mobFaction == 0 && (localFaction == 1 || localFaction == 2))
                    return Reaction.Neutral;
                WoWFactionTemplate t1 = WoWFactionTemplate.FromId(mobFaction);
                WoWFactionTemplate t2 = WoWFactionTemplate.FromId(localFaction);


                if (t1 == null || t2 == null)
                {
                    return Reaction.Unknown;
                }
                return t1.GetReactionTowards(t2);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetReaction(uint localFaction, uint mobFaction): " + exception);
            }
            return Reaction.Unknown;
        }

        public static Reaction GetReaction(WoWObject localObj, WoWObject mobObj)
        {
            try
            {
                return GetReaction(new WoWUnit(localObj.GetBaseAddress).Faction,
                                   new WoWUnit(mobObj.GetBaseAddress).Faction);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetReaction(WoWObject localObj, WoWObject mobObj): " + exception);
                return Reaction.Unknown;
            }
        }
    }
}