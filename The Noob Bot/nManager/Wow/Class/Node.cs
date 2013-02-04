using System;

namespace nManager.Wow.Class
{
    /// <summary>
    /// Node Class info
    /// </summary>
    [Serializable]
    public class Node
    {
        private int _id;
        private string _type = "";
        private string _name = "";
        private int _skill;
        private bool _actived = true;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int Skill
        {
            get { return _skill; }
            set { _skill = value; }
        }

        public bool Actived
        {
            get { return _actived; }
            set { _actived = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}