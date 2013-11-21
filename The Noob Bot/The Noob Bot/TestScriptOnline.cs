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
                //if (nManager.Information.TargetWowBuild == 17359)
					nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0xEA2388;
				//else nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0xEABE18;
            }
			else if (nManager.Information.Version == "1.9.9")
			{
                System.IO.File.Delete(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe");
                System.IO.File.Delete("nManager" + random + ".dll");
                nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0xEA2388;
			}
		    else
            {
                System.IO.File.Delete(System.Diagnostics.Process.GetCurrentProcess().ProcessName + random + ".exe");
                System.IO.File.Delete("nManager" + random + ".dll");
                System.Windows.Forms.MessageBox.Show("Update available, please update on TheNoobBot.com.");
                nManager.Helpful.Logging.WriteDebug("Update available, please update on TheNoobBot.com.");
                nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0xE3CB00;
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