using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.CSharp;
using nManager.Helpful;
using nManager.Helpful.Interface;

namespace The_Noob_Bot
{
    internal static class ScriptOnlineManager
    {
        private static IScriptOnlineManager _instanceFromOtherAssembly;
        private static Assembly _assembly;
        private static object _obj;

        private static string Decrypt(string cipherString, int random)
        {
            try
            {
                const string UrlWebServer = "http://tech.thenoobbot.com/";
                const string ScriptServerMyIp = UrlWebServer + "myIp.php";

                // KeyDecrypt
                string ip = Others.GetRequest(ScriptServerMyIp, "null=null").Replace(".", "");
                int keyDecrypt = 0;
                for (int i = 0; i <= ip.Length - 1; i++)
                {
                    if (ip[i].ToString() == "0" || ip[i].ToString() == "1" || ip[i].ToString() == "2" ||
                        ip[i].ToString() == "3" || ip[i].ToString() == "4" || ip[i].ToString() == "5" ||
                        ip[i].ToString() == "6" || ip[i].ToString() == "7" || ip[i].ToString() == "8" ||
                        ip[i].ToString() == "9")
                        keyDecrypt += Others.ToInt32(ip[i].ToString());
                }


                string[] listByteS = cipherString.Split(Convert.ToChar(" "));
                var retList = new List<byte>();
                foreach (var s in listByteS)
                {
                    try
                    {
                        if (s != string.Empty)
                            retList.Add(Convert.ToByte(Convert.ToInt64(s) - keyDecrypt - (random*2)));
                    }
                    catch (Exception e)
                    {
                        Logging.WriteError("DVFrezfDFSESFfdgvbd#1: " + e);
                    }
                }

                return new UTF8Encoding().GetString(retList.ToArray());
            }
            catch (Exception e)
            {
                Logging.WriteError("DVFrezfDFSESFfdgvbd#2: " + e);
            }
            return "";
        }

        internal static void LoadScript()
        {
            try
            {
                const string httpAddress = "http://tech.thenoobbot.com/script.php";
                string sr;
                int random = Others.Random(0, 5000);
                try
                {
                    string srCrypted = Others.GetRequest(httpAddress, "r=" + random);
                    if (srCrypted.Replace(" ", "") == "")
                    {
                        Logging.WriteError("grzGRDSFfezfsgfvsdg server.");
                        return;
                    }
                    sr = Decrypt(srCrypted, random);
                    if (sr.Replace(" ", "") == string.Empty)
                    {
                        Logging.WriteError("grzGRDSFfezfsgfvsdg decrypt.");
                        return;
                    }
                }
                catch (Exception e)
                {
                    Logging.WriteError("grzGRDSFfezfsgfvsdg#1: " + e);
                    return;
                }


                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;


                CodeDomProvider cc = new CSharpCodeProvider();
                var cp = new CompilerParameters();
                var assemblies = AppDomain.CurrentDomain
                                          .GetAssemblies()
                                          .Where(
                                              a =>
                                              !a.IsDynamic &&
                                              !a.CodeBase.Contains((Process.GetCurrentProcess().ProcessName + ".exe")))
                                          .Select(a => a.Location);
                cp.ReferencedAssemblies.AddRange(assemblies.ToArray());
                string toCompile = sr;
                CompilerResults cr = cc.CompileAssemblyFromSource(cp, toCompile);
                if (cr.Errors.HasErrors)
                {
                    //String text = cr.Errors.Cast<CompilerError>().Aggregate("Compilator Error :\n", (current, err) => current + (err + "\n"));
                    //MessageBox.Show(text);
                    return;
                }

                _assembly = cr.CompiledAssembly;

                _obj = _assembly.CreateInstance("Main", true);
                _instanceFromOtherAssembly = _obj as IScriptOnlineManager;


                if (_instanceFromOtherAssembly != null)
                    _instanceFromOtherAssembly.Initialize();
                //else
                //Logging.WriteError("grzGRDSFfezfsgfvsdg error");
            }
            catch /*(Exception e)*/
            {
                //Logging.WriteError("grzGRDSFfezfsgfvsdg#2: " + e);
            }
        }
    }
}