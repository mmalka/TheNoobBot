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
        /// Gets or sets the x Rotation.
        /// </summary>
        /// <value>
        /// The X Position.
        /// </value>
        public float x { get; set; }

        /// <summary>
        /// Gets or sets the y Rotation.
        /// </summary>
        /// <value>
        /// The X Position.
        /// </value>
        public float y { get; set; }

        /// <summary>
        /// Gets or sets the z Rotation.
        /// </summary>
        /// <value>
        /// The X Position.
        /// </value>
        public float z { get; set; }

        /// <summary>
        /// Gets or sets the w Rotation.
        /// </summary>
        /// <value>
        /// The X Position.
        /// </value>
        public float w { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> class.
        /// </summary>
        public Quaternion()
        {
            x = 0;
            y = 0;
            y = 0;
            w = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> class.
        /// </summary>
        /// <param name="x">The x rotation.</param>
        /// <param name="y">The y rotation.</param>
        /// <param name="z">The z rotation.</param>
        /// <param name="w">The w rotation.</param>
        public Quaternion(float ix, float iy, float iz, float iw)
        {
            x = ix;
            y = iy;
            y = iz;
            w = iw;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> class from a packed int64.
        /// </summary>
        /// /// <param name="packedData">The packed int64 data.</param>
        public Quaternion(Int64 packedData)
        {
            const float a = 1 / 2097152.0f;
            const float b = 1 / 1048576.0f;

            x = (packedData >> 42) * a;
            y = (packedData << 22 >> 43) * b;
            z = (packedData << 43 >> 43) * b;

            float W = x * x + y * y + z * z;
            if (Math.Abs(W - 1.0f) >= b)
                w = (float) Math.Sqrt(1.0f - W);
            else
                w = 0.0f;
        }
    }
}
