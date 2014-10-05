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

namespace nManager.Plugins
{
    public class Plugins
    {
        public static List<Plugin> ListLoadedPlugins = new List<Plugin>();

        public static void ReLoadPlugins()
        {
            try
            {
                DisposePlugins();
                foreach (string p in nManagerSetting.CurrentSetting.ActivatedPluginsList)
                {
                    string pathToPluginFile = Application.StartupPath + "\\Plugins\\" + p;
                    var plugin = new Plugin {PathToPluginFile = pathToPluginFile};
                    plugin.LoadPlugin();
                    ListLoadedPlugins.Add(plugin);
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("LoadPlugins(): " + e);
            }
        }

        public static void DisposePlugins()
        {
            try
            {
                foreach (Plugin p in ListLoadedPlugins)
                {
                    if (p != null)
                    {
                        p.DisposePlugin();
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("DisposePlugins(): " + e);
            }
            ListLoadedPlugins.Clear();
        }
    }

    public class Plugin
    {
        private Assembly _assembly;
        private IPlugins _instanceFromOtherAssembly;
        private object _obj;
        private string _threadName = "";
        private Thread _worker;
        private string _pathToPluginFile;

        public string PathToPluginFile
        {
            get { return _pathToPluginFile; }
            set
            {
                if (value != null)
                {
                    _pathToPluginFile = value;
                    LoadPlugin(false, false, true); // Initialize a deamon version of the plugin for checking purposes.
                }
            }
        }

        public bool IsAlive
        {
            get { return _instanceFromOtherAssembly != null; }
        }

        public bool IsStarted
        {
            get { return _instanceFromOtherAssembly != null && _instanceFromOtherAssembly.IsStarted; }
        }

        public string Name
        {
            get { return _instanceFromOtherAssembly == null ? "" : _instanceFromOtherAssembly.Name; }
        }

        public string Version
        {
            get { return _instanceFromOtherAssembly == null ? "" : _instanceFromOtherAssembly.Version; }
        }

        public bool IsExpired
        {
            get
            {
                string[] targetVersion = _instanceFromOtherAssembly.TargetVersion.Split('.');
                string[] currentVersion = Information.Version.Split('.');
                return targetVersion.Length < 2 || targetVersion[0] != currentVersion[0] || targetVersion[1] != currentVersion[1];
            }
        }

        public bool IsDll
        {
            get
            {
                string fileExt = PathToPluginFile.Substring(PathToPluginFile.Length - 3);
                return fileExt == "dll";
            }
        }

        public void LoadPlugin(bool settingOnly = false, bool resetSettings = false, bool onlyCheckVersion = false)
        {
            try
            {
                DisposePlugin();
                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;

                if (!IsDll)
                {
                    CodeDomProvider cc = new CSharpCodeProvider();
                    var cp = new CompilerParameters();
                    IEnumerable<string> assemblies =
                        AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic && !a.CodeBase.Contains((Process.GetCurrentProcess().ProcessName + ".exe"))).Select(a => a.Location);
                    cp.ReferencedAssemblies.AddRange(assemblies.ToArray());
                    StreamReader sr = File.OpenText(PathToPluginFile);
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
                    if (!(_obj is IPlugins))
                    {
                        Logging.WriteError("The plugin doesn't implement IPlugins or have a different namespace than \"Main\". Path: " + PathToPluginFile);
                        return;
                    }
                }
                else
                {
                    _assembly = Assembly.LoadFrom(PathToPluginFile);
                    _obj = _assembly.CreateInstance("Main", false);
                    if (!(_obj is IPlugins))
                    {
                        Logging.WriteError("The plugin doesn't implement IPlugins or have a different namespace than \"Main\". Path: " + PathToPluginFile);
                        return;
                    }
                }
                if (_assembly != null)
                {
                    _instanceFromOtherAssembly = _obj as IPlugins;
                    foreach (Plugin plugin in Plugins.ListLoadedPlugins)
                    {
                        if (plugin.Name == _instanceFromOtherAssembly.Name)
                        {
                            Logging.WriteError("A plugin with the same name is already started.");
                            return;
                        }
                    }
                    _threadName = "Plugin " + _instanceFromOtherAssembly.Name;
                    if (settingOnly && resetSettings)
                    {
                        _instanceFromOtherAssembly.ResetConfiguration();
                        return;
                    }
                    if (settingOnly)
                    {
                        _instanceFromOtherAssembly.ShowConfiguration();
                        return;
                    }
                    _worker = onlyCheckVersion
                        ? new Thread(_instanceFromOtherAssembly.CheckFields) {IsBackground = true, Name = _threadName}
                        : new Thread(_instanceFromOtherAssembly.Initialize) {IsBackground = true, Name = _threadName};
                    _worker.Start();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadPlugin(bool settingOnly = false, bool resetSettings = false, bool onlyCheckVersion = false): " + exception);
            }
        }

        public void DisposePlugin()
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
                Thread.Sleep(1000);
            }
            catch (Exception exception)
            {
                Logging.WriteError("DisposePlugin(): " + exception);
            }
            finally
            {
                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;
            }
        }

        public void ShowConfigurationPlugin()
        {
            try
            {
                Plugins.DisposePlugins();
                LoadPlugin(true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationPlugin(): " + exception);
            }
        }

        public void ResetConfigurationPlugin()
        {
            try
            {
                Plugins.DisposePlugins();
                LoadPlugin(true, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationPlugin(): " + exception);
            }
        }
    }
}