using System;
using nManager.Helpful;
using Math = System.Math;
using System.ComponentModel;

namespace nManager.Wow.Class
{
    /// <summary>
    /// Point class
    /// </summary>
    [Serializable]
    public class Point : Vector3
    {
        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (Type != null ? Type.GetHashCode() : 0);
            }
        }

        [DefaultValue("None")]
        public string Type { get; set; }

        public Point()
        {
            X = 0;
            Y = 0;
            Z = 0;
            Type = "None";
        }

        public Point(float x, float y, float z, string type = "None")
        {
            X = x;
            Y = y;
            Z = z;
            Type = type;
        }

        public Point(float[] array)
        {
            X = array[0];
            Y = array[1];
            Z = array[2];
            Type = "None";
        }

        public Point(Point other)
        {
            X = other.X;
            Y = other.Y;
            Z = other.Z;
            Type = other.Type;
        }

        public Point(Vector3 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            Type = "None";
        }

        public Point(string v)
        {
            string[] value = v.Split(';');
            if (!v.Contains(";") || value.Length < 3)
            {
                X = 0f;
                Y = 0f;
                Z = 0f;
            }
            else
            {
                X = Others.ToSingle(value[0]);
                Y = Others.ToSingle(value[1]);
                Z = Others.ToSingle(value[2]);
                if (X == 0 || Y == 0 || Z == 0)
                {
                    X = 0f;
                    Y = 0f;
                    Z = 0f;
                }
            }
            if (value.Length >= 4)
            {
                switch (value[3].ToLower())
                {
                    case "swimming":
                        Type = "Swimming";
                        break;
                    case "flying":
                        Type = "Flying";
                        break;
                    default:
                        Type = "None";
                        break;
                }
            }
            else
                Type = "None";
        }

        public override string ToString()
        {
            return string.Format("{0} ; {1} ; {2} ; {3}", X, Y, Z, Type);
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
                return base.Equals(obj as Point);
            return false;
        }

        public bool Equals(Point obj)
        {
            //Logging.Write("Test Equals, X = " + Math.Abs(X) + ", obj.X = " + Math.Abs(obj.X) + ", Math.Abs(X-obj.X) = " + Math.Abs(X - obj.X));
            if (Math.Abs(X - obj.X) > 0.001)
                return false;
            if (Math.Abs(Y - obj.Y) > 0.001)
                return false;
            if (Math.Abs(Z - obj.Z) > 0.001)
                return false;
            if (Type != obj.Type)
                return false;
            return true;
        }
    }

    /*
    /// <summary>
    /// Point class
    /// </summary>
    [Serializable]
    public class Point
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> class.
        /// </summary>
        public Point()
        {
            X = 0;
            Y = 0;
            Z = 0;
            _type = "None";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        /// <param name="type">Type (Flying, None) </param>
        public Point(float x, float y, float z, string type = "None")
        {
            try
            {
                X = x;
                Y = y;
                Z = z;
                _type = type;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Point(float x, float y, float z): " + exception);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> class.
        /// </summary>
        /// <param name="point">The point.</param>
        public Point(Point point)
        {
            try
            {
                X = point.X;
                Y = point.Y;
                Z = point.Z;
                _type = point.Type;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Point(Point point): " + exception);
            }
        }

        /// <summary>
        /// Gets or sets the X Position.
        /// </summary>
        /// <value>
        /// The X Position.
        /// </value>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the Y Position.
        /// </summary>
        /// <value>
        /// The Y Position.
        /// </value>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the Z Position.
        /// </summary>
        /// <value>
        /// The Z Position.
        /// </value>
        public float Z { get; set; }

        private string _type = "None";

        /// <summary>
        /// Point Type.
        /// </summary>
        /// <value>
        /// Point Type.
        /// </value>
        [DefaultValue("None")]
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public bool IsValid
        {
            get { return (X != 0 || Y != 0 || Z != 0); }
        }

        public override string ToString()
        {
            try
            {
                return string.Format("{0} ; {1} ; {2} ; {3}", X, Y, Z, Type);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Point > ToString(): " + exception);
            }
            return "";
        }

        public Point InFrontOf(float heading, float d)
        {
            try
            {
                float nx = X + (float) Math.Cos(heading)*d;
                float ny = Y + (float) Math.Sin(heading)*d;
                float nz = Z;
                return new Point(nx, ny, nz);
            }
            catch (Exception exception)
            {
                Logging.WriteError("InFrontOf(float heading, float d): " + exception);
            }
            return new Point();
        }

        public float Size()
        {
            try
            {
                return (float) Math.Sqrt(X*X + Y*Y + Z*Z);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Point > Size(): " + exception);
            }
            return 0;
        }

        public float Size2D()
        {
            try
            {
                return (float) Math.Sqrt(X*X + Y*Y);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Point > Size2D(): " + exception);
            }
            return 0;
        }

        public float DistanceZ(Point a, Point b)
        {
            try
            {
                float disZTemp = a.Z - b.Z;
                if (disZTemp < 0)
                {
                    disZTemp = -1*disZTemp;
                }
                return disZTemp;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Point > DistanceZ(Point a, Point b): " + exception);
            }
            return 0;
        }

        public float DistanceZ(Point b)
        {
            try
            {
                float disZTemp = Z - b.Z;
                if (disZTemp < 0)
                {
                    disZTemp = -1*disZTemp;
                }
                return disZTemp;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Point > DistanceZ(Point b): " + exception);
            }
            return 0;
        }

        public float DistanceTo(Point b)
        {
            try
            {
                return (this - b).Size();
            }
            catch (Exception exception)
            {
                Logging.WriteError("Point > DistanceTo(Point b): " + exception);
            }
            return 0;
        }

        public float DistanceTo2D(Point b)
        {
            try
            {
                return (this - b).Size2D();
            }
            catch (Exception exception)
            {
                Logging.WriteError("Point > DistanceTo2D(Point b): " + exception);
            }
            return 0;
        }

        public static float Distance(Point a, Point b)
        {
            try
            {
                return (a - b).Size();
            }
            catch (Exception exception)
            {
                Logging.WriteError("Point > Distance(Point a, Point b): " + exception);
            }
            return 0;
        }

        public static float Distance2D(Point a, Point b)
        {
            try
            {
                return (a - b).Size2D();
            }
            catch (Exception exception)
            {
                Logging.WriteError("Point > Distance2D(Point a, Point b): " + exception);
            }
            return 0;
        }

        public static Point operator -(Point a, Point b)
        {
            try
            {
                return new Point(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Point > -(Point a, Point b): " + exception);
            }
            return new Point();
        }

        public static Point operator +(Point a, Point b)
        {
            try
            {
                return new Point(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Point > +(Point a, Point b): " + exception);
            }
            return new Point();
        }

        public static Point operator *(Point a, Point b)
        {
            try
            {
                return new Point(a.X*b.X, a.Y*b.Y, a.Z*b.Z);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Point > *(Point a, Point b): " + exception);
            }
            return new Point();
        }

        public static Point operator /(Point a, Point b)
        {
            try
            {
                return new Point(a.X/b.X, a.Y/b.Y, a.Z/b.Z);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Point >  /(Point a, Point b): " + exception);
            }
            return new Point();
        }

        public static Point operator *(Point a, float b)
        {
            try
            {
                return new Point(a.X*b, a.Y*b, a.Z*b);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Point > *(Point a, float b): " + exception);
            }
            return new Point();
        }

        public static Point operator /(Point a, float b)
        {
            try
            {
                return new Point(a.X/b, a.Y/b, a.Z/b);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Point > /(Point a, float b): " + exception);
            }
            return new Point();
        }
    }*/
}