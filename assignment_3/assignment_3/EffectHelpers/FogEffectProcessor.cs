using assignment_3.Entites;
using assignment_3.Handlers;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.EffectHelpers
{
    public class FogEffectProcessor : EffectProcessor
    {
        private readonly Settings _settings;
        private readonly CameraHandler _cameraHandler;

        public FogEffectProcessor(Settings settings, CameraHandler cameraHandler)
        {
            _settings = settings;
            _cameraHandler = cameraHandler;
        }

        public override void Process(Entity entity, ref Effect effect, ModelMesh mesh)
        {
            var camera = _cameraHandler.ActiveCamera;

            effect.Parameters["FogEnabled"].SetValue(true);
            effect.Parameters["FogStart"].SetValue(_settings.FogStart);

            effect.Parameters["FogEnd"].SetValue(_settings.FogEnd);

            effect.Parameters["FogColor"].SetValue(_settings.FogColor.ToVector4());

            effect.Parameters["CameraPosition"].SetValue(camera.CameraPosition);

        }
    }
}
