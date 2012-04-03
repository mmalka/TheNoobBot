using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.CSharp;
using WowManager;

namespace Questing_Bot.Bot.Tasks
{
    public class Script
    {
        internal static bool Run(string script)
        {
            try
            {
                if (script.Replace(" ", "") == string.Empty)
                    return true;

                if (script[0] == Convert.ToChar("="))
                {
                    script = WowManager.Others.Others.ReadFile(Application.StartupPath + "\\Products\\Questing Bot\\Profiles\\" + script.Replace("=", ""));
                }


                IScript _instanceFromOtherAssembly = null;
                Assembly _assembly = null;
                object _obj = null;


                CodeDomProvider cc = new CSharpCodeProvider();
                var cp = new CompilerParameters();
                cp.ReferencedAssemblies.Add("System.dll");
                cp.ReferencedAssemblies.Add("System.Xml.dll");
                cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");
                cp.ReferencedAssemblies.Add("WowManager.dll");
                cp.ReferencedAssemblies.Add("Products\\Questing Bot.dll");

                string toCompile = 
                "using System; " + Environment.NewLine +
                "using System.Windows.Forms; " + Environment.NewLine +
                "using WowManager; " + Environment.NewLine +

                "public class Main : Questing_Bot.Bot.Tasks.IScript " + Environment.NewLine +
                "{ " + Environment.NewLine +

                "    public bool Script() " + Environment.NewLine +
                "    { " + Environment.NewLine +
                "        try " + Environment.NewLine +
                "        { " + Environment.NewLine +
                " " + script + " " + Environment.NewLine +
                "        } " + Environment.NewLine +
                "        catch (Exception e) " + Environment.NewLine +
                "        { " + Environment.NewLine +
                "            WowManager.Log.AddLog(\"Error :\\n\" + e.ToString()); " + Environment.NewLine +
                "        } " + Environment.NewLine +
                "        return true; " + Environment.NewLine +
                "    } " + Environment.NewLine +

                "} " + Environment.NewLine;

                CompilerResults cr = cc.CompileAssemblyFromSource(cp, toCompile);
                if (cr.Errors.HasErrors)
                {
                    String text = "Compilator Error :\n";
                    foreach (CompilerError err in cr.Errors)
                        text += err + "\n";
                    Log.AddLog(text);
                    return true;
                }

                _assembly = cr.CompiledAssembly;

                _obj = _assembly.CreateInstance("Main", true);
                _instanceFromOtherAssembly = _obj as IScript;

                return _instanceFromOtherAssembly.Script();
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

