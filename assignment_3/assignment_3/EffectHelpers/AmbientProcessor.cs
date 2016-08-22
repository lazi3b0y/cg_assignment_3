using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using assignment_3.Entites;
using Microsoft.Xna.Framework.Graphics;

namespace assignment_3.EffectHelpers
{
    public class AmbientProcessor : EffectProcessor
    {
        private readonly Settings _settings;

        public AmbientProcessor(Settings settings)
        {
            _settings = settings;
        }

        public override void Process(Entity entity, ref Effect effect, ModelMesh mesh)
        {
            effect.Parameters["ambientEnabled"].SetValue(true);
            effect.Parameters["AmbientColor"].SetValue(_settings.AmbientColor.ToVector4());
            effect.Parameters["AmbientIntensity"].SetValue(_settings.AmbientIntensity);
        }
    }
}
