using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.CSharp;
using nManager.Helpful;
using nManager.Wow.ObjectManager;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class CombatClass
    {
        private static ICombatClass _instanceFromOtherAssembly;
        private static Assembly _assembly;
        private static object _obj;
        private static Thread _worker;
        private static string _pathToCombatClassFile = "";
        private static string _threadName = "";

        public static bool InRange(WoWUnit unit)
        {
            try
            {
                float Distance = unit.GetDistance;
                float CombatReach = (float) unit.GetDescriptor<Descriptors.UnitFields>(Descriptors.UnitFields.CombatReach);
                //Logging.WriteDebug("InRange check: Distance " + Distance + ", CombatReach " + CombatReach + ", Range " + GetRange);
                return Distance - CombatReach < GetRange - 0.5;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public static bool InMinRange(WoWUnit unit)
        {
            try
            {
                float Distance = unit.GetDistance;
                float CombatReach = (float) unit.GetDescriptor<Descriptors.UnitFields>(Descriptors.UnitFields.CombatReach);
                //Logging.WriteDebug("InMinRange check: Distance " + Distance + ", CombatReach " + CombatReach + ", Range " + GetRange);
                return Distance - CombatReach < GetRange - 0.5 && Distance - CombatReach > -1.5;
            }
            catch (Exception)
            {
            }
            return false;
        }

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
                    Logging.WriteError("CombatClass > GetRange: " + exception);
                    return 5.0f;
                }
            }
        }

        public static bool IsAliveCombatClass
        {
            get
            {
                try
                {
                    return (_obj != null);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("IsAliveCombatClass: " + exception);
                    return false;
                }
            }
        }

        public static void LoadCombatClass()
        {
            try
            {
                if (nManagerSetting.CurrentSetting.CombatClass != "")
                {
                    string __pathToCombatClassFile = Application.StartupPath + "\\CombatClasses\\" +
                                                     nManagerSetting.CurrentSetting.CombatClass;
                    string fileExt = __pathToCombatClassFile.Substring(__pathToCombatClassFile.Length - 3);
                    if (fileExt == "dll")
                        LoadCombatClass(__pathToCombatClassFile, false, false, false);
                    else
                        LoadCombatClass(__pathToCombatClassFile);
                }
                else
                    Logging.Write("No custom class selected");
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadCombatClass(): " + exception);
            }
        }

        public static void LoadCombatClass(string pathToCombatClassFile, bool settingOnly = false,
                                           bool resetSettings = false,
                                           bool CSharpFile = true)
        {
            try
            {
                _pathToCombatClassFile = pathToCombatClassFile;
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
                    CompilerParameters cp = new CompilerParameters();
                    IEnumerable<string> assemblies = AppDomain.CurrentDomain
                                                              .GetAssemblies()
                                                              .Where(
                                                                  a =>
                                                                  !a.IsDynamic &&
                                                                  !a.CodeBase.Contains((Process.GetCurrentProcess().ProcessName + ".exe")))
                                                              .Select(a => a.Location);
                    cp.ReferencedAssemblies.AddRange(assemblies.ToArray());
                    StreamReader sr = File.OpenText(pathToCombatClassFile);
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
                    _threadName = "CombatClass CS";
                }
                else
                {
                    _assembly = Assembly.LoadFrom(_pathToCombatClassFile);
                    _obj = _assembly.CreateInstance("Main", false);
                    _threadName = "CombatClass DLL";
                }
                if (_obj != null && _assembly != null)
                {
                    _instanceFromOtherAssembly = _obj as ICombatClass;
                    if (_instanceFromOtherAssembly != null)
                    {
                        if (settingOnly)
                        {
                            if (resetSettings)
                                _instanceFromOtherAssembly.ResetConfiguration();
                            else
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
                Logging.WriteError("LoadCombatClass(string _pathToCombatClassFile): " + exception);
            }
        }

        public static void DisposeCombatClass()
        {
            try
            {
                if (_instanceFromOtherAssembly != null)
                {
                    _instanceFromOtherAssembly.Dispose();
                }
                if (_worker != null)
                {
                    if (_worker.IsAlive)
                    {
                        _worker.Abort();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DisposeCombatClass(): " + exception);
            }
            finally
            {
                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;
            }
        }

        public static void ResetCombatClass()
        {
            try
            {
                if (IsAliveCombatClass)
                {
                    DisposeCombatClass();
                    Thread.Sleep(1000);
                    string fileExt = _pathToCombatClassFile.Substring(_pathToCombatClassFile.Length - 3);
                    if (fileExt == "dll")
                        LoadCombatClass(_pathToCombatClassFile, false, false, false);
                    else
                        LoadCombatClass(_pathToCombatClassFile);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ResetCombatClass(): " + exception);
            }
        }

        public static void ShowConfigurationCombatClass(string filePath)
        {
            try
            {
                string fileExt = filePath.Substring(filePath.Length - 3);
                if (fileExt == "dll")
                    LoadCombatClass(filePath, true, false, false);
                else
                    LoadCombatClass(filePath, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationCombatClass(): " + exception);
            }
        }

        public static void ResetConfigurationCombatClass(string filePath)
        {
            try
            {
                string fileExt = filePath.Substring(filePath.Length - 3);
                if (fileExt == "dll")
                    LoadCombatClass(filePath, true, true, false);
                else
                    LoadCombatClass(filePath, true, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationCombatClass(): " + exception);
            }
        }
    }


    public interface ICombatClass
    {
        #region Properties

        float Range { get; }

        #endregion Properties

        #region Methods

        void Initialize();

        void Dispose();

        void ShowConfiguration();

        void ResetConfiguration();

        #endregion Methods
    }
}