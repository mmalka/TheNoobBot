namespace nManager
{
    public static class Information
    {
        public const string TargetWowVersion = "5.4.0";
        public const int TargetWowBuild = 17371;
        public const int MinWowBuild = 17128 + 1; // no need to update it
        public const int MaxWowBuild = 23000; // not to be changed until we are closer
        // The Min and Max check are in case the build offset have changed and the address return a value higher than 0, it's kind of a pattern check.
        public const string Version = "DevVersionRestrict"; // current = 1.8.10
    }
}