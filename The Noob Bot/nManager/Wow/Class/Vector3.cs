using System;
using System.Xml.Serialization;
using CSharpMath = System.Math;
using System.ComponentModel;

namespace nManager.Wow.Class
{
    /// <summary>
    /// Vector3 class
    /// </summary>
    [Serializable]
    public class Vector3 : IComparable, IComparable<Vector3>, IEquatable<Vector3>, IFormattable
    {
        /// <summary>
        /// Get or set X.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Get or set Y.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Get or set Z.
        /// </summary>
        public float Z { get; set; }

        public Vector3()
        {
            X = 0f;
            Y = 0f;
            Z = 0f;
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(Vector3 other)
        {
            X = other.X;
            Y = other.Y;
            Z = other.Z;
        }

        [XmlIgnore]
        public float[] Array
        {
            get { return new float[] {X, Y, Z}; }
            set
            {
                if (value.Length == 3)
                {
                    X = value[0];
                    Y = value[1];
                    Z = value[2];
                }
                else
                {
                    throw new ArgumentException("Array must contain exactly three values: [x,y,z]");
                }
            }
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                    {
                        return X;
                    }
                    case 1:
                    {
                        return Y;
                    }
                    case 2:
                    {
                        return Z;
                    }
                    default:
                        throw new ArgumentException("Array must contain exactly three values: [x,y,z]", "index");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                    {
                        X = value;
                        break;
                    }
                    case 1:
                    {
                        Y = value;
                        break;
                    }
                    case 2:
                    {
                        Z = value;
                        break;
                    }
                    default:
                        throw new ArgumentException("Array must contain exactly three values: [x,y,z]", "index");
                }
            }
        }

        [XmlIgnore]
        public float Magnitude
        {
            get { return (float) CSharpMath.Sqrt(X*X + Y*Y + Z*Z); }
            set
            {
                if (value < 0 || this == origin)
                    return;
                Vector3 v = this*(value/Magnitude);
                X = v.X;
                Y = v.Y;
                Z = v.Z;
            }
        }

        [XmlIgnore]
        public bool IsValid
        {
            get { return this != Vector3.origin; }
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            if ((object) v1 == null) return ((object) v2 == null);
            if ((object) v2 == null) return ((object) v1 == null);
            return
                (
                    (v1.X == v2.X) &&
                    (v1.Y == v2.Y) &&
                    (v1.Z == v2.Z)
                    );
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !(v1 == v2);
        }

        public static bool operator <(Vector3 v1, Vector3 v2)
        {
            return v1.Magnitude < v2.Magnitude;
        }

        public static bool operator >(Vector3 v1, Vector3 v2)
        {
            return v1.Magnitude > v2.Magnitude;
        }

        public static bool operator <=(Vector3 v1, Vector3 v2)
        {
            return v1.Magnitude <= v2.Magnitude;
        }

        public static bool operator >=(Vector3 v1, Vector3 v2)
        {
            return v1.Magnitude >= v2.Magnitude;
        }

        public static Vector3 operator *(Vector3 v1, float s2)
        {
            return
                (
                    new Vector3
                        (
                        v1.X*s2,
                        v1.Y*s2,
                        v1.Z*s2
                        )
                    );
        }

        public static Vector3 operator *(float s1, Vector3 v2)
        {
            return v2*s1;
        }

        public static Vector3 operator /(Vector3 v1, float s2)
        {
            return
                (
                    new Vector3
                        (
                        v1.X/s2,
                        v1.Y/s2,
                        v1.Z/s2
                        )
                    );
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return
                (
                    new Vector3
                        (
                        v1.X + v2.X,
                        v1.Y + v2.Y,
                        v1.Z + v2.Z
                        )
                    );
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return
                (
                    new Vector3
                        (
                        v1.X - v2.X,
                        v1.Y - v2.Y,
                        v1.Z - v2.Z
                        )
                    );
        }

        public static Vector3 operator -(Vector3 v1)
        {
            return
                (
                    new Vector3
                        (
                        -v1.X,
                        -v1.Y,
                        -v1.Z
                        )
                    );
        }

        public static Vector3 operator +(Vector3 v1)
        {
            return
                (
                    new Vector3
                        (
                        +v1.X,
                        +v1.Y,
                        +v1.Z
                        )
                    );
        }

        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            return
                (
                    new Vector3
                        (
                        v1.Y*v2.Z - v1.Z*v2.Y,
                        v1.Z*v2.X - v1.X*v2.Z,
                        v1.X*v2.Y - v1.Y*v2.X
                        )
                    );
        }

        public Vector3 Cross(Vector3 other)
        {
            return Cross(this, other);
        }

        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return
                (
                    v1.X*v2.X +
                    v1.Y*v2.Y +
                    v1.Z*v2.Z
                    );
        }

        public float Dot(Vector3 other)
        {
            return Dot(this, other);
        }

        public static bool IsUnitVector(Vector3 v1)
        {
            return v1.Magnitude == 1;
        }

        public bool IsUnitVector()
        {
            return IsUnitVector(this);
        }

        public static float Abs(Vector3 v1)
        {
            return v1.Magnitude;
        }

        public static float Distance(Vector3 v1, Vector3 v2)
        {
            return
                (float) (
                    Math.Sqrt
                        (
                            (v1.X - v2.X)*(v1.X - v2.X) +
                            (v1.Y - v2.Y)*(v1.Y - v2.Y) +
                            (v1.Z - v2.Z)*(v1.Z - v2.Z)
                        )
                    );
        }

        public float DistanceTo(Vector3 other)
        {
            return Distance(this, other);
        }

        public static float DistanceTo2D(Vector3 v1, Vector3 v2)
        {
            return
                (float) (
                    Math.Sqrt
                        (
                            (v1.X - v2.X)*(v1.X - v2.X) +
                            (v1.Y - v2.Y)*(v1.Y - v2.Y)
                        )
                    );
        }

        public float DistanceTo2D(Vector3 other)
        {
            return DistanceTo2D(this, other);
        }

        public static float DistanceZ(Vector3 v1, Vector3 v2)
        {
            return Math.Abs(v1.Z - v2.Z);
        }

        public float DistanceZ(Vector3 other)
        {
            return DistanceZ(this, other);
        }

        /// <summary>
        /// Return the angle between the 2 2D (X, Y) vectors in radian from -PI to +PI.
        /// </summary>
        public static float Angle2D(Vector3 v1, Vector3 v2)
        {
            double angle = CSharpMath.Atan2(v2.Y, v2.X) - CSharpMath.Atan2(v1.Y, v1.X);
            if (angle > CSharpMath.PI)
                angle -= (CSharpMath.PI*2.0);
            else if (angle < (0.0 - CSharpMath.PI))
                angle += (CSharpMath.PI*2.0);
            return (float) angle;
        }

        /// <summary>
        /// Return the 2D (X, Y) angle between current vector and the one in parameter in radian from -PI to +PI.
        /// </summary>
        public float Angle2D(Vector3 other)
        {
            return Angle2D(this, other);
        }

        public static Vector3 Normalize(Vector3 v1)
        {
            // Check division by zero
            if (v1.Magnitude == 0)
            {
                throw new DivideByZeroException("Cannot normalize a vector of magnitude zero");
            }
            else
            {
                float inverse = 1/v1.Magnitude;
                return
                    (
                        new Vector3
                            (
                            v1.X*inverse,
                            v1.Y*inverse,
                            v1.Z*inverse
                            )
                        );
            }
        }

        public void Normalize()
        {
            Vector3 v = Normalize(this);
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public static Vector3 Pitch(Vector3 v1, float degree)
        {
            float x = v1.X;
            float y = (v1.Y*(float) CSharpMath.Cos(degree)) - (v1.Z*(float) CSharpMath.Sin(degree));
            float z = (v1.Y*(float) CSharpMath.Sin(degree)) + (v1.Z*(float) CSharpMath.Cos(degree));
            return new Vector3(x, y, z);
        }

        public static Vector3 Yaw(Vector3 v1, float degree)
        {
            float x = (v1.Z*(float) CSharpMath.Sin(degree)) + (v1.X*(float) CSharpMath.Cos(degree));
            float y = v1.Y;
            float z = (v1.Z*(float) CSharpMath.Cos(degree)) - (v1.X*(float) CSharpMath.Sin(degree));
            return new Vector3(x, y, z);
        }

        public void Yaw(float degree)
        {
            Vector3 v = Yaw(this, degree);
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public static Vector3 Roll(Vector3 v1, float degree)
        {
            float x = (v1.X*(float) CSharpMath.Cos(degree)) - (v1.Y*(float) CSharpMath.Sin(degree));
            float y = (v1.X*(float) CSharpMath.Sin(degree)) + (v1.Y*(float) CSharpMath.Cos(degree));
            float z = v1.Z;
            return new Vector3(x, y, z);
        }

        public void Roll(float degree)
        {
            Vector3 v = Roll(this, degree);
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public static readonly Vector3 origin = new Vector3(0, 0, 0);
        public static readonly Vector3 xAxis = new Vector3(1, 0, 0);
        public static readonly Vector3 yAxis = new Vector3(0, 1, 0);
        public static readonly Vector3 zAxis = new Vector3(0, 0, 1);

        public Vector3 Transform(Matrix4 matrix)
        {
            var v = new Vector3();
            v.X = X * matrix.xx + Y * matrix.xy + Z * matrix.xz + matrix.xw;
            v.Y = X * matrix.yx + Y * matrix.yy + Z * matrix.yz + matrix.yw;
            v.Z = X * matrix.zx + Y * matrix.zy + Z * matrix.zz + matrix.ww;
            return v;
        }

        // *****************************************
        // C# stuff
        // *****************************************
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            Vector3 other = obj as Vector3;
            return other == this;
        }

        public bool Equals(Vector3 other)
        {
            return other == this;
        }

        public override int GetHashCode()
        {
            return ((X.GetHashCode()*-17) + (Y.GetHashCode()*7) + (Z.GetHashCode()*3))%Int32.MaxValue;
        }

        public int CompareTo(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                throw new ArgumentException("Cannot compare a Vector to a non-Vector and you passed a " + obj.GetType().ToString());
            Vector3 other = obj as Vector3;
            if (this < other)
            {
                return -1;
            }
            else if (this > other)
            {
                return 1;
            }
            return 0;
        }

        public int CompareTo(Vector3 other)
        {
            if (this < other)
            {
                return -1;
            }
            else if (this > other)
            {
                return 1;
            }
            return 0;
        }

        public override string ToString()
        {
            return String.Format("({0}, {1}, {2})", X, Y, Z);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            // If no format given
            if (format == null || format == "")
                return String.Format("({0}, {1}, {2})", X, Y, Z);

            char firstChar = format[0];
            string remainder = null;

            if (format.Length > 1)
                remainder = format.Substring(1);
            switch (firstChar)
            {
                case 'x':
                    return X.ToString(remainder, formatProvider);
                case 'y':
                    return Y.ToString(remainder, formatProvider);
                case 'z':
                    return Z.ToString(remainder, formatProvider);
                default:
                    return
                        String.Format
                            (
                                "({0}, {1}, {2})",
                                X.ToString(format, formatProvider),
                                Y.ToString(format, formatProvider),
                                Z.ToString(format, formatProvider)
                            );
            }
        }
    }
}