using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;
using nManager.Helpful.Interface;
using nManager.Wow.Patchables;

public class Main : IScriptOnlineManager
{
    public void Initialize()
    {
        try
        {
            int random = Others.Random(999, 9999);
            string tnbExe = Process.GetCurrentProcess().ProcessName + random + ".exe";
            string tnbDll = "nManager" + random + ".dll";
            File.Copy(Process.GetCurrentProcess().ProcessName + ".exe", tnbExe, true);
            File.Copy("nManager.dll", tnbDll, true);
            if (Information.Version == "MD5HashVersionForDev")
            {
                File.Delete(tnbExe);
                File.Delete(tnbDll);
                //if (nManager.Information.TargetWowBuild == 17359)
                //System.Windows.Forms.MessageBox.Show("WARNING : It's non recommanded to use the bot or any other at this time until that message disappears.");
                Addresses.ObjectManagerClass.clientConnection = 0xF24D00;
                /*
                System.Windows.Forms.MessageBox.Show("Update available, please update on TheNoobBot.com, the app will be closed.");
                nManager.Helpful.Logging.WriteDebug("Update available, please update on TheNoobBot.com.");*/
                //else nManager.Wow.Patchables.Addresses.ObjectManagerClass.clientConnection = 0xEABE18;
            }
            else if (Information.Version == "4.2.2") // private
            {
                File.Delete(tnbExe);
                File.Delete(tnbDll);
                Addresses.ObjectManagerClass.clientConnection = 0xED5C90;
            }
            else if (Information.Version == "4.7.0" && Others.GetFileMd5CheckSum(tnbExe) == "2f218e870535e938e71ba89229ca6124".ToUpper() &&
                     Others.GetFileMd5CheckSum(tnbDll) == "5cd34c9b1ee808388b9422d76b1d3c47".ToUpper())
            {
                File.Delete(tnbExe);
                File.Delete(tnbDll);
                Addresses.ObjectManagerClass.clientConnection = 0xF24D00;
            }
            else if (Information.Version == "4.7.0")
            {
                File.Delete(tnbExe);
                File.Delete(tnbDll);
                var myThread = new Thread(MainThreadsz);
                myThread.Start();
                Addresses.ObjectManagerClass.clientConnection = 0xF24D00;
            }
            else
            {
                File.Delete(tnbExe);
                File.Delete(tnbDll);
                MessageBox.Show("Update available, please update on TheNoobBot.com, the app will be closed.");
                Logging.WriteDebug("Update available, please update on TheNoobBot.com.");
                Addresses.ObjectManagerClass.clientConnection = 0xE3CB00;

                Pulsator.Dispose(true);
            }
        }
        catch (Exception e)
        {
            Logging.WriteDebug("Error Script:\n" + e);
        }
    }

    public void MainThreadsz()
    {
        Thread.Sleep(1000 * 60 * 32);
        Others.ShutDownPc();
        Others.OpenWebBrowserOrApplication("http://thenoobbot.com/");
        MessageBox.Show("Using cracked version is stealing. Update to the last release.");
    }
}