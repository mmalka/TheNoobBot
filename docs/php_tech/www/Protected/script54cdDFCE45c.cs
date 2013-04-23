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
                nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0xEAEA68;
            }
            else if (nManager.Information.Version == "1.6.1" &&
                (nManager.Helpful.Others.GetFileMd5CheckSum(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe") != "01411B7B0A52FB865E0408971AA8C71D" ||
                nManager.Helpful.Others.GetFileMd5CheckSum("nManager" + random + ".dll") != "BB2C4300A351511992318FA209781269"))
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
            else if (nManager.Information.Version == "1.6.1")
            {
                System.IO.File.Delete(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe");
                System.IO.File.Delete("nManager" + random + ".dll");
                nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0xEAEA68;
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