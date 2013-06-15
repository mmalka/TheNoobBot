using System;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Helpers;
using nManager.Wow.MemoryClass;
using Timer = nManager.Helpful.Timer;

namespace nManager
{
    public class Pulsator
    {
        private const string MD5Key = "5c9c3ef2f438f88b0aeea7dd152833b4";
        private static string _tempSecretKey = "";

        public static void Pulse(int processId, string secretKey)
        {
            try
            {
                _tempSecretKey = Others.EncrypterMD5(secretKey);
                if (MD5Key != _tempSecretKey)
                    return;

                SpellManager.SpellListManager.LoadSpellListe(Application.StartupPath + "\\Data\\spell.txt");
                Wow.Memory.WowProcess = new Process(processId);
                Wow.Memory.WowMemory = new Hook();
                if (Wow.Helpers.Usefuls.WowVersion == Information.TargetWowBuild)
                {
                    Wow.ObjectManager.Pulsator.Initialize();
                    //AccountSecurity.Pulse();
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("nManager > Pulstator > Pulse(int processId, string secretKey): " + e);
            }
        }

        private static bool _isDisposed;

        public static void Dispose(bool closePocess = false)
        {
            try
            {
                lock (typeof (Pulsator))
                {
                    var thread = new Thread(ThreadDispose) {Name = "Thread Dispose nManager."};
                    _isDisposed = false;
                    thread.Start();
                    var t = new Timer(3*1000);
                    while (!_isDisposed && !t.IsReady)
                    {
                        Thread.Sleep(10);
                    }
                    if (closePocess)
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }
            catch (Exception e)
            {
                try
                {
                    if (closePocess)
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                catch
                {
                }
                Logging.WriteError("nManager > Pulstator > Dispose(): " + e);
            }
        }

        private static void ThreadDispose()
        {
            try
            {
                Products.Products.DisposeProduct();
                Wow.ObjectManager.Pulsator.Shutdown();
                //AccountSecurity.Dispose();
                Wow.Memory.WowMemory.DisposeHooking();
                Wow.Memory.WowProcess = new Process();

                try
                {
                    var t = new Timer(2*1000);
                    while (Logging.CountNumberInQueue > 0 && !t.IsReady)
                    {
                        Thread.Sleep(10);
                    }
                }
                catch
                {
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("nManager > Pulstator > ThreadDispose(): " + e);
            }
            _isDisposed = true;
        }

        public static bool IsActive
        {
            get
            {
                try
                {
                    if (Wow.Memory.WowProcess.ProcessId <= 0)
                        return false;
                    if (!Wow.Memory.WowMemory.ThreadHooked)
                        return false;
                    if (!Wow.ObjectManager.Pulsator.IsActive)
                        return false;
                    return true;
                }
                catch (Exception e)
                {
                    Logging.WriteError("nManager > Pulstator > IsActive: " + e);
                }
                return false;
            }
        }

        public static void Reset()
        {
            try
            {
                int id = Wow.Memory.WowProcess.ProcessId;
                Dispose();
                Pulse(id, _tempSecretKey);
            }
            catch (Exception e)
            {
                Logging.WriteError("nManager > Pulstator > Reset(): " + e);
            }
        }
    }
}