using Microsoft.Xna.Framework;

namespace meshDisplay
{
    public class Camera
    {
        protected float Fov = MathHelper.Pi/3;
        protected float AspectRatio = 1;
        protected Vector3 CameraPosition;
        protected Quaternion CameraRotation;
        protected float FarClip = 10000.0f;
        protected float NearClip = 2.0f;

        protected float Pitch;
        protected float Roll;
        protected float Yaw;

        public Camera()
        {
            CameraRotation = new Quaternion();
            CameraPosition = Vector3.Zero;

            Yaw = 0;
            Pitch = 0;
            Roll = 0;
        }

        public Quaternion Rotation
        {
            get { return CameraRotation; }
            set { CameraRotation = value; }
        }

        public Vector3 Position
        {
            get { return CameraPosition; }
            set { CameraPosition = value; }
        }

        public Matrix Projection
        {
            get { return Matrix.CreatePerspectiveFieldOfView(Fov, AspectRatio, NearClip, FarClip); }
        }

        public Matrix View
        {
            get { return Matrix.Invert(Matrix.CreateFromQuaternion(Rotation)*Matrix.CreateTranslation(Position)); }
        }

        public void Rotate(float xRotation, float yRotation, float zRotation)
        {
            Yaw += xRotation;
            Pitch += yRotation;
            Roll += zRotation;

            Quaternion q1 = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), Yaw);
            Quaternion q2 = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), Pitch);
            Quaternion q3 = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), Roll);
            CameraRotation = q1*q2*q3;
        }

        public void Translate(Vector3 distance)
        {
            Vector3 diff = Vector3.Transform(distance, Matrix.CreateFromQuaternion(CameraRotation));
            CameraPosition += diff;
        }
    }
}