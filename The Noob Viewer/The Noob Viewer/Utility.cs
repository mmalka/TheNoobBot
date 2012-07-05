using Microsoft.Xna.Framework;

namespace meshPathVisualizer
{

    public static class Utility
    {
        public static float[] ToFloatArray(this Vector3 v)
        {
            return new[] { v.X, v.Y, v.Z };
        }

        public static float[] ToWoW(this float[] v)
        {
            return new[]{-v[2], -v[0], v[1]};
        }

        public static float[] ToRecast(this float[] v)
        {
            return new[] { -v[1], v[2], -v[0] };
        }

        public static Vector3 ToWoW(this Vector3 v)
        {
            return new Vector3(-v.Z, -v.X, v.Y);
        }

        public static Vector3 ToRecast(this Vector3 v)
        {
            return new Vector3(-v.Y, v.Z, -v.X);
        }

        public static float[] Origin = new[] { -17066.666f, 0, -17066.666f };

        public static float TileSize
        {
            get { return 533.33333f; }
        }
    }

}