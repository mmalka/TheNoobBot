public class Main : nManager.Helpful.Interface.IScriptOnlineManager
{
    public void Initialize()
    {
        try
        {
            /*
            if (nManager.Information.Version == "1.0 Beta 5")
            {
                System.IO.File.Copy(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", System.Diagnostics.Process.GetCurrentProcess().ProcessName + "2.exe", true);
                System.IO.File.Copy("nManager.dll", "nManager2.dll", true);
                if (nManager.Helpful.Others.GetFileMd5CheckSum(System.Diagnostics.Process.GetCurrentProcess().ProcessName + "2.exe") != "086FE18ED584F81DF14186F4160E68A4" ||
                    nManager.Helpful.Others.GetFileMd5CheckSum("nManager2.dll") != "205BA505A997676CF840F3C0EB5B1068")
                {
                    System.IO.File.Delete(System.Diagnostics.Process.GetCurrentProcess().ProcessName + "2.exe");
                    System.IO.File.Delete("nManager2.dll");
                    nManager.Helpful.Logging.WriteDebug("Server init OK");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    return;
                }
                System.IO.File.Delete(System.Diagnostics.Process.GetCurrentProcess().ProcessName + "2.exe");
                System.IO.File.Delete("nManager2.dll");
            }
            */

            // put offset
            if (nManager.Information.ForBuildWowVersion == 15005 || nManager.Information.ForBuildWowVersion == 15050)
                nManager.Wow.Patchables.Addresses.ObjectManager.clientConnection = 0x9BE678;
            if (nManager.Information.ForBuildWowVersion == 15211)
                nManager.Wow.Patchables.Addresses.ObjectManager.clientConnection = 0x9BD030;

            if (The_Noob_Bot.LoginServer.IsFreeVersion)
            {
                System.Threading.Thread.Sleep(1000*60*20);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            //nManager.Helpful.Logging.WriteDebug("Server init OK");
        }
        catch (System.Exception e)
        {
            nManager.Helpful.Logging.WriteDebug("Error Script:\n" + e);
        }
    }
}