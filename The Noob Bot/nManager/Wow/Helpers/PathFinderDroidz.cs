using System.Collections.Generic;
using nManager.Wow.Class;

namespace nManager.Wow.Helpers
{
    public class PathFinderDroidz
    {
        static List<Point> ListPointForPath = new List<Point>();

        public static void AddListPointForPath(Point point)
        {
            lock ("PathFinderDroidz")
            {
                if (point.Type.ToLower() != "flying" && point.Type.ToLower() != "swimming")
                    point.Type = "Ground";
                if (!ContainsListPointForPath(point))
                    ListPointForPath.Add(point);
            }
        }
        static bool ContainsListPointForPath(Point point)
        {
            foreach (var point1 in ListPointForPath)
            {
                if (point.DistanceTo(point1) < 0.5 && point.Type == point1.Type)
                    return true;
            }
            return false;
        }
        public static void AddRangeListPointForPath(List<Point> points)
        {
            foreach (var point in points)
            {
                AddListPointForPath(point);
            }
        }
        public static List<Point>  GetListPointForPath(string type = "Ground")
        {
            lock ("PathFinderDroidz")
            {
                var l = new List<Point>();
                type = type.ToLower();
                foreach (var point in ListPointForPath)
                {
                    if (point.Type.ToLower() == type)
                        l.Add(point);
                }
                return l;
            }
        }


        private static int continentId = -1;
        public static List<Point> FindPath(Point Destination, string type = "Ground")
        {
            bool t;
            return FindPath(Destination, out t, type);
        }
        public static List<Point> FindPath(Point Destination, out bool result, string type = "Ground")
        {
            if (continentId < 0)
                continentId = Usefuls.ContinentId;
            if (continentId != Usefuls.ContinentId)
                continentId = Usefuls.ContinentId;

            float findDistance = 5;
            float findDistanceZ = 3.0f;
            float firstAndLastRadiusFind = 15;
            if (type.ToLower() == "flying")
            {
                findDistance = 15;
                findDistanceZ = 10.0f;
                firstAndLastRadiusFind = 50;
            }

            if (type.ToLower() == "swimming")
            {
                findDistance = 15;
                findDistanceZ = 10.0f;
                firstAndLastRadiusFind = 50;
            }
            



            return FindPath(ObjectManager.ObjectManager.Me.Position, Destination, GetListPointForPath(type), out result, findDistance, findDistanceZ, firstAndLastRadiusFind);
        }

        static List<Point> curentPath = new List<Point>();
        public static List<Point> FindPath(Point From, Point Destination, List<Point> listPoint, float findDistance = 5, float findDistanceZ = 3.0f, float firstAndLastRadiusFind = 15)
        {
            bool result;
            return FindPath(From, Destination, listPoint, out result, findDistance, findDistanceZ, firstAndLastRadiusFind);
        }
        public static List<Point> FindPath(Point From, Point Destination, List<Point> listPoint, out bool result, float findDistance = 5, float findDistanceZ = 3.0f, float firstAndLastRadiusFind = 15)
        {
            result = false;

            if (listPoint.Count <= 0)
                return new List<Point> { From, Destination };

            curentPath = new List<Point> { From };

            var iNear = Helpful.Math.NearestPointOfListPoints(listPoint, From);
            if (From.DistanceTo2D(listPoint[iNear]) > firstAndLastRadiusFind)
                return new List<Point> { From, Destination };
            curentPath.Add(listPoint[iNear]);

            iNear = Helpful.Math.NearestPointOfListPoints(listPoint, Destination);
            if (Destination.DistanceTo2D(listPoint[iNear]) > firstAndLastRadiusFind)
                return new List<Point> { From, Destination };
            var nearDestination = listPoint[iNear];

            blackList = new Dictionary<string, List<string>>();
            
            
            while (listPoint.Count > 0)
            {
                var i = NearPoint(curentPath[curentPath.Count - 1], nearDestination, listPoint, findDistance, findDistanceZ);
                if (i < 0)
                {
                    curentPath.RemoveAt(curentPath.Count - 1);
                    if (curentPath.Count <= 0)
                        return new List<Point> { From, nearDestination };
                }
                else
                {
                    curentPath.Add(listPoint[i]);
                    if (nearDestination.DistanceTo2D(curentPath[curentPath.Count - 1]) <= findDistance && nearDestination.DistanceZ(curentPath[curentPath.Count - 1]) <= findDistanceZ)
                    {
                        curentPath.Add(nearDestination);
                        result = true;
                        return curentPath;
                    }
                }
            }
            return new List<Point> { From, Destination };
        }

        static Dictionary<string, List<string>> blackList = new Dictionary<string, List<string>>();
        static int NearPoint(Point From, Point Destination, List<Point> listPoints, float findDistance = 5, float findDistanceZ = 3.0f)
        {
            
            var lIndex = new List<int>();
            int ret = -1;
            float fromDistance = float.MaxValue;
            for (var i = 0; i <= listPoints.Count - 1; i++)
            {
                if (From.DistanceTo2D(listPoints[i]) <= findDistance && From.DistanceZ(listPoints[i]) <= findDistanceZ && From.DistanceTo(listPoints[i]) <= fromDistance)
                {
                    // Cherche si from a déja tester d'aller vers destination
                    List<string> tOut;
                    if (blackList.TryGetValue(From.ToString(), out tOut))
                        if (tOut.Contains(listPoints[i].ToString()))
                            continue;

                    // Cherche si destination a déja testé d'aller vers from
                    List<string> tOut2;
                    if (blackList.TryGetValue(listPoints[i].ToString(), out tOut2))
                        if (tOut2.Contains(From.ToString()))
                            continue;

                    // Cherche si from != de la position trouvé
                    if (listPoints[i].ToString() == From.ToString())
                        continue;

                    // Cherche si il y pas un point plus pret de destination que form dans la liste du path
                    var iNear = Helpful.Math.NearestPointOfListPoints(curentPath, listPoints[i]);
                    if (From.DistanceTo(listPoints[i]) >= curentPath[iNear].DistanceTo(listPoints[i]) && From.ToString() != curentPath[iNear].ToString())
                        continue;

                    // retourne la liste des plus proche de from
                    if (From.DistanceTo(listPoints[i]) < fromDistance)
                        lIndex.Clear();
                    fromDistance = From.DistanceTo(listPoints[i]);
                    lIndex.Add(i);
                }
            }

            // prend le plus proche de destination
            float desDistance = float.MaxValue;
            foreach (var i in lIndex)
            {
                if (Destination.DistanceTo(listPoints[i]) < desDistance)
                {
                    desDistance = Destination.DistanceTo(listPoints[i]);
                    ret = i;
                    
                }
            }


            if (ret >= 0)
            {
                // Ajoute tout blacklist
                List<string> tOut;
                if (blackList.TryGetValue(From.ToString(), out tOut))
                    tOut.Add(listPoints[ret].ToString());
                else
                {
                    blackList.Add(From.ToString(), new List<string> { listPoints[ret].ToString() });
                }
            }

            return ret;
        }
    }
}
