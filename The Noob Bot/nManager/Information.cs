namespace nManager
{
    public static class Information
    {
        public const string TargetWowVersion = "6.1.2";
        public const int TargetWowBuild = 19865;
        public const int MinWowBuild = 17128 + 1; // no need to update it
        public const int MaxWowBuild = 23000; // not to be changed until we are closer
        // The Min and Max check are in case the build offset have changed and the address return a value higher than 0, it's kind of a pattern check.
        public const string Version = "4.6.5"; // current = 4.6.5 vs DevVersionRestrict
        public static string MainTitle = "The Noob Bot " + Version;
    }
}