using System;
using System.Runtime.InteropServices;
using System.Text;

namespace nManager.Helpful
{
    /// <summary>
    /// Create a New INI file to store or load data
    /// </summary>
    public class IniFile
    {
        private string _path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>
        public IniFile(string iniPath)
        {
            try
            {
                _path = iniPath;
            }
            catch (Exception exception)
            {
                Logging.WriteError("IniFile(string iniPath): " + exception);
            }
        }

        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// Section name
        /// <PARAM name="Key"></PARAM>
        /// Key Name
        /// <PARAM name="Value"></PARAM>
        /// Value Name
        public void IniWriteValue(string section, string key, string value)
        {
            try
            {
                WritePrivateProfileString(section, key, value, _path);
            }
            catch (Exception exception)
            {
                Logging.WriteError("IniWriteValue(string section, string key, string value): " + exception);
            }
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <PARAM name="Key"></PARAM>
        /// <PARAM name="Path"></PARAM>
        /// <returns></returns>
        public string IniReadValue(string section, string key)
        {
            try
            {
                var temp = new StringBuilder(255);
                GetPrivateProfileString(section, key, "", temp,
                                        255, _path);
                return temp.ToString();
            }
            catch (Exception exception)
            {
                Logging.WriteError("IniReadValue(string section, string key): " + exception);
            }
            return "";
        }
    }
}
