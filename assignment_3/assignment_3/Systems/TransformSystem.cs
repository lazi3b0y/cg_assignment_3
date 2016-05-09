using assignment_3.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.Systems
{
    public class TransformSystem : UpdateSystem
    {
        public override void Update(GameTime gameTime)
        {
            var transforms = ComponentHandler.GetAllComponents<TransformComponent>();
            if (transforms == null)
                return;

            foreach (TransformComponent transform in transforms)
            {
                Quaternion quatRotation = Quaternion.CreateFromYawPitchRoll(transform.Rotation.Y, transform.Rotation.X, transform.Rotation.Z);
                transform.QuaternionRotation *= quatRotation;
                transform.Forward = Vector3.Transform(Vector3.Forward, transform.QuaternionRotation);
                transform.WorldMatrix = Matrix.CreateScale(transform.Scale) * Matrix.CreateFromQuaternion(transform.QuaternionRotation) * Matrix.CreateTranslation(transform.Position);
            }
        }
    }
}
