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
    public class HealerClass
    {
        private static IHealerClass _instanceFromOtherAssembly;
        private static Assembly _assembly;
        private static object _obj;
        private static Thread _worker;
        private static string _pathToHealerClassFile = "";
        private static string _threadName = "";

        public static bool InRange(WoWUnit unit)
        {
            try
            {
                if (!IsAliveHealerClass)
                    return CombatClass.InRange(unit);
                float distance = unit.GetDistance;
                float combatReach = (float) unit.GetDescriptor<Descriptors.UnitFields>(Descriptors.UnitFields.CombatReach);
                //Logging.WriteDebug("InRange check: Distance " + Distance + ", CombatReach " + CombatReach + ", Range " + GetRange);
                return distance - combatReach <= GetRange - 0.5;
            }
            catch (Exception exception)
            {
                Logging.WriteError("HealerClass > InRange: " + exception);
            }
            return false;
        }

        public static bool InCustomRange(WoWUnit unit, float minRange, float maxRange)
        {
            try
            {
                if (!IsAliveHealerClass)
                    return CombatClass.InCustomRange(unit, minRange, maxRange);
                float distance = unit.GetDistance;
                float combatReach = (float) unit.GetDescriptor<Descriptors.UnitFields>(Descriptors.UnitFields.CombatReach);
                //Logging.WriteDebug("InCustomRange check: Distance " + Distance + ", CombatReach " + CombatReach + ", minRange " + minRange + ", maxRange " + maxRange);
                return distance - combatReach <= maxRange - 0.5 && distance - combatReach >= minRange + 0.5;
            }
            catch (Exception exception)
            {
                Logging.WriteError("HealerClass > InCustomRange: " + exception);
            }
            return false;
        }

        public static bool InMinRange(WoWUnit unit)
        {
            try
            {
                if (!IsAliveHealerClass)
                    return CombatClass.InMinRange(unit);
                float distance = unit.GetDistance;
                float combatReach = (float) unit.GetDescriptor<Descriptors.UnitFields>(Descriptors.UnitFields.CombatReach);
                //Logging.WriteDebug("InMinRange check: Distance " + Distance + ", CombatReach " + CombatReach + ", Range " + GetRange);
                return distance - combatReach <= GetRange - 0.5 && distance - combatReach >= -1.5;
            }
            catch (Exception exception)
            {
                Logging.WriteError("HealerClass > InMinRange: " + exception);
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
                    Logging.WriteError("HealerClass > GetRange: " + exception);
                    return 5.0f;
                }
            }
        }

        public static bool IsAliveHealerClass
        {
            get
            {
                try
                {
                    return (_obj != null);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("IsAliveHealerClass: " + exception);
                    return false;
                }
            }
        }

        public static void LoadHealerClass()
        {
            try
            {
                if (nManagerSetting.CurrentSetting.HealerClass != "")
                {
                    string __pathToHealerClassFile = Application.StartupPath + "\\HealerClasses\\" +
                                                     nManagerSetting.CurrentSetting.HealerClass;
                    string fileExt = __pathToHealerClassFile.Substring(__pathToHealerClassFile.Length - 3);
                    if (fileExt == "dll")
                        LoadHealerClass(__pathToHealerClassFile, false, false, false);
                    else
                        LoadHealerClass(__pathToHealerClassFile);
                }
                else
                    Logging.Write("No custom class selected");
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadHealerClass(): " + exception);
            }
        }

        public static void LoadHealerClass(string pathToHealerClassFile, bool settingOnly = false,
                                           bool resetSettings = false,
                                           bool CSharpFile = true)
        {
            try
            {
                _pathToHealerClassFile = pathToHealerClassFile;
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
                    StreamReader sr = File.OpenText(pathToHealerClassFile);
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
                    _threadName = "HealerClass CS";
                }
                else
                {
                    _assembly = Assembly.LoadFrom(_pathToHealerClassFile);
                    _obj = _assembly.CreateInstance("Main", false);
                    _threadName = "HealerClass DLL";
                }
                if (_obj != null && _assembly != null)
                {
                    _instanceFromOtherAssembly = _obj as IHealerClass;
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
                            {
                                IsBackground = true,
                                Name = _threadName
                            };
                        _worker.Start();
                    }
                    else
                        Logging.WriteError("Custom Class Loading error.");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadHealerClass(string _pathToHealerClassFile): " + exception);
            }
        }

        public static void DisposeHealerClass()
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
                Logging.WriteError("DisposeHealerClass(): " + exception);
            }
            finally
            {
                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;
            }
        }

        public static void ResetHealerClass()
        {
            try
            {
                if (IsAliveHealerClass)
                {
                    DisposeHealerClass();
                    Thread.Sleep(1000);
                    string fileExt = _pathToHealerClassFile.Substring(_pathToHealerClassFile.Length - 3);
                    if (fileExt == "dll")
                        LoadHealerClass(_pathToHealerClassFile, false, false, false);
                    else
                        LoadHealerClass(_pathToHealerClassFile);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ResetHealerClass(): " + exception);
            }
        }

        public static void ShowConfigurationHealerClass(string filePath)
        {
            try
            {
                string fileExt = filePath.Substring(filePath.Length - 3);
                if (fileExt == "dll")
                    LoadHealerClass(filePath, true, false, false);
                else
                    LoadHealerClass(filePath, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationHealerClass(): " + exception);
            }
        }

        public static void ResetConfigurationHealerClass(string filePath)
        {
            try
            {
                string fileExt = filePath.Substring(filePath.Length - 3);
                if (fileExt == "dll")
                    LoadHealerClass(filePath, true, true, false);
                else
                    LoadHealerClass(filePath, true, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationHealerClass(): " + exception);
            }
        }
    }


    public interface IHealerClass
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