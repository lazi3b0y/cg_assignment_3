using assignment_3.Components;
using assignment_3.Entites;
using assignment_3.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.EffectHelpers
{
    public class ShadowProcessor : EffectProcessor
    {
        private readonly ComponentHandler _componentHandler;
        private readonly CameraHandler _cameraHandler;

        public ShadowProcessor(ComponentHandler componentHandler, CameraHandler cameraHandler)
        {
            _componentHandler = componentHandler;
            _cameraHandler = cameraHandler;
        }

        public override void Process(Entity entity, ref Effect effect, ModelMesh mesh)
        {
            var ef = _componentHandler.GetComponent<EffectComponent>(entity);
            var settings = ef.MeshEffects[mesh.Name].Settings;

            effect.Parameters["ShadowEnabled"].SetValue(true);

            if (settings.ContainsKey("ShadowIntensity"))
                effect.Parameters["ShadowIntensity"].SetValue((float)settings["ShadowIntensity"]);

            //effect.Parameters["ShadowMapTexture"].SetValue();
        }
    }
}
