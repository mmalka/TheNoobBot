namespace meshPathVisualizer
{
    
    public enum HopType
    {
        Waypoint,
        Flightmaster,
    }

    public class Hop
    {
        public HopType Type { get; set; }
        public Vector3 Location { get; set; }

        /// <summary>
        /// Only valid for hops with Flightmaster type
        /// </summary>
        public string FlightTarget { get; set; }
    }

}