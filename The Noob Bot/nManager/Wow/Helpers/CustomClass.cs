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
        private static string _pathToCustomClassFile = "";
        private static string _threadName = "";

        public static float GetRange
        {
            get
            {
                try
                {
                    if (_instanceFromOtherAssembly != null)
                        return _instanceFromOtherAssembly.Range < 5.0f ? 5.0f : _instanceFromOtherAssembly.Range;
                    return 5.0f;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("CustomClass > GetRange: " + exception);
                    return 5.0f;
                }
            }
        }

        public static bool IsAliveCustomClass
        {
            get
            {
                try
                {
                    return (_obj != null);
                }
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
                {
                    string __pathToCustomClassFile = Application.StartupPath + "\\CustomClasses\\" + nManagerSetting.CurrentSetting.customClass;
                    string fileExt = __pathToCustomClassFile.Substring(__pathToCustomClassFile.Length - 3);
                    if (fileExt == "dll")
                        LoadCustomClass(__pathToCustomClassFile, false, false);
                    else
                        LoadCustomClass(__pathToCustomClassFile);
                }
                else
                    Logging.Write("No custom class selected");
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadCustomClass(): " + exception);
            }
        }

        public static void LoadCustomClass(string pathToCustomClassFile, bool settingOnly = false, bool CSharpFile = true)
        {
            try
            {
                _pathToCustomClassFile = pathToCustomClassFile;
                if (_instanceFromOtherAssembly != null)
                {
                    _instanceFromOtherAssembly.Dispose();
                }

                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;

                if (CSharpFile)
                {
                    CodeDomProvider cc = new CSharpCodeProvider();
                    var cp = new CompilerParameters();
                    var assemblies = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .Where(
                            a =>
                            !a.IsDynamic && !a.CodeBase.Contains((Process.GetCurrentProcess().ProcessName + ".exe")))
                        .Select(a => a.Location);
                    cp.ReferencedAssemblies.AddRange(assemblies.ToArray());
                    StreamReader sr = File.OpenText(pathToCustomClassFile);
                    string toCompile = sr.ReadToEnd();
                    CompilerResults cr = cc.CompileAssemblyFromSource(cp, toCompile);
                    if (cr.Errors.HasErrors)
                    {
                        String text = cr.Errors.Cast<CompilerError>().Aggregate("Compilator Error :\n",
                                                                                (current, err) => current + (err + "\n"));
                        Logging.WriteError(text);
                        MessageBox.Show(text);
                        return;
                    }

                    _assembly = cr.CompiledAssembly;
                    _obj = _assembly.CreateInstance("Main", true);
                    _threadName = 'CustomClass CS';
                }
                else if (!CSharpFile)
                {
                    _assembly = Assembly.LoadFrom(_pathToCustomClassFile);
                    _obj = _assembly.CreateInstance("Main", false);
                    _threadName = 'CustomClass DLL';
                }
                if (_obj = !null && _assembly != null)
                {
                    _instanceFromOtherAssembly = _obj as ICustomClass;
                    if (_instanceFromOtherAssembly != null)
                    {
                        if (settingOnly)
                        {
                            _instanceFromOtherAssembly.ShowConfiguration();
                            _instanceFromOtherAssembly.Dispose();
                            return;
                        }

                        _worker = new Thread(_instanceFromOtherAssembly.Initialize)
                                      {IsBackground = true, Name = _threadName};
                        _worker.Start();
                    }
                    else
                        Logging.WriteError("Custom Class Loading error.");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadCustomClass(string _pathToCustomClassFile): " + exception);
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
                    string fileExt = _pathToCustomClassFile.Substring(_pathToCustomClassFile.Length - 3);
                    if (fileExt == "dll")
                        LoadCustomClass(_pathToCustomClassFile, false, false);
                    else
                        LoadCustomClass(_pathToCustomClassFile);
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
                string fileExt = filePath.Substring(filePath.Length - 3);
                if (fileExt == "dll")
                    LoadCustomClass(filePath, true, false);
                else
                    LoadCustomClass(filePath, true);
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

        float Range { get; }

        #endregion Properties

        #region Methods

        void Initialize();

        void Dispose();

        void ShowConfiguration();

        #endregion Methods
    }
}