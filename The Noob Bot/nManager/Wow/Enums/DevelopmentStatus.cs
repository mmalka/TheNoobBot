namespace nManager.Wow.Enums
{
    public enum DevelopmentStatus
    {
        // coments examples are based as of march the 16th 2017
        Completed, // completed and up to date, ie: "Silverpines"
        WorkInProgress, // DEFAULT: incomplete and untested, ie: Scholazar Basin
        Outdated, // completed but outdated, ie: Blood Elf Starting Zone
        Untested, // completed but untested, ie: Alliance Tanaris
        ReleaseCandidate // ready to release unless significant bugs emerge, ie: Horde Tanaris
    }
}