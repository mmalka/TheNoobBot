using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Helpers;
using Timer = nManager.Helpful.Timer;

namespace nManager.Products
{
    public class Products
    {
        private static IProduct instanceFromOtherAssembly;
        private static Assembly assembly;
        private static object obj;
        private static readonly Engine Fsm = new Engine(false);

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
                    instanceFromOtherAssembly = null;
                    assembly = null;
                    obj = null;

                    assembly = Assembly.LoadFrom(Application.StartupPath + "\\Products\\" + nameDll + ".dll");
                    obj = assembly.CreateInstance("Main", false);
                    instanceFromOtherAssembly = obj as IProduct;
                    instanceFromOtherAssembly.Initialize();
                }
            }
            catch (Exception e)
            {
                ProductName = "";
                MessageBox.Show("Exception: " + e);
                Logging.WriteError("LoadProducts(string nameDll): " + e);
                DisposeProduct();
                instanceFromOtherAssembly = null;
                assembly = null;
                obj = null;
            }
        }

        public static IProduct LoadProductsWithoutInit(string nameDll)
        {
            try
            {
                Assembly assembly2 = Assembly.LoadFrom(Others.GetCurrentDirectory + "\\Products\\" + nameDll + ".dll");
                var obj2 = assembly2.CreateInstance("Main", true);
                var instanceFromOtherAssembly2 = obj2 as IProduct;
                return instanceFromOtherAssembly2;
            }
            catch (Exception e)
            {
                ProductName = "";
                MessageBox.Show("Exception: " + e);
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

        private static bool _isDisposed;

        private static void ThreadDisposeProduct()
        {
            try
            {
                ProductStop();
                if (instanceFromOtherAssembly != null)
                    instanceFromOtherAssembly.Dispose();
                instanceFromOtherAssembly = null;
                assembly = null;
                obj = null;
            }
            catch (Exception e)
            {
                Logging.WriteError("ThreadDisposeProduct(): " + e);
                instanceFromOtherAssembly = null;
                assembly = null;
                obj = null;
            }
            _isDisposed = true;
        }


        public static bool IsAliveProduct
        {
            get { return (instanceFromOtherAssembly != null); }
        }

        public static bool ProductStart()
        {
            try
            {
                if (instanceFromOtherAssembly != null)
                {
                    _inPause = false;

                    instanceFromOtherAssembly.Start();
                    Statistics.Reset();

                    // Fsm
                    Fsm.States.Clear();
                    Fsm.AddState(new Wow.Bot.States.relogger {Priority = 10});
                    Fsm.AddState(new Wow.Bot.States.StopBotIf {Priority = 5});
                    Fsm.AddState(new Wow.Bot.States.Idle {Priority = 1});
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

        public static bool ProductStop()
        {
            try
            {
                Fsm.StopEngine();
                if (instanceFromOtherAssembly != null)
                {
                    instanceFromOtherAssembly.Stop();
                    Thread.Sleep(500);
                    MovementManager.StopMove();
                    Fight.StopFight();
                    CombatClass.DisposeCombatClass();
                    LongMove.StopLongMove();
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
                if (instanceFromOtherAssembly != null)
                {
                    instanceFromOtherAssembly.Settings();
                    return true;
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("ProductSettings(): " + e);
            }
            return false;
        }

        public static bool IsStarted
        {
            get
            {
                try
                {
                    if (instanceFromOtherAssembly != null)
                    {
                        return instanceFromOtherAssembly.IsStarted;
                    }
                }
                catch (Exception e)
                {
                    Logging.WriteError("Products > IsStarted: " + e);
                }
                return false;
            }
        }

        private static string _productName = "";

        public static string ProductName
        {
            get { return _productName; }
            private set { _productName = value; }
        }

        private static bool _inPause;

        public static bool InPause
        {
            get { return _inPause; }
            set { _inPause = value; }
        }

        private static bool _oldIsStarted;
        private static bool _oldIsAliveProduc;
        private static Thread _threadEventChangeProduct;

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

        public delegate void IsStartedChangeEventHandler(IsStartedChangeEventArgs e);

        public static event IsStartedChangeEventHandler OnChangedIsStarted;

        public class IsStartedChangeEventArgs : EventArgs
        {
            public bool IsStarted { get; set; }
        }

        public delegate void IsAliveProductChangeEventHandler(IsAliveProductChangeEventArgs e);

        public static event IsAliveProductChangeEventHandler OnChangedIsAliveProduct;

        public class IsAliveProductChangeEventArgs : EventArgs
        {
            public bool IsAliveProduct { get; set; }
        }
    }
}