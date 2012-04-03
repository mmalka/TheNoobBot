using System;
using System.Reflection;
using System.Windows.Forms;
using WowManager.Others;

namespace Battleground_Bot.API
{
    internal class Plugins
    {
        public static IPlugins LoadPlugin(string nameDll)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(Others.GetCurrentDirectory + "\\Products\\Battleground Bot\\Plugins\\" + nameDll +
                                                      ".dll");
                object obj = assembly.CreateInstance("Main", true);
                var instanceFromOtherAssembly = obj as IPlugins;
                if (instanceFromOtherAssembly != null)
                {
                    instanceFromOtherAssembly.Name = nameDll;
                    return instanceFromOtherAssembly;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e);
                return null;
            }
            return null;
        }
    }
}