using System;
using System.Collections.Generic;
using nManager.Wow.Class;

namespace nManager.Helpful
{
    public static class Math
    {
        /// <summary>
        /// Return Radian.
        /// </summary>
        /// <param name="degrees"> </param>
        /// <returns></returns>
        public static float DegreeToRadian(float degrees)
        {
            try
            {
                return degrees*(3.141592654f/180.0f);
            }
            catch (Exception exception)
            {
                Logging.WriteError("DegreeToRadian(float degrees): " + exception);
            }
            return 0;
        }

        /// <summary>
        /// Return Degree.
        /// </summary>
        /// <param name="radianValue"></param>
        /// <returns></returns>
        public static float RadianToDegree(float radianValue)
        {
            try
            {
                return (float) (System.Math.Round(Convert.ToDouble(radianValue*57.295779513082323)));
            }
            catch (Exception exception)
            {
                Logging.WriteError("RadianToDegree(float radianValue): " + exception);
            }
            return 0;
        }

        /// <summary>
        /// Return Angle to pos x =0 z = 0.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static float GetAngle(float x, float y)
        {
            try
            {
                double angle;

                double r = System.Math.Sqrt(System.Math.Pow(x, 2) + System.Math.Pow(y, 2));
                if (r == 0)
                {
                    return 0;
                }

                if (x == 0)
                {
                    angle = System.Math.Sign(y)*System.Math.PI/2;
                }
                else
                {
                    angle = System.Math.Atan(y/x);
                    if (x < 0)
                    {
                        angle = System.Math.PI + angle;
                    }
                }

                if (angle < 0)
                {
                    angle = angle + 2*System.Math.PI;
                }
                angle = 180*angle/System.Math.PI;
                return (float) angle;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetAngle(float x, float y): " + exception);
            }
            return 0;
        }

        /// <summary>
        /// Return Angle.
        /// </summary>
        /// <returns></returns>
        public static float GetAngle(Point pointA, Point pointB)
        {
            try
            {
                double xOffset = System.Math.Abs(pointB.X - pointA.X);
                double yOffset = System.Math.Abs(pointB.Y - pointA.Y);
                if (pointB.X < pointA.X && pointB.Y < pointA.Y)
                    return (float) (System.Math.PI/2 + System.Math.Atan(xOffset/yOffset));
                if (pointB.X < pointA.X)
                    return (float) (System.Math.PI + System.Math.Atan(yOffset/xOffset));
                if (pointB.Y < pointA.Y)
                    return (float) (System.Math.Atan(yOffset/xOffset));
                return (float) (System.Math.PI*1.5 + System.Math.Atan(xOffset/yOffset));
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetAngle(float x, float y): " + exception);
            }
            return 0;
        }

        /// <summary>
        /// Find the nearest Point in the list of Point (return a id).
        /// </summary>
        /// <param name="listPoint"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static int NearestPointOfListPoints(List<Point> listPoint, Point point)
        {
            try
            {
                int id = 0;
                float distance = 9999999999;
                int i = 0;

                foreach (Point pointTemp in listPoint)
                {
                    float dist = pointTemp.DistanceTo(point);
                    if (dist < distance)
                    {
                        distance = dist;
                        id = i;
                    }
                    i++;
                }
                return id;
            }
            catch (Exception e)
            {
                Logging.WriteError("NearestPointOfListPoints(List<Point> listPoint, Point point)" + e);
                return 0;
            }
        }

        /// <summary>
        /// Position 2d.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="angle"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static Point GetPosition2DOfAngleAndDistance(Point a, float angle, float distance)
        {
            try
            {
                float dy = (float) System.Math.Sin(angle)*distance;
                float dx = (float) System.Math.Cos(angle)*distance;
                return new Point(a.X + dx, a.Y + dy, a.Z);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetPosition2DOfAngleAndDistance(Point a, float angle, float distance): " + exception);
            }
            return new Point(0, 0, 0);
        }

        /// <summary>
        /// Position 2d.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static Point GetPosition2DOfLineByDistance(Point a, Point b, float distance)
        {
            try
            {
                if (a.X == b.X && a.Y == b.Y)
                    return a;

                float x = b.X - a.X;
                float y = b.Y - a.Y;
                float d = (float) System.Math.Sqrt(x*x + (double) (y*y));
                float xc = a.X + x*distance/d;
                float yc = a.Y + y*distance/d;

                return new Point(xc, yc, System.Math.Max(a.Z, b.Z));
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetPosition2dOfLineByDistance(Point a, Point b, float distance): " + exception);
            }
            return new Point(0, 0, 0);
        }

        public static Point GetPositionOffsetBy3DDistance(Point a, Point b, float distance)
        {
            if (a.X == b.X && a.Y == b.Y)
                return a;

            float x = b.X - a.X;
            float y = b.Y - a.Y;
            float z = b.Z - a.Z;
            float d = (float) System.Math.Sqrt((double) (x*x) + y*y + z*z);
            float xc = b.X + x/d*distance;
            float yc = b.Y + y/d*distance;
            float zc = b.Z + z/d*distance;

            return new Point(xc, yc, zc);
        }

        public static float DistanceListPoint(List<Point> listPoints)
        {
            try
            {
                if (listPoints.Count == 1)
                    return listPoints[0].DistanceTo(Wow.ObjectManager.ObjectManager.Me.Position);

                float size = 0;

                for (int i = 0; i <= listPoints.Count - 1; i++)
                {
                    int j = i + 1;
                    if (j <= listPoints.Count - 1)
                        size = size + listPoints[i].DistanceTo(listPoints[j]);
                }

                return size;
            }
            catch (Exception exception)
            {
                Logging.WriteError("DistanceListPoint(List<Point> listPoints): " + exception);
            }
            return 0;
        }
    }
}