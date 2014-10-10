using System;
using CSharpMath = System.Math;

namespace nManager.Wow.Class
{
    /// <summary>
    ///     Matrix4 class
    /// </summary>
    [Serializable]
    public class Matrix4
    {
        public Matrix4(MatrixX x, MatrixY y, MatrixZ z, MatrixW w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Matrix4(Matrix4 matrix)
        {
            X = matrix.X;
            Y = matrix.Y;
            Z = matrix.Z;
            W = matrix.W;
        }

        private Matrix4()
        {
            X = new MatrixX();
            Y = new MatrixY();
            Z = new MatrixZ();
            W = new MatrixW();
        }

        private MatrixX X { get; set; }

        public float xx
        {
            get { return X.xx; }
        }

        public float yx
        {
            get { return X.yx; }
        }

        public float zx
        {
            get { return X.zx; }
        }

        public float wx
        {
            get { return X.wx; }
        }

        private MatrixY Y { get; set; }

        public float xy
        {
            get { return Y.xy; }
        }

        public float yy
        {
            get { return Y.yy; }
        }

        public float zy
        {
            get { return Y.zy; }
        }

        public float wy
        {
            get { return Y.wy; }
        }

        public MatrixZ Z { get; set; }

        public float xz
        {
            get { return Z.xz; }
        }

        public float yz
        {
            get { return Z.yz; }
        }

        public float zz
        {
            get { return Z.zz; }
        }

        public float wz
        {
            get { return Z.wz; }
        }

        public MatrixW W { get; set; }

        public float xw
        {
            get { return W.xw; }
        }

        public float yw
        {
            get { return W.yw; }
        }

        public float zw
        {
            get { return W.zw; }
        }

        public float ww
        {
            get { return W.ww; }
        }

        public class MatrixW
        {
            public float ww;
            public float xw;
            public float yw;
            public float zw;

            public MatrixW()
            {
                xw = 0f;
                yw = 0f;
                zw = 0f;
                ww = 0f;
            }

            public MatrixW(float x, float y, float z, float w)
            {
                xw = x;
                yw = y;
                zw = z;
                ww = w;
            }

            public MatrixW(MatrixW matrix)
            {
                xw = matrix.xw;
                yw = matrix.yw;
                zw = matrix.zw;
                ww = matrix.ww;
            }
        }

        public class MatrixX
        {
            public float wx;
            public float xx;
            public float yx;
            public float zx;

            public MatrixX()
            {
                xx = 0f;
                yx = 0f;
                zx = 0f;
                wx = 0f;
            }

            public MatrixX(float x, float y, float z, float w)
            {
                xx = x;
                yx = y;
                zx = z;
                wx = w;
            }

            public MatrixX(MatrixX matrix)
            {
                xx = matrix.xx;
                yx = matrix.yx;
                zx = matrix.zx;
                wx = matrix.wx;
            }
        }

        public class MatrixY
        {
            public float wy;
            public float xy;
            public float yy;
            public float zy;

            public MatrixY()
            {
                xy = 0f;
                yy = 0f;
                zy = 0f;
                wy = 0f;
            }

            public MatrixY(float x, float y, float z, float w)
            {
                xy = x;
                yy = y;
                zy = z;
                wy = w;
            }

            public MatrixY(MatrixY matrix)
            {
                xy = matrix.xy;
                yy = matrix.yy;
                zy = matrix.zy;
                wy = matrix.wy;
            }
        }

        public class MatrixZ
        {
            public float wz;
            public float xz;
            public float yz;
            public float zz;

            public MatrixZ()
            {
                xz = 0f;
                yz = 0f;
                zz = 0f;
                wz = 0f;
            }

            public MatrixZ(float x, float y, float z, float w)
            {
                xz = x;
                yz = y;
                zz = z;
                wz = w;
            }

            public MatrixZ(MatrixZ matrix)
            {
                xz = matrix.xz;
                yz = matrix.yz;
                zz = matrix.zz;
                wz = matrix.wz;
            }
        }
    }
}