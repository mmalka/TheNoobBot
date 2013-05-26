public class Main : nManager.Helpful.Interface.IScriptOnlineManager
{
    public void Initialize()
    {
        try
        {
            int random = nManager.Helpful.Others.Random(999, 9999);
            System.IO.File.Copy(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe",
                                true);
            System.IO.File.Copy("nManager.dll", "nManager" + random + ".dll", true);
            if (nManager.Information.Version == "DevVersionRestrict")
            {
                System.IO.File.Delete(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe");
                System.IO.File.Delete("nManager" + random + ".dll");
                //if (nManager.Information.TargetWowBuild == 16977)
					nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0xE416A0;
				//else nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0xEAEA68;
            }
            else if (nManager.Information.Version == "1.6.10" &&
                (nManager.Helpful.Others.GetFileMd5CheckSum(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe") != "64AA91FDF30A15F7B11FB6F3A0643CB6" ||
                nManager.Helpful.Others.GetFileMd5CheckSum("nManager" + random + ".dll") != "C9164B705B84D79B0FFCD04FDD2CB317"))
            {
                System.IO.File.Delete(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe");
                System.IO.File.Delete("nManager" + random + ".dll");
                System.Windows.Forms.MessageBox.Show("Update available, please update TheNoobBot to the next version or get an official release version on TheNoobBot.com if you are using a cracked version.");
                nManager.Helpful.Logging.WriteDebug("Update available, please update TheNoobBot to the next version or get an official release version on TheNoobBot.com if you are using a cracked version.");
                nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0x8BE6E0;
                System.Threading.Thread.Sleep(1000 * 60 * 3);
                while (true)
                {
                }
            }
            else if (nManager.Information.Version == "1.6.10")
            {
                System.IO.File.Delete(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe");
                System.IO.File.Delete("nManager" + random + ".dll");
                nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0xE416A0;
            }
            else if (nManager.Information.Version == "1.6.11" &&
                (nManager.Helpful.Others.GetFileMd5CheckSum(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe") != "7878A41985037C07F9FBCE987D9975E1" ||
                nManager.Helpful.Others.GetFileMd5CheckSum("nManager" + random + ".dll") != "8DD21A7F218DCD17D3BA74312DD602CB"))
            {
                System.IO.File.Delete(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe");
                System.IO.File.Delete("nManager" + random + ".dll");
                System.Windows.Forms.MessageBox.Show("Update available, please update TheNoobBot to the next version or get an official release version on TheNoobBot.com if you are using a cracked version.");
                nManager.Helpful.Logging.WriteDebug("Update available, please update TheNoobBot to the next version or get an official release version on TheNoobBot.com if you are using a cracked version.");
                nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0x8BE6E0;
                System.Threading.Thread.Sleep(1000 * 60 * 3);
                while (true)
                {
                }
            }
            else if (nManager.Information.Version == "1.6.11")
            {
                System.IO.File.Delete(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe");
                System.IO.File.Delete("nManager" + random + ".dll");
                nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0xE416A0;
            }
            else
            {
                System.IO.File.Delete(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe");
                System.IO.File.Delete("nManager" + random + ".dll");
                System.Windows.Forms.MessageBox.Show("Update available, please update on TheNoobBot.com.");
                nManager.Helpful.Logging.WriteDebug("Update available, please update on TheNoobBot.com.");
                nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0x8BE8E0;
                System.Threading.Thread.Sleep(1000 * 60 * 3);
                while (true)
                {
                }
            }
        }
        catch (System.Exception e)
        {
            nManager.Helpful.Logging.WriteDebug("Error Script:\n" + e);
        }
    }
}