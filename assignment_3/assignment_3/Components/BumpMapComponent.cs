using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.Components
{
    class BumpMapComponent : Component
    {
        public Effect Effect { get; set; }
        public Texture2D NormalMap { get; set; }
    }
}
