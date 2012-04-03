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
            try
            {
                return status.HasFlag(DetourStatus.Success);
            }
            catch (Exception exception)
            {
                Logging.WriteError("HasSucceeded(this DetourStatus status): " + exception);
            }
            return false;
        }

        public static bool HasFailed(this DetourStatus status)
        {
            try
            {
                return status.HasFlag(DetourStatus.Failure);
            }
            catch (Exception exception)
            {
                Logging.WriteError("HasFailed(this DetourStatus status): " + exception);
            }
            return true;
        }

        public static bool IsInProgress(this DetourStatus status)
        {
            try
            {
                return status.HasFlag(DetourStatus.InProgress);
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsInProgress(this DetourStatus status): " + exception);
            }
            return false;
        }

        public static bool IsPartialResult(this DetourStatus status)
        {
            try
            {
                return status.HasFlag(DetourStatus.PartialResult);
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsPartialResult(this DetourStatus status): " + exception);
            }
            return false;
        }

        public static float[] ToFloatArray(this Point v)
        {
            try
            {
                return new[] { v.X, v.Y, v.Z };
            }
            catch (Exception exception)
            {
                Logging.WriteError(" ToFloatArray(this Point v): " + exception);
            }
            return new float[3];
        }

        public static float[] ToWoW(this float[] v)
        {
            try
            {
                return new[] { -v[2], -v[0], v[1] };
            }
            catch (Exception exception)
            {
                Logging.WriteError("ToWoW(this float[] v): " + exception);
            }
            return new float[3];
        }

        public static float[] ToRecast(this float[] v)
        {
            try
            {
                return new[] { -v[1], v[2], -v[0] };
            }
            catch (Exception exception)
            {
                Logging.WriteError("ToRecast(this float[] v): " + exception);
            }
            return new float[3];
        }

        public static Point ToWoW(this Point v)
        {
            try
            {
                return new Point(-v.Z, -v.X, v.Y);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ToWoW(this Point v): " + exception);
            }
            return new Point();
        }

        public static Point ToRecast(this Point v)
        {
            try
            {
                return new Point(-v.Y, v.Z, -v.X);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ToRecast(this Point v): " + exception);
            }
            return new Point();
        }

        public static float[] Origin = new[] { -17066.666f, 0, -17066.666f };

        public static float TileSize
        {
            get { return 533.33333f; }
        }
    }
}
