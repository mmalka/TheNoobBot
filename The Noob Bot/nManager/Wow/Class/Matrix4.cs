using System;
using System.Runtime.InteropServices;
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
            set { X.xx = value; }
        }

        public float yx
        {
            get { return X.yx; }
            set { X.yx = value; }
        }

        public float zx
        {
            get { return X.zx; }
            set { X.zx = value; }
        }

        public float wx
        {
            get { return X.wx; }
            set { X.wx = value; }
        }

        private MatrixY Y { get; set; }

        public float xy
        {
            get { return Y.xy; }
            set { Y.xy = value; }
        }

        public float yy
        {
            get { return Y.yy; }
            set { Y.yy = value; }
        }

        public float zy
        {
            get { return Y.zy; }
            set { Y.zy = value; }
        }

        public float wy
        {
            get { return Y.wy; }
            set { Y.wy = value; }
        }

        public MatrixZ Z { get; set; }

        public float xz
        {
            get { return Z.xz; }
            set { Z.xz = value; }
        }

        public float yz
        {
            get { return Z.yz; }
            set { Z.yz = value; }
        }

        public float zz
        {
            get { return Z.zz; }
            set { Z.zz = value; }
        }

        public float wz
        {
            get { return Z.wz; }
            set { Z.wz = value; }
        }

        public MatrixW W { get; set; }

        public float xw
        {
            get { return W.xw; }
            set { W.xw = value; }
        }

        public float yw
        {
            get { return W.yw; }
            set { W.yw = value; }
        }

        public float zw
        {
            get { return W.zw; }
            set { W.zw = value; }
        }

        public float ww
        {
            get { return W.ww; }
            set { W.ww = value; }
        }

        public override string ToString()
        {
            return string.Format("xx={0}, yx={1}, zx={2}, wx={3}, xy={4}, yy={5}, zy={6}, wy={7}, xz={8}, yz={9}, zz={10}, wz={11}, xw={12}, yw={13}, zw={14}, ww={15}", xx, yx, zx, wx, xy, yy, zy, wy, xz, yz, zz, wz,
                xw, yw, zw, ww);
        }

        public void Invert()
        {
            xx = zy*wz*yw - wy*zz*yw + wy*yz*zw - yy*wz*zw - zy*yz*ww + yy*zz*ww;
            yx = wx*zz*yw - zx*wz*yw - wx*yz*zw + yx*wz*zw + zx*yz*ww - yx*zz*ww;
            zx = zx*wy*yw - wx*zy*yw + wx*yy*zw - yx*wy*zw - zx*yy*ww + yx*zy*ww;
            wx = wx*zy*yz - zx*wy*yz - wx*yy*zz + yx*wy*zz + zx*yy*wz - yx*zy*wz;
            xy = wy*zz*xw - zy*wz*xw - wy*xz*zw + xy*wz*zw + zy*xz*ww - xy*zz*ww;
            yy = zx*wz*xw - wx*zz*xw + wx*xz*zw - xx*wz*zw - zx*xz*ww + xx*zz*ww;
            zy = wx*zy*xw - zx*wy*xw - wx*xy*zw + xx*wy*zw + zx*xy*ww - xx*zy*ww;
            wy = zx*wy*xz - wx*zy*xz + wx*xy*zz - xx*wy*zz - zx*xy*wz + xx*zy*wz;
            xz = yy*wz*xw - wy*yz*xw + wy*xz*yw - xy*wz*yw - yy*xz*ww + xy*yz*ww;
            yz = wx*yz*xw - yx*wz*xw - wx*xz*yw + xx*wz*yw + yx*xz*ww - xx*yz*ww;
            zz = yx*wy*xw - wx*yy*xw + wx*xy*yw - xx*wy*yw - yx*xy*ww + xx*yy*ww;
            wz = wx*yy*xz - yx*wy*xz - wx*xy*yz + xx*wy*yz + yx*xy*wz - xx*yy*wz;
            xw = zy*yz*xw - yy*zz*xw - zy*xz*yw + xy*zz*yw + yy*xz*zw - xy*yz*zw;
            yw = yx*zz*xw - zx*yz*xw + zx*xz*yw - xx*zz*yw - yx*xz*zw + xx*yz*zw;
            zw = zx*yy*xw - yx*zy*xw - zx*xy*yw + xx*zy*yw + yx*xy*zw - xx*yy*zw;
            ww = yx*zy*xz - zx*yy*xz + zx*xy*yz - xx*zy*yz - yx*xy*zz + xx*yy*zz;
            //Scale(1/Determinant()); //??
        }

        public double Determinant()
        {
            double value = wx*zy*yz*xw - zx*wy*yz*xw - wx*yy*zz*xw + yx*wy*zz*xw +
                           zx*yy*wz*xw - yx*zy*wz*xw - wx*zy*xz*yw + zx*wy*xz*yw +
                           wx*xy*zz*yw - xx*wy*zz*yw - zx*xy*wz*yw + xx*zy*wz*yw +
                           wx*yy*xz*zw - yx*wy*xz*zw - wx*xy*yz*zw + xx*wy*yz*zw +
                           yx*xy*wz*zw - xx*yy*wz*zw - zx*yy*xz*ww + yx*zy*xz*ww +
                           zx*xy*yz*ww - xx*zy*yz*ww - yx*xy*zz*ww + xx*yy*zz*ww;
            return value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MatrixColumn
        {
            public float m1; // x line
            public float m2; // y line
            public float m3; // z line
            public float m4; // w line
        };

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