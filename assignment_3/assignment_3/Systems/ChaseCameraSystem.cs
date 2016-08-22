using assignment_3.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.Systems
{
    public class ChaseCameraSystem : UpdateSystem
    {
        public override void Update(GameTime gameTime)
        {
            var chaseCameras = ComponentHandler.GetAllComponents<ChaseCameraComponent>();

            if (chaseCameras == null)
                return;

            foreach (ChaseCameraComponent camera in chaseCameras)
            {
                CameraComponent baseCamera = ComponentHandler.GetComponent<CameraComponent>(camera.Owner);
                TransformComponent transform = ComponentHandler.GetComponent<TransformComponent>(camera.Owner);
                Vector3 cameraOffset = camera.CameraOffset;

                Matrix quaternionRotationMatrix = Matrix.CreateFromQuaternion(transform.QuaternionRotation);
                Vector3 cameraPosition = Vector3.Transform(cameraOffset, quaternionRotationMatrix);

                cameraPosition += transform.Position;
                baseCamera.CameraUpVector = Vector3.Transform(Vector3.Up, quaternionRotationMatrix);
                baseCamera.CameraPosition = cameraPosition;
                baseCamera.CameraLookAt = transform.Position;
            }
        }
    }
}
