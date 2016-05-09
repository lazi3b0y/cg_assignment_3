using assignment_3.Handlers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.Systems
{
    public class CameraSystem : UpdateSystem
    {
        public override void Update(GameTime gameTime)
        {
            var camera = CameraHandler.ActiveCamera;
            if (camera == null)
                return;

            camera.ViewMatrix = Matrix.CreateLookAt(camera.CameraPosition, camera.CameraLookAt, camera.CameraUpVector);
            camera.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(75), camera.CameraAspectRatio, camera.CameraNearPlaneDistance, camera.CameraFarPlaneDistance);
        }
    }
}
