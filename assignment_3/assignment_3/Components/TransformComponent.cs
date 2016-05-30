using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.Components
{
    public class TransformComponent : Component
    {
        public Matrix WorldMatrix { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Forward { get; set; }
        public Vector3 Right { get; set; }
        public Vector3 Scale { get; set; }
        public Quaternion QuaternionRotation { get; set; }
    }
}
