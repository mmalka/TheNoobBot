using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;

namespace nManager.Wow.Helpers
{
    /// <summary>
    /// Nodes.xml Manager
    /// </summary>
    public class NodesList
    {
        private static List<Node> _nodes;
        /// <summary>
        /// Gets the list of nodes Id.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="skill">The skill.</param>
        /// <returns></returns>
        public static List<int> GetListId(string type, int skill)
        {
            try
            {
                lock ("NodesList")
                {
                    if (_nodes == null)
                        _nodes = LoadList();

                    return (from nT in _nodes where nT.Skill <= skill && nT.Type == type && nT.Actived && !nManagerSetting.CurrentSetting.blackListHarvest.Contains(nT.Id) select nT.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("NodesList > GetListId(string type, int skill): " + ex);
                return new List<int>();
            }
        }

        public static string GetNameById(int id)
        {
            try
            {
                lock ("NodesList")
                {
                    if (_nodes == null)
                        _nodes = LoadList();

                    foreach (var node in _nodes.Where(node => node.Id == id))
                    {
                        return node.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("NodesList > GetNameById(int id): " + ex);
            }
            return "";
        }

        /// <summary>
        /// Loads the list of nodes Nodes.xml .
        /// </summary>
        /// <returns></returns>
        public static List<Node> LoadList()
        {
            try
            {
                var nodes = XmlSerializer.Deserialize<List<Node>>(Application.StartupPath + "\\Data\\Nodes.xml");
                return nodes;
            }
            catch (Exception ex)
            {
                Logging.WriteError("NodesList > LoadList(): " + ex);
                return new List<Node>();
            }
        }

        /// <summary>
        /// Saves the list.
        /// </summary>
        /// <returns></returns>
        public static void SaveList(List<Node> nodes)
        {
            try
            {
                lock ("NodesList")
                {
                    XmlSerializer.Serialize(Application.StartupPath + "\\Data\\Nodes.xml", nodes);
                    _nodes = null;
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("NodesList > SaveList(List<Node> nodes): " + ex);
            }
        }
    }
}
