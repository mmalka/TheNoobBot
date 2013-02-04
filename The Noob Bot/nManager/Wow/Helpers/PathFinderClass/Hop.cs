using nManager.Wow.Class;

namespace nManager.Wow.Helpers.PathFinderClass
{
    internal enum HopType
    {
        Waypoint,
        Flightmaster,
    }

    internal class Hop
    {
        public HopType Type { get; set; }
        public Point Location { get; set; }

        /// <summary>
        /// Only valid for hops with Flightmaster type
        /// </summary>
        public string FlightTarget { get; set; }
    }
}