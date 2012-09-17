using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.CSharp;
using nManager.Helpful;

namespace nManager.Wow.Helpers
{
    public class CustomClass
    {
        private static ICustomClass _instanceFromOtherAssembly;
        private static Assembly _assembly;
        private static object _obj;
        private static Thread _worker;
        private static string _pathScriptCShart = "";

        public static float GetRange
        {
            get
            {
                try
                {
                    if (_instanceFromOtherAssembly != null)
                        return _instanceFromOtherAssembly.Range < 3.6f ? 3.6f : _instanceFromOtherAssembly.Range;
                    return 3.6f;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("CustomClass > GetRange: " + exception);
                    return 3.6f;
                }
            }
        }

        public static bool IsAliveCustomClass
        {
            get
            {
                try
                { return (_obj != null); }
                catch (Exception exception)
                {
                    Logging.WriteError("IsAliveCustomClass: " + exception);
                    return false;
                }
            }
        }

        public static void LoadCustomClass()
        {
            try
            {
                if (nManagerSetting.CurrentSetting.customClass != "")
                    LoadCustomClass(Application.StartupPath + "\\CustomClasses\\" + nManagerSetting.CurrentSetting.customClass);
                else
                    Logging.Write("No custom class selected");
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadCustomClass(): " + exception);
            }
        }

        public static void LoadCustomClass(string pathScriptCShart, bool settingOnly = false)
        {
            try
            {
                _pathScriptCShart = pathScriptCShart;
                if (_instanceFromOtherAssembly != null)
                {
                    _instanceFromOtherAssembly.Dispose();
                }

                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;


                CodeDomProvider cc = new CSharpCodeProvider();
                var cp = new CompilerParameters();
                var assemblies = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Where(a => !a.IsDynamic && !a.CodeBase.Contains((Process.GetCurrentProcess().ProcessName + ".exe")))
                    .Select(a => a.Location);
                cp.ReferencedAssemblies.AddRange(assemblies.ToArray());
                StreamReader sr = File.OpenText(pathScriptCShart);
                string toCompile = sr.ReadToEnd();
                CompilerResults cr = cc.CompileAssemblyFromSource(cp, toCompile);
                if (cr.Errors.HasErrors)
                {
                    String text = cr.Errors.Cast<CompilerError>().Aggregate("Compilator Error :\n", (current, err) => current + (err + "\n"));
                    Logging.WriteError(text);
                    MessageBox.Show(text);
                    return;
                }

                _assembly = cr.CompiledAssembly;

                _obj = _assembly.CreateInstance("Main", true);
                _instanceFromOtherAssembly = _obj as ICustomClass;

                if (_instanceFromOtherAssembly != null)
                {
                    if (settingOnly)
                    {
                        _instanceFromOtherAssembly.ShowConfiguration();
                        _instanceFromOtherAssembly.Dispose();
                        return;
                    }

                    _worker = new Thread(_instanceFromOtherAssembly.Initialize) { IsBackground = true, Name = "Script CS" };
                    _worker.Start();
                }
                else
                    Logging.WriteError("Custom Class Loading error.");
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadCustomClass(string pathScriptCShart): " + exception);
            }
        }

        public static void DisposeCustomClass()
        {
            try
            {
                if (_instanceFromOtherAssembly != null)
                {
                    _instanceFromOtherAssembly.Dispose();
                }
                if (_worker.IsAlive)
                {
                    _worker.Abort();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DisposeCustomClass(): " + exception);
            }
            finally
            {
                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;
            }
        }

        public static void ResetCustomClass()
        {
            try
            {
                if (IsAliveCustomClass)
                {
                    DisposeCustomClass();
                    Thread.Sleep(1000);
                    LoadCustomClass(_pathScriptCShart);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ResetCustomClass(): " + exception);
            }
        }

        public static void ShowConfigurationCustomClass(string filePath)
        {
            try
            {
                LoadCustomClass(filePath,true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationCustomClass(): " + exception);
            }
        }
    }


    public interface ICustomClass
    {
        #region Properties

        float Range
        {
            get;
        }

        #endregion Properties

        #region Methods

        void Initialize();

        void Dispose();

        void ShowConfiguration();

        #endregion Methods
    }

}
