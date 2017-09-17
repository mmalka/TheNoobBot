using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow;
using nManager.Wow.Bot.States;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using Timer = nManager.Helpful.Timer;

namespace nManager.Products
{
    public class Products
    {
        public delegate void IsAliveProductChangeEventHandler(IsAliveProductChangeEventArgs e);

        public delegate void IsStartedChangeEventHandler(IsStartedChangeEventArgs e);

        private static IProduct _instanceFromOtherAssembly;
        private static Assembly _assembly;
        private static object _obj;
        private static readonly Engine Fsm = new Engine(false);
        private static bool _isDisposed;
        private static string _productName = "";
        private static bool _inAutoPause;
        private static bool _inManualPause;
        private static bool _oldIsStarted;
        private static bool _oldIsAliveProduc;
        private static Thread _threadEventChangeProduct;

        public static bool IsAliveProduct
        {
            get { return (_instanceFromOtherAssembly != null); }
        }

        public static bool IsStarted
        {
            get
            {
                try
                {
                    if (_instanceFromOtherAssembly != null)
                    {
                        return _instanceFromOtherAssembly.IsStarted;
                    }
                }
                catch (Exception e)
                {
                    Logging.WriteError("Products > IsStarted: " + e);
                }
                return false;
            }
        }

        public static string ProductName
        {
            get { return _productName; }
            private set { _productName = value; }
        }

        public static bool InAutoPause
        {
            get { return _inAutoPause; }
            set
            {
                _inAutoPause = value;
                Logging.WriteFileOnly("Paused from: " + Wow.MemoryClass.Hook.CurrentCallStack);
            }
        }

        public static bool InManualPause
        {
            get { return _inManualPause; }
            set
            {
                _inManualPause = value;
                Logging.WriteFileOnly("Paused from: " + Wow.MemoryClass.Hook.CurrentCallStack);
            }
        }

        public static Point TravelTo { get; set; }
        public static Point TravelFrom { get; set; }
        public static int TravelToContinentId { get; set; }
        public static int TravelFromContinentId { get; set; }
        public static bool ForceTravel { get; set; }
        public static Func<Point, bool> TargetValidationFct { get; set; }

        public static void LoadProducts(string nameDll)
        {
            try
            {
                lock (typeof (Products))
                {
                    if (_threadEventChangeProduct == null)
                    {
                        _threadEventChangeProduct = new Thread(ThreadEventChangeProduct)
                        {Name = "ThreadEventChangeProduct"};
                        _threadEventChangeProduct.Start();
                    }

                    ProductName = nameDll;
                    _instanceFromOtherAssembly = null;
                    _assembly = null;
                    _obj = null;

                    _assembly = Assembly.LoadFrom(Application.StartupPath + "\\Products\\" + nameDll + ".dll");
                    _obj = _assembly.CreateInstance("Main", false);
                    _instanceFromOtherAssembly = _obj as IProduct;
                    if (_instanceFromOtherAssembly != null) _instanceFromOtherAssembly.Initialize();
                }
            }
            catch (Exception e)
            {
                ProductName = "";
                MessageBox.Show(string.Format("Exception: {0}", e));
                Logging.WriteError("LoadProducts(string nameDll): " + e);
                DisposeProduct();
                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;
            }
        }

        public static IProduct LoadProductsWithoutInit(string nameDll)
        {
            try
            {
                Assembly assembly2 = Assembly.LoadFrom(Others.GetCurrentDirectory + "\\Products\\" + nameDll + ".dll");
                object obj2 = assembly2.CreateInstance("Main", true);
                var instanceFromOtherAssembly2 = obj2 as IProduct;
                return instanceFromOtherAssembly2;
            }
            catch (Exception e)
            {
                ProductName = "";
                MessageBox.Show(string.Format("Exception: {0}", e));
                Logging.WriteError("LoadProductsWithoutInit(string nameDll)(string nameDll): " + e);
            }
            return null;
        }

        public static void DisposeProduct()
        {
            try
            {
                lock (typeof (Products))
                {
                    var thread = new Thread(ThreadDisposeProduct) {Name = "Thread Dispose Product."};
                    _isDisposed = false;
                    thread.Start();
                    var t = new Timer(2*1000);
                    while (!_isDisposed && !t.IsReady)
                    {
                        Thread.Sleep(10);
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("DisposeProduct(): " + e);
            }
        }

        private static void ThreadDisposeProduct()
        {
            try
            {
                ProductStop();
                if (_instanceFromOtherAssembly != null)
                    _instanceFromOtherAssembly.Dispose();
                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;
            }
            catch (Exception e)
            {
                Logging.WriteError("ThreadDisposeProduct(): " + e);
                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;
            }
            _isDisposed = true;
        }

        public static void ToggleCinematic(bool started = true)
        {
            if (started)
            {
                Lua.LuaDoString("StopCinematic()");
                Thread.Sleep(1000);
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + " = tostring(InCinematic())");
                string ret = Lua.GetLocalizedText(randomString);
                if (ret == "false")
                    return; // leave if not InCinematic.
            }
            _inAutoPause = started;
        }

        public static bool ProductStart()
        {
            try
            {
                if (_instanceFromOtherAssembly != null)
                {
                    _inAutoPause = false;
                    _inManualPause = false;
                    TravelToContinentId = 9999999;
                    TravelFromContinentId = 9999999;
                    TravelTo = new Point();
                    TravelFrom = new Point();
                    ForceTravel = false;

                    _instanceFromOtherAssembly.Start();
                    if (!_instanceFromOtherAssembly.IsStarted)
                        return false;
                    EventsListener.HookEvent(WoWEventsType.LOOT_READY, callback => FarmingTask.TakeFarmingLoots());
                    EventsListener.HookEvent(WoWEventsType.LOOT_OPENED, callback => FarmingTask.TakeFarmingLoots());
                    EventsListener.HookEvent(WoWEventsType.CINEMATIC_START, callback => ToggleCinematic(true));
                    EventsListener.HookEvent(WoWEventsType.CINEMATIC_STOP, callback => ToggleCinematic(false));

                    Statistics.Reset();

                    // Fsm
                    Fsm.States.Clear();
                    Fsm.AddState(new Relogger {Priority = 10});
                    Fsm.AddState(new StopBotIf {Priority = 5});
                    Fsm.AddState(new Idle {Priority = 1});
                    Fsm.States.Sort();
                    Fsm.StartEngine(1);

                    return true;
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("ProductStart(): " + e);
            }
            return false;
        }

        public static bool ProductRemoteStart(string[] args)
        {
            try
            {
                if (_instanceFromOtherAssembly != null)
                {
                    _inAutoPause = false;
                    _inManualPause = false;
                    TravelToContinentId = 9999999;
                    TravelFromContinentId = 9999999;
                    TravelTo = new Point();
                    TravelFrom = new Point();
                    ForceTravel = false;
                    nManagerSetting.ActivateProductTipOff = false;

                    _instanceFromOtherAssembly.RemoteStart(args);
                    if (!_instanceFromOtherAssembly.IsStarted)
                        return false;
                    EventsListener.HookEvent(WoWEventsType.LOOT_READY, callback => FarmingTask.TakeFarmingLoots());
                    EventsListener.HookEvent(WoWEventsType.LOOT_OPENED, callback => FarmingTask.TakeFarmingLoots());
                    EventsListener.HookEvent(WoWEventsType.CINEMATIC_START, callback => ToggleCinematic(true));
                    EventsListener.HookEvent(WoWEventsType.CINEMATIC_STOP, callback => ToggleCinematic(false));

                    Statistics.Reset();

                    // Fsm
                    Fsm.States.Clear();
                    Fsm.AddState(new Relogger {Priority = 10});
                    Fsm.AddState(new StopBotIf {Priority = 5});
                    Fsm.AddState(new Idle {Priority = 1});
                    Fsm.States.Sort();
                    Fsm.StartEngine(1);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("ProductRemoteStart(): " + (object) ex, true);
            }
            return false;
        }

        public static bool ProductRestart()
        {
            try
            {
                ProductStop();
                ProductStart();
            }
            catch (Exception e)
            {
                Logging.WriteError("ProductRestart(): " + e);
            }
            return false;
        }

        public static void ProductStopFromFSM()
        {
            ProductStop();
        }

        public static bool ProductStop()
        {
            try
            {
                Fsm.StopEngine();
                if (_instanceFromOtherAssembly != null)
                {
                    EventsListener.UnHookEvent(WoWEventsType.LOOT_READY, callback => FarmingTask.TakeFarmingLoots());
                    EventsListener.UnHookEvent(WoWEventsType.LOOT_OPENED, callback => FarmingTask.TakeFarmingLoots());
                    EventsListener.UnHookEvent(WoWEventsType.CINEMATIC_START, callback => ToggleCinematic(true));
                    EventsListener.UnHookEvent(WoWEventsType.CINEMATIC_STOP, callback => ToggleCinematic(false));
                    _instanceFromOtherAssembly.Stop();
                    Thread.Sleep(500);
                    MovementManager.StopMove();
                    Fight.StopFight();
                    CombatClass.DisposeCombatClass();
                    LongMove.StopLongMove();
                    Memory.WowMemory.GameFrameUnLock();
                    return true;
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("ProductStop(): " + e);
            }
            return false;
        }

        public static bool ProductSettings()
        {
            try
            {
                if (_instanceFromOtherAssembly != null)
                {
                    _instanceFromOtherAssembly.Settings();
                    return true;
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("ProductSettings(): " + e);
            }
            return false;
        }

        private static void ThreadEventChangeProduct()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        if (_oldIsStarted != IsStarted && OnChangedIsStarted != null)
                        {
                            _oldIsStarted = IsStarted;
                            var e = new IsStartedChangeEventArgs {IsStarted = IsStarted};
                            OnChangedIsStarted(e);
                        }

                        if (_oldIsAliveProduc != IsAliveProduct && OnChangedIsAliveProduct != null)
                        {
                            _oldIsAliveProduc = IsAliveProduct;
                            var e = new IsAliveProductChangeEventArgs {IsAliveProduct = IsAliveProduct};
                            OnChangedIsAliveProduct(e);
                        }
                    }
                    catch (Exception e)
                    {
                        Logging.WriteError("ThreadEventChangeProduct()#1: " + e);
                    }

                    Thread.Sleep(500); // 100
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("ThreadEventChangeProduct()#2: " + e);
            }
        }

        public static event IsStartedChangeEventHandler OnChangedIsStarted;

        public static event IsAliveProductChangeEventHandler OnChangedIsAliveProduct;

        public class IsAliveProductChangeEventArgs : EventArgs
        {
            public bool IsAliveProduct { get; set; }
        }

        public class IsStartedChangeEventArgs : EventArgs
        {
            public bool IsStarted { get; set; }
        }
    }
}