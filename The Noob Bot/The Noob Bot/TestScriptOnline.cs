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
                nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0xE3CB00;
                //else nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0xEAEA68;
            }
            else if (nManager.Information.Version == "1.6.12" &&
                (nManager.Helpful.Others.GetFileMd5CheckSum(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe") != "68834AC550ECD090528879B54FD8FD57" ||
                nManager.Helpful.Others.GetFileMd5CheckSum("nManager" + random + ".dll") != "D02627C9D6493D7E55057811CD13ADB1"))
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
            else if (nManager.Information.Version == "1.6.12")
            {
                System.IO.File.Delete(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe");
                System.IO.File.Delete("nManager" + random + ".dll");
                nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0xE416A0;
            }
            else if (nManager.Information.Version == "1.7.0" &&
                (nManager.Helpful.Others.GetFileMd5CheckSum(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe") != "CD645FD452C00FCC31AB9D514D894AC9" ||
                nManager.Helpful.Others.GetFileMd5CheckSum("nManager" + random + ".dll") != "2BE10870AFB5D8E72B2768F81C7FFC13"))
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
            else if (nManager.Information.Version == "1.7.0")
            {
                System.IO.File.Delete(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe");
                System.IO.File.Delete("nManager" + random + ".dll");
                nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0xE3CB00;
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