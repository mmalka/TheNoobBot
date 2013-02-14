namespace meshReader.Game
{

    public struct Triangle<T>
    {
        public T V0;
        public T V1;
        public T V2;

        public TriangleType Type;

        public Triangle(TriangleType type, T v0, T v1, T v2)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            Type = type;
        }
    }

}