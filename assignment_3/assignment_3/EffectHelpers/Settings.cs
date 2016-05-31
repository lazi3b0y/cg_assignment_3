using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.EffectHelpers
{
    public class Settings
    {
        public float FogStart { get; set; }
        public float FogEnd { get; set; }
        public Color FogColor { get; set; }
        public Color AmbientColor { get; set; }
        public float AmbientIntensity { get; set; }
        public Vector3 DiffuseLightDirection { get; set; }
        public Color DiffuseColor { get; set; }
        public float DiffuseIntensity { get; set; }
    }
}
