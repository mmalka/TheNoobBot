namespace nManager.Wow.Enums
{
    public enum DevelopmentStatus
    {
        // coments examples are based as of march the 16th 2017
        WorkInProgress, // DEFAULT: incomplete and untested, ie: Scholazar Basin
        Untested, // completed but untested, ie: Alliance Tanaris
        Outdated, // completed but outdated, ie: Blood Elf Starting Zone
        ReleaseCandidate, // ready to release unless significant bugs emerge, ie: Horde Tanaris
        Completed, // completed and up to date, ie: "Silverpines"
    }
}