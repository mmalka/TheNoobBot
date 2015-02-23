using meshReader.Game.ADT;
using meshReader.Game.WMO;
using SlimDX;
using SlimMath;

namespace meshReader.Game
{
    
    public static class Transformation
    {
        
        public interface IDefinition
        {
            Vector3 Position { get; }
            float Scale { get; }
            Vector3 Rotation { get; }
        }

        public static Matrix GetTransformation(IDefinition def)
        {
            Matrix translation;
            if (def.Position.X == 0.0f && def.Position.Y == 0.0f && def.Position.Z == 0.0f)
                translation = Matrix.Identity;
            else
                translation = Matrix.Translation(-(def.Position.Z - Constant.MaxXY),
                                                       -(def.Position.X - Constant.MaxXY), def.Position.Y);

            var rotation = Matrix.RotationX(MathHelper.DegreesToRadians(def.Rotation.Z)) *
                           Matrix.RotationY(MathHelper.DegreesToRadians(def.Rotation.X)) * Matrix.RotationZ(MathHelper.DegreesToRadians(def.Rotation.Y + 180));

            if (def.Scale < 1.0f || def.Scale > 1.0f)
                return (Matrix.Scaling(new Vector3(def.Scale,def.Scale,def.Scale))*rotation)*translation;
            return rotation * translation;
        }

        public static Matrix GetWmoDoodadTransformation(DoodadInstance inst, WorldModelHandler.WorldModelDefinition root)
        {
            var rootTransformation = GetTransformation(root);

            var translation = Matrix.Translation(inst.Position);
            var scale = Matrix.Scaling(new Vector3(inst.Scale, inst.Scale, inst.Scale));
            var rotation = Matrix.RotationY(MathHelper.Pi);
            var quatRotation =
                Matrix.RotationQuaternion(new Quaternion(-inst.QuatY, inst.QuatZ, -inst.QuatX, inst.QuatW));

            return scale*rotation*quatRotation*translation*rootTransformation;
        }
    }

}