using SlimDX;

namespace meshReader.Game
{

        public static class World
        {
            public static float[] GetMinimum()
            {
                return new[] { -Constant.MaxXY, -500.0f, -Constant.MaxXY };
            }

            public static float[] GetMaximum()
            {
                return new[] { Constant.MaxXY, 500.0f, Constant.MaxXY };
            }

            public static float[] ToRecast(this float[] val)
            {
                return new[] { -val[1], val[2], -val[0] };
            }

            public static float[] ToWoW(this float[] val)
            {
                return new[] { -val[2], -val[0], val[1] };
            }

            public static float[] Origin
            {
                get
                {
                    return new[]{-Constant.MaxXY, 0, -Constant.MaxXY};
                }
            }

            public static float[] ToFloatArray(this Vector3 v)
            {
                return new[]{v.X, v.Y, v.Z};
            }

            public static Vector3 ToWoW(this Vector3 v)
            {
                return new Vector3(-v.Z, -v.X, v.Y);
            }

            public static Vector3 ToRecast(this Vector3 v)
            {
                return new Vector3(-v.Y, v.Z, -v.X);
            }
        }

}
