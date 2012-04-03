using System;

namespace nManager.Wow.Class
{
    [Serializable]
    public class Digsite
    {
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        public uint continentId
        {
            get { return _continentId; }
            set { _continentId = value; }
        }
        public Point position
        {
            get { return _position; }
            set { _position = value; }
        }
        public string px
        {
            get { return _px; }
            set { _px = value; }
        }
        public string py
        {
            get { return _py; }
            set { _py = value; }
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

        string _name;
        uint _continentId;
        Point _position;
        string _px;
        string _py;
        float _priorityDigsites;
        bool _active;
    }
}
