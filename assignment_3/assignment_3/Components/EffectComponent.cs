using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.Components
{
    public class MeshEffect
    {
        public Effect Effect { get; set; }
        public bool IsTransparent { get; set; }
        public bool IsShadow { get; set; }
        public ICollection<string> Parameters { get; set; }
        public IDictionary<string, object> Settings { get; set; }
    }

    public class EffectComponent : Component
    {
        public Dictionary<string, MeshEffect> MeshEffects { get; set; }
    }
}
