public class Main : nManager.Helpful.Interface.IScriptOnlineManager
{
    public void Initialize()
    {
        try
        {
            /*if (nManager.Information.ForBuildWowVersion == 16057 || nManager.Information.ForBuildWowVersion == 16135)
            {*/
            /*System.IO.File.Copy(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", System.Diagnostics.Process.GetCurrentProcess().ProcessName + "2.exe", true);
            System.IO.File.Copy("nManager.dll", "nManager2.dll", true);
            if (nManager.Helpful.Others.GetFileMd5CheckSum(System.Diagnostics.Process.GetCurrentProcess().ProcessName + "2.exe") != "0769B9087DE14057994B5972EEAFFE75" ||
                nManager.Helpful.Others.GetFileMd5CheckSum("nManager2.dll") != "44217B3CD34097479A0D7EACA524866B" && nManager.Information.Version == "1.1.3")
            {
                System.IO.File.Delete(System.Diagnostics.Process.GetCurrentProcess().ProcessName + "2.exe");
                System.IO.File.Delete("nManager2.dll");
                nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0x8BE6E0;
                System.Threading.Thread.Sleep(1000*60*3);
                while(true)
                {
                  
                }
            }
            else*/
            if (nManager.Information.Version == "1.2.15" || nManager.Information.Version == "1.3.0")
            {
                //System.IO.File.Delete(System.Diagnostics.Process.GetCurrentProcess().ProcessName + "2.exe");
                //System.IO.File.Delete("nManager2.dll");
                nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0xDC9598;
            }
            else
            {
                nManager.Helpful.Logging.WriteDebug("Update available, please update TheNoobBot to the next version.");
                nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0x8BE6E0; // wrong value
                System.Threading.Thread.Sleep(1000 * 60 * 3);
                while (true)
                {

                }
            }
            /*}*/
            /*if (The_Noob_Bot.LoginServer.IsFreeVersion)
            {
                System.Threading.Thread.Sleep(1000*60*20);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }*/
        }
        catch (System.Exception e)
        {
            nManager.Helpful.Logging.WriteDebug("Error Script:\n" + e);
        }
    }
}