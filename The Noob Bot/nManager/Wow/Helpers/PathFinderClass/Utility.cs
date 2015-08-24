using System;
using DetourLayer;
using nManager.Helpful;
using nManager.Wow.Class;

namespace nManager.Wow.Helpers.PathFinderClass
{
    public static class Utility
    {
        public static bool HasSucceeded(this DetourStatus status)
        {
            return status.HasFlag(DetourStatus.Success);
        }

        public static bool HasFailed(this DetourStatus status)
        {
            return status.HasFlag(DetourStatus.Failure);
        }

        public static string GetDetourSupportedVersion()
        {
            int val = (int) DetourLayer.SupportedVersion.value;
            return val.ToString();
        }

        public static bool IsWrongVersion(this DetourStatus status)
        {
            return status.HasFlag(DetourStatus.WrongVersion);
        }

        public static bool IsInProgress(this DetourStatus status)
        {
            return status.HasFlag(DetourStatus.InProgress);
        }

        public static bool IsPartialResult(this DetourStatus status)
        {
            return status.HasFlag(DetourStatus.PartialResult);
        }

        public static float[] ToFloatArray(this Point v)
        {
            return new[] {v.X, v.Y, v.Z};
        }

        public static float[] ToWoW(this float[] v)
        {
            return new[] {-v[2], -v[0], v[1]};
        }

        public static float[] ToRecast(this float[] v)
        {
            return new[] {-v[1], v[2], -v[0]};
        }

        public static Point ToWoW(this Point v)
        {
            return new Point(-v.Z, -v.X, v.Y);
        }

        public static Point ToRecast(this Point v)
        {
            return new Point(-v.Y, v.Z, -v.X);
        }

        public static float[] Origin = new[] {-17066.666f, 0, -17066.666f};

        public static float TileSize
        {
            get { return 533f + (1 / (float)3); }
        }
    }
}