using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.CSharp;
using nManager.Helpful;

namespace Quester.Tasks
{
    public class Script
    {
        internal static bool Run(string script)
        {
            try
            {
                if (script.Replace(" ", "") == string.Empty)
                    return true;

                if (script[0] == '=')
                {
                    script = Others.ReadFile(Application.StartupPath + "\\Profiles\\Quester\\" + script.Replace("=", ""));
                    // this is for loading a file that will be added inside a method.
                }
                // or 1-line script directly from the field.

                // Example: "return (Usefuls.ContinentId == 1440);"


                CodeDomProvider cc = new CSharpCodeProvider();
                CompilerParameters cp = new CompilerParameters();
                cp.ReferencedAssemblies.Add("System.dll");
                cp.ReferencedAssemblies.Add("System.Xml.dll");
                cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");
                cp.ReferencedAssemblies.Add("nManager.dll");
                cp.ReferencedAssemblies.Add("Products\\Quester.dll");
                string toCompile =
                    "using System; " + Environment.NewLine +
                    "using System.Windows.Forms; " + Environment.NewLine +
                    "using nManager.Wow.Class; " + Environment.NewLine +
                    "using nManager.Helpful; " + Environment.NewLine +
                    "using nManager.Wow; " + Environment.NewLine +
                    "using nManager.Wow.Bot.Tasks; " + Environment.NewLine +
                    "using nManager.Wow.Enums; " + Environment.NewLine +
                    "using nManager.Wow.Helpers; " + Environment.NewLine +
                    "using nManager.Wow.ObjectManager; " + Environment.NewLine +
                    "public class Main : Quester.Tasks.IScript " + Environment.NewLine +
                    "{ " + Environment.NewLine +
                    "    public bool Script() " + Environment.NewLine +
                    "    { " + Environment.NewLine +
                    "        try " + Environment.NewLine +
                    "        { " + Environment.NewLine +
                    " " + script + " " + Environment.NewLine +
                    "        } " + Environment.NewLine +
                    "        catch (Exception e) " + Environment.NewLine +
                    "        { " + Environment.NewLine +
                    "            Logging.Write(\"Error :\\n\" + e.ToString()); " + Environment.NewLine +
                    "        } " + Environment.NewLine +
                    "        return true; " + Environment.NewLine +
                    "    } " + Environment.NewLine +
                    "} " + Environment.NewLine;

                CompilerResults cr = cc.CompileAssemblyFromSource(cp, toCompile);
                if (cr.Errors.HasErrors)
                {
                    String text = cr.Errors.Cast<CompilerError>().Aggregate("Compilator Error :\n", (current, err) => current + (err + "\n"));
                    Logging.Write(text);
                    return true;
                }

                Assembly assembly = cr.CompiledAssembly;

                object obj = assembly.CreateInstance("Main", true);
                IScript instanceFromOtherAssembly = obj as IScript;

                return instanceFromOtherAssembly != null && instanceFromOtherAssembly.Script();
            }
            catch
            {
            }

            return true;
        }
    }

    public interface IScript
    {
        #region Methods

        bool Script();

        #endregion Methods
    }
}