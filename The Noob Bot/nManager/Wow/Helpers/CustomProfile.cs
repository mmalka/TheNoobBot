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

namespace nManager.Wow.Helpers
{
    public class CustomProfile
    {
        private static ICustomProfile _instanceFromOtherAssembly;
        private static Assembly _assembly;
        private static object _obj;
        private static Thread _worker;
        private static string _pathToCustomProfileFile = "";
        private static string _threadName = "";

        public static bool IsAliveCustomProfile
        {
            get
            {
                try
                {
                    return (_obj != null);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("IsAliveCustomProfile: " + exception);
                    return false;
                }
            }
        }

        public static void LoadCustomProfile(bool init, string profileTypeScriptName)
        {
            try
            {
                if (profileTypeScriptName != "")
                {
                    string pathToCustomProfileFile = Application.StartupPath +
                                                     "\\Profiles\\Battlegrounder\\ProfileType\\CSharpProfile\\" +
                                                     profileTypeScriptName;
                    string fileExt = pathToCustomProfileFile.Substring(pathToCustomProfileFile.Length - 3);
                    if (fileExt == "dll")
                        LoadCustomProfile(pathToCustomProfileFile, false, false, false);
                    else
                        LoadCustomProfile(pathToCustomProfileFile);
                }
                else
                    Logging.Write("ProfileType: ProfileTypeScriptName is empty.");
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadCustomProfile(): " + exception);
            }
        }

        public static void LoadCustomProfile(string pathToCustomProfileFile, bool settingOnly = false,
                                             bool resetSettings = false,
                                             bool cSharpFile = true)
        {
            try
            {
                _pathToCustomProfileFile = pathToCustomProfileFile;
                if (_instanceFromOtherAssembly != null)
                {
                    _instanceFromOtherAssembly.Dispose();
                }

                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;

                if (cSharpFile)
                {
                    CodeDomProvider cc = new CSharpCodeProvider();
                    var cp = new CompilerParameters();
                    IEnumerable<string> assemblies = AppDomain.CurrentDomain
                                                              .GetAssemblies()
                                                              .Where(
                                                                  a =>
                                                                  !a.IsDynamic &&
                                                                  !a.CodeBase.Contains(
                                                                      (Process.GetCurrentProcess().ProcessName + ".exe")))
                                                              .Select(a => a.Location);
                    cp.ReferencedAssemblies.AddRange(assemblies.ToArray());
                    StreamReader sr = File.OpenText(pathToCustomProfileFile);
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
                    _threadName = "CustomProfile CS";
                }
                else
                {
                    _assembly = Assembly.LoadFrom(_pathToCustomProfileFile);
                    _obj = _assembly.CreateInstance("Main", false);
                    _threadName = "CustomProfile DLL";
                }
                if (_obj == null || _assembly == null) return;
                _instanceFromOtherAssembly = _obj as ICustomProfile;
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
                    Logging.WriteError("Custom Profile Loading error.");
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadCustomProfile(string _pathToCustomProfileFile): " + exception);
            }
        }

        public static void DisposeCustomProfile()
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
                Logging.WriteError("DisposeCustomProfile(): " + exception);
            }
            finally
            {
                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;
            }
        }

        public static void ResetCustomProfile()
        {
            try
            {
                if (IsAliveCustomProfile)
                {
                    DisposeCustomProfile();
                    Thread.Sleep(1000);
                    string fileExt = _pathToCustomProfileFile.Substring(_pathToCustomProfileFile.Length - 3);
                    if (fileExt == "dll")
                        LoadCustomProfile(_pathToCustomProfileFile, false, false, false);
                    else
                        LoadCustomProfile(_pathToCustomProfileFile);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ResetCustomProfile(): " + exception);
            }
        }

        public static void ShowConfigurationCustomProfile(string filePath)
        {
            try
            {
                string fileExt = filePath.Substring(filePath.Length - 3);
                if (fileExt == "dll")
                    LoadCustomProfile(filePath, true, false, false);
                else
                    LoadCustomProfile(filePath, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationCustomProfile(): " + exception);
            }
        }

        public static void ResetConfigurationCustomProfile(string filePath)
        {
            try
            {
                string fileExt = filePath.Substring(filePath.Length - 3);
                if (fileExt == "dll")
                    LoadCustomProfile(filePath, true, true, false);
                else
                    LoadCustomProfile(filePath, true, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationCustomProfile(): " + exception);
            }
        }
    }


    public interface ICustomProfile
    {
        #region Methods

        void Initialize();

        void Dispose();

        void ShowConfiguration();

        void ResetConfiguration();

        #endregion Methods
    }
}