using assignment_3.Entites;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.EffectHelpers
{
    public abstract class EffectProcessor
    {
        public abstract void Process(Entity entity, ref Effect effect, ModelMesh mesh);
    }
}
