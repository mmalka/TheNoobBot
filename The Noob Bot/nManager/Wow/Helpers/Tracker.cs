using System;
using System.Collections.Generic;
using System.Linq;
using nManager.Helpful;
using nManager.Wow.Enums;

namespace nManager.Wow.Helpers
{
    public class Tracker
    {
        public static void TrackObjectFlags(List<string> listTrackObjectFlags)
        {
            try
            {
                TrackObjectFlags none = listTrackObjectFlags.Aggregate(Enums.TrackObjectFlags.None, (current, s) => current | (TrackObjectFlags)Enum.Parse(typeof(TrackObjectFlags), s, true));

                ObjectManager.ObjectManager.Me.MeObjectTrack = none;
            }
            catch (Exception exception)
            {
                Logging.WriteError("TrackObjectFlags(List<string> listTrackObjectFlags): " + exception);
            }
        }

        public static void TrackCreatureFlags(List<string> listTrackCreatureFlags)
        {
            try
            {
                TrackCreatureFlags none = listTrackCreatureFlags.Aggregate<string, TrackCreatureFlags>(0, (current, s) => current | (TrackCreatureFlags)Enum.Parse(typeof(TrackCreatureFlags), s, true));

                ObjectManager.ObjectManager.Me.MeCreatureTrack = none;
            }
            catch (Exception exception)
            {
                Logging.WriteError("TrackCreatureFlags(List<string> listTrackCreatureFlags): " + exception);
            }
        }
    }
}
