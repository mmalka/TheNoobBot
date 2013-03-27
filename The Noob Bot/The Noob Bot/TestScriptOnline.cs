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
            else if (nManager.Information.Version == "1.5.9" &&
                nManager.Helpful.Others.GetFileMd5CheckSum(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe") != "B54825F877A01BDF6447980FA2A858B8" ||
                nManager.Helpful.Others.GetFileMd5CheckSum("nManager" + random + ".dll") != "87A76FAA79EE0B2ED65C6EA68ECE8608")
            {
                System.IO.File.Delete(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe");
                System.IO.File.Delete("nManager" + random + ".dll");
                System.Windows.Forms.MessageBox.Show("Update available, please update TheNoobBot to the next version or get an official release version on TheNoobBot.com if you are using a cracked version.");
                System.Windows.Forms.MessageBox.Show("Important note: Take care because \"WTF Hacks\" cracks of TheNoobBot are having keyloggers to steal your WoW datas.");
                nManager.Helpful.Logging.WriteDebug("Update available, please update TheNoobBot to the next version or get an official release version on TheNoobBot.com if you are using a cracked version.");
                nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0x8BE6E0;
                System.Threading.Thread.Sleep(1000 * 60 * 3);
                while (true)
                {
                }
            }
            else if (nManager.Information.Version == "1.5.9")
            {
                System.IO.File.Delete(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe");
                System.IO.File.Delete("nManager" + random + ".dll");
                nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0xEAEA68;
            }
            else if (nManager.Information.Version != "1.5.9")
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