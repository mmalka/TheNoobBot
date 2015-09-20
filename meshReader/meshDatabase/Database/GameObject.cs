using SlimDX;

namespace meshDatabase.Database
{
    public class GameObject
    {
        public int Id { get; set; }
        public int Model { get; set; }
        public Matrix Transformation { get; set; }
        //public Vector3 Rotations { get; set; }

        public Vector3 Coordinates
        {
            get
            {
                return new Vector3(Transformation.M14, Transformation.M24, Transformation.M34);
            }
        }
    }
}
