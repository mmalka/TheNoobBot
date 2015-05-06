using System;
using System.Runtime.InteropServices;
using nManager.Helpful;
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

        public Matrix4 Invert()
        {
            var m = this;
            var s0 = xx*yy - yx*xy;
            var s1 = xx*yz - yx*xz;
            var s2 = xx*yw - yx*xw;
            var s3 = xy*yz - yy*xz;
            var s4 = xy*yw - yy*xw;
            var s5 = xz*yw - yz*xw;

            var c5 = zz*ww - wz*zw;
            var c4 = zy*ww - wy*zw;
            var c3 = zy*wz - wy*zz;
            var c2 = zx*ww - wx*zw;
            var c1 = zx*wz - wx*zz;
            var c0 = zx*wy - wx*zy;

            var invdet = 1/(s0*c5 - s1*c4 + s2*c3 + s3*c2 - s4*c1 + s5*c0);
            m.xx = (yy*c5 - yz*c4 + yw*c3)*invdet;
            m.xy = (-xy*c5 + xz*c4 - xw*c3)*invdet;
            m.xz = (wy*s5 - wz*s4 + ww*s3)*invdet;
            m.xw = (-zy*s5 + zz*s4 - zw*s3)*invdet;
            m.yx = (-yx*c5 + yz*c2 - yw*c1)*invdet;
            m.yy = (xx*c5 - xz*c2 + xw*c1)*invdet;
            m.yz = (-wx*s5 + wz*s2 - ww*s1)*invdet;
            m.yw = (zx*s5 - zz*s2 + zw*s1)*invdet;
            m.zx = (yx*c4 - yy*c2 + yw*c0)*invdet;
            m.zy = (-xx*c4 + xy*c2 - xw*c0)*invdet;
            m.zz = (wx*s4 - wy*s2 + ww*s0)*invdet;
            m.zw = (-zx*s4 + zy*s2 - zw*s0)*invdet;
            return m;
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