public class Main : nManager.Helpful.Interface.IScriptOnlineManager
{
    public void Initialize()
    {
        try
        {
            if (nManager.Information.ForBuildWowVersion == 15005 || nManager.Information.ForBuildWowVersion == 15050)
                nManager.Wow.Patchables.Addresses.ObjectManager.clientConnection = 0x9BE678;
            if (nManager.Information.ForBuildWowVersion == 15211)
                nManager.Wow.Patchables.Addresses.ObjectManager.clientConnection = 0x9BD030;
            if (nManager.Information.ForBuildWowVersion == 15354)
                nManager.Wow.Patchables.Addresses.ObjectManager.clientConnection = 0x9BC9F8;
        }
        catch (System.Exception e)
        {
            nManager.Helpful.Logging.WriteDebug("Error Script:\n" + e);
        }
    }
}