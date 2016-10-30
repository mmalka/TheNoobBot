using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.CSharp;
using nManager.Helpful;
using Quester.Profile;

namespace Quester.Tasks
{
    public class Script
    {
        internal static Dictionary<string, IScript> CachedScripts = new Dictionary<string, IScript>();

        internal static bool Run(string script)
        {
            var qO = new QuestObjective();
            return Run(script, 0, ref qO);
        }

        internal static bool Run(string script, int questId, ref QuestObjective qObjective)
        {
            try
            {
                if (script.Replace(" ", "") == string.Empty)
                    return true;

                string originalScript = script;
                if (CachedScripts.ContainsKey(originalScript) && CachedScripts[originalScript] != null)
                    return CachedScripts[originalScript].Script(ref qObjective);

                if (script[0] == '=')
                {
                    script = Others.ReadFile(Application.StartupPath + "\\Profiles\\Quester\\Scripts\\" + script.Replace("=", ""), true);
                    // this is for loading a file that will be added inside a method.
                }
                // or 1-line script directly from the field.
                // Example: "return (Usefuls.ContinentId == 1440);"

                CodeDomProvider cc = new CSharpCodeProvider();
                var cp = new CompilerParameters();

                IEnumerable<string> assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic && !a.CodeBase.Contains((Process.GetCurrentProcess().ProcessName + ".exe"))).Select(a => a.Location);
                cp.ReferencedAssemblies.AddRange(assemblies.ToArray());
                cp.ReferencedAssemblies.Add("nManager.dll");
                cp.ReferencedAssemblies.Add("System.dll");
                cp.ReferencedAssemblies.Add("System.Linq.dll");
                cp.ReferencedAssemblies.Add("System.Xml.dll");
                cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");
                cp.ReferencedAssemblies.Add("nManager.dll");
                cp.ReferencedAssemblies.Add("Products\\Quester.dll");
                string toCompile =
                    "using System; " + Environment.NewLine +
                    "using System.Threading; " + Environment.NewLine +
                    "using System.Windows.Forms; " + Environment.NewLine +
                    "using System.Collections.Generic; " + Environment.NewLine +
                    "using System.Linq; " + Environment.NewLine +
                    "using nManager; " + Environment.NewLine +
                    "using nManager.Wow.Class; " + Environment.NewLine +
                    "using nManager.Helpful; " + Environment.NewLine +
                    "using nManager.Wow; " + Environment.NewLine +
                    "using nManager.Wow.Bot.Tasks; " + Environment.NewLine +
                    "using nManager.Wow.Enums; " + Environment.NewLine +
                    "using nManager.Wow.Helpers; " + Environment.NewLine +
                    "using nManager.Wow.ObjectManager; " + Environment.NewLine +
                    "using Quester.Profile; " + Environment.NewLine +
                    "namespace Quester.Tasks " + Environment.NewLine +
                    "{ " + Environment.NewLine +
                    "public class Main : Quester.Tasks.IScript " + Environment.NewLine +
                    "{ " + Environment.NewLine +
                    "    public bool Script(ref QuestObjective questObjective) " + Environment.NewLine +
                    "    { " + Environment.NewLine +
                    "        try " + Environment.NewLine +
                    "        { " + Environment.NewLine +
                    "           int QuestID = " + questId + "; " + Environment.NewLine +
                    " " + script + " " + Environment.NewLine +
                    "        } " + Environment.NewLine +
                    "        catch (Exception e) " + Environment.NewLine +
                    "        { " + Environment.NewLine +
                    "            Logging.Write(\"Error :\\n\" + e.ToString()); " + Environment.NewLine +
                    "        } " + Environment.NewLine +
                    "        return true; " + Environment.NewLine +
                    "    } " + Environment.NewLine +
                    "} " + Environment.NewLine +
                    "} " + Environment.NewLine;
                Logging.WriteDebug(toCompile);

                CompilerResults cr = cc.CompileAssemblyFromSource(cp, toCompile);
                if (cr.Errors.HasErrors)
                {
                    String text = cr.Errors.Cast<CompilerError>().Aggregate("Compilator Error :\n", (current, err) => current + (err + "\n"));
                    Logging.Write(text);
                    return true;
                }

                Assembly assembly = cr.CompiledAssembly;

                object obj = assembly.CreateInstance("Main", true);
                var instanceFromOtherAssembly = obj as IScript;

                CachedScripts.Add(originalScript, instanceFromOtherAssembly);

                return instanceFromOtherAssembly != null && CachedScripts[originalScript].Script(ref qObjective);
            }
            catch
            {
            }

            return true;
        }
    }

    public class Main : IScript
    {
        public bool Script(ref QuestObjective questObjective)
        {
            return false;
        }
    }

    public interface IScript
    {
        #region Methods

        bool Script(ref QuestObjective questObjective);

        #endregion Methods
    }
}