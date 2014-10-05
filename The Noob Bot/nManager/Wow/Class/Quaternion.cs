using System;
using nManager.Helpful;
using Math = System.Math;
using System.ComponentModel;

namespace nManager.Wow.Class
{
    /// <summary>
    /// Quaternion class
    /// </summary>
    [Serializable]
    public class Quaternion
    {
        /// <summary>
        /// Get the X Cartesian Rotation.
        /// </summary>
        /// <value>
        /// The X Cartesian Rotation.
        /// </value>
        public float X { get; private set; }

        /// <summary>
        /// Get the Y Cartesian Rotation.
        /// </summary>
        /// <value>
        /// The Y Cartesian Rotation.
        /// </value>
        public float Y { get; private set; }

        /// <summary>
        /// Get the Z Eucliden Rotation.
        /// </summary>
        /// <value>
        /// The Z Eucliden Rotation.
        /// </value>
        public float Z { get; private set; }

        /// <summary>
        /// Get the W Cartesian Rotation.
        /// </summary>
        /// <value>
        /// The W Cartesian Rotation.
        /// </value>
        public float W { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> class.
        /// </summary>
        /// <param name="x">The x Cartesian rotation.</param>
        /// <param name="y">The y Cartesian rotation.</param>
        /// <param name="z">The z Eucliden rotation.</param>
        /// <param name="w">The w Cartesian rotation.</param>
        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> class.
        /// </summary>
        /// <param name="w">The w Cartesian rotation.</param>
        /// <param name="v">The v 3D vector for axis of rotation.</param>
        public Quaternion(float w, Point v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> class from a packed int64.
        /// </summary>
        /// <param name="packedData">The packed int64 data.</param>
        public Quaternion(Int64 packedData)
        {
            const float a = 1/2097152.0f;
            const float b = 1/1048576.0f;

            X = (packedData >> 42)*a;
            Y = (packedData << 22 >> 43)*b;
            Z = (packedData << 43 >> 43)*b;

            double w = X*X + Y*Y + Z*Z;
            if (Math.Abs(w - 1.0) >= b)
                W = (float) Math.Sqrt(1.0 - w);
            else
                W = 0.0f;
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}, Z: {2}, W: {3}", X, Y, Z, W);
        }
    }
}