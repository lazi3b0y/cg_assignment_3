using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.Components
{
    public class CameraComponent : Component
    {
        public Matrix ProjectionMatrix { get; set; }
        public Matrix ViewMatrix { get; set; }
        public Vector3 CameraPosition { get; set; }
        public Vector3 CameraLookAt { get; set; }
        public Vector3 CameraUpVector { get; set; }
        public float CameraFarPlaneDistance { get; set; }
        public float CameraNearPlaneDistance { get; set; }
        public float CameraAspectRatio { get; set; }
    }
}
