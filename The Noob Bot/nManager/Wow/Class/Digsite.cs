using System;

namespace nManager.Wow.Class
{
    [Serializable]
    public class Digsite
    {
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        public float PriorityDigsites
        {
            get { return _priorityDigsites; }
            set { _priorityDigsites = value; }
        }

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        private int _id;
        private string _name;
        private float _priorityDigsites;
        private bool _active;
    }
}