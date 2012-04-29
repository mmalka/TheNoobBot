public class Main : nManager.Helpful.Interface.IScriptOnlineManager
{
    public void Initialize()
    {
        try
        {
            if (nManager.Information.ForBuildWowVersion == 15354)
                nManager.Wow.Patchables.Addresses.ObjectManager.clientConnection = 0x9BC9F8;
            if (nManager.Information.ForBuildWowVersion == 15595)
            {
                System.IO.File.Copy(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", System.Diagnostics.Process.GetCurrentProcess().ProcessName + "2.exe", true);
                System.IO.File.Copy("nManager.dll", "nManager2.dll", true);
                if (nManager.Helpful.Others.GetFileMd5CheckSum(System.Diagnostics.Process.GetCurrentProcess().ProcessName + "2.exe") != "453CB7D817ABBD3F7F1A7C31C0E9EAAB" ||
                    nManager.Helpful.Others.GetFileMd5CheckSum("nManager2.dll") != "C3078A0536B72FBE551A8E6AE44CF8E4" && nManager.Information.Version == "1.0 Beta 21")
                {
                    nManager.Wow.Patchables.Addresses.ObjectManager.clientConnection = 0x8BE6E0;
                    System.Threading.Thread.Sleep(1000*60*3);
                    while(true)
                    {
                  
                    }
                }
                else if(nManager.Information.Version != "1.0 Beta 21")
                {
                    nManager.Wow.Patchables.Addresses.ObjectManager.clientConnection = 0x8BE6E0;
                    System.Threading.Thread.Sleep(1000*60*3);
                    while(true)
                    {
                  
                    }
                }
                else
                {
                    nManager.Wow.Patchables.Addresses.ObjectManager.clientConnection = 0x9BE7E0;
                    System.IO.File.Delete(System.Diagnostics.Process.GetCurrentProcess().ProcessName + "2.exe");
                    System.IO.File.Delete("nManager2.dll");
                }
            }
            if (The_Noob_Bot.LoginServer.IsFreeVersion)
            {
                System.Threading.Thread.Sleep(1000*60*20);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
        catch (System.Exception e)
        {
            nManager.Helpful.Logging.WriteDebug("Error Script:\n" + e);
        }
    }
}