using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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
        public static Dictionary<string, IScript> CachedScripts = new Dictionary<string, IScript>();

        internal static bool Run(string script, int questId = 0)
        {
            if (string.IsNullOrWhiteSpace(script))
                return true;
            var qO = new QuestObjective();
            return Run(script, 0, ref qO);
        }

        internal static bool Run(string script, int questId, ref QuestObjective qObjective)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(script))
                    return true;

                string originalScript = script;
                if (CachedScripts.ContainsKey(originalScript) && CachedScripts[originalScript] != null)
                    return CachedScripts[originalScript].Script(ref qObjective);

                string scriptName = script; // Either the full script if it's a single line script, or the file name.
                if (script[0] == '=')
                {
                    scriptName = script.Replace("=", "");
                    script = Others.ReadFile(Application.StartupPath + "\\Profiles\\Quester\\Scripts\\" + script.Replace("=", ""), true);
                    // this is for loading a file that will be added inside a method.
                }
                // or 1-line script directly from the field.
                // Example: "return (Usefuls.ContinentId == 1440);"

                CodeDomProvider cc = new CSharpCodeProvider();
                var cp = new CompilerParameters();
                cp.ReferencedAssemblies.Add("System.dll");
                cp.ReferencedAssemblies.Add("System.Numerics.dll");
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
                    "using System.Diagnostics; " + Environment.NewLine +
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
                    "using Timer = nManager.Helpful.Timer;" + Environment.NewLine +
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
                    "} " + Environment.NewLine;
                //Logging.WriteDebug(toCompile);

                CompilerResults cr = cc.CompileAssemblyFromSource(cp, toCompile);
                if (cr.Errors.HasErrors)
                {
                    String text = cr.Errors.Cast<CompilerError>().Aggregate("Compilator Error :\n", (current, err) => current + (err + "\n"));
                    Logging.Write("Script: " + scriptName + " " + text);
                    return true;
                }

                Assembly assembly = cr.CompiledAssembly;

                object obj = assembly.CreateInstance("Main", true);
                var instanceFromOtherAssembly = obj as IScript;

                CachedScripts.Add(originalScript, instanceFromOtherAssembly);

                return instanceFromOtherAssembly != null && CachedScripts[originalScript].Script(ref qObjective);
            }
            catch (Exception e)
            {
                Logging.WriteError("internal static bool Run(string script, int questId, ref QuestObjective qObjective): " + e);
            }

            return true;
        }
    }

    /*public class Main : IScript
    {
        public bool Script(ref QuestObjective questObjective)
        {
            return false;
        }
    }*/

    public interface IScript
    {
        #region Methods

        bool Script(ref QuestObjective questObjective);

        #endregion Methods
    }
}