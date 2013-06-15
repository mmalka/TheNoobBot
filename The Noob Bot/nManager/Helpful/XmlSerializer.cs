using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace nManager.Helpful
{
    /// <summary>
    /// XmlSerializer
    /// </summary>
    public static class XmlSerializer
    {
        /// <summary>
        /// Serializes the specified Class to XML.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="object">The @object.</param>
        /// <returns></returns>
        public static Boolean Serialize(String path, object @object)
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
            FileStream fs = null;
            try
            {
                using (fs = new FileStream(path, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(@object.GetType());
                        s.Serialize(w, @object);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    if (fs != null)
                        fs.Close();
                }
                catch
                {
                }
                Logging.WriteError("Serialize(String path, object @object)#2: " + ex);
                MessageBox.Show("XML Serialize: " + ex);
            }

            return false;
        }

        /// <summary>
        /// Deserializes the XML to Class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static T Deserialize<T>(String path)
        {
            if (!File.Exists(path))
                return default(T);

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(typeof (T));
            T result = (T) s.Deserialize(fs);
            fs.Close();
            return result;
        }
    }
}