using assignment_3.Entites;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.EffectHelpers
{
    public class DirectionProcessor : EffectProcessor
    {
        private readonly Settings _settings;

        public DirectionProcessor(Settings settings)
        {
            _settings = settings;
        }

        public override void Process(Entity entity, ref Effect effect, ModelMesh mesh)
        {
            effect.Parameters["directionLightEnabled"].SetValue(true);
            effect.Parameters["DiffuseLightDirection"].SetValue(_settings.DiffuseLightDirection);
            effect.Parameters["DiffuseColor"].SetValue(_settings.DiffuseColor.ToVector4());
            effect.Parameters["DiffuseIntensity"].SetValue(_settings.DiffuseIntensity);
        }
    }
}
