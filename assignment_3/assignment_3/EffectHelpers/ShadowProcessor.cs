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
            var tr = _componentHandler.GetComponent<TransformComponent>(entity);
            var settings = ef.MeshEffects[mesh.Name].Settings;

            effect.Parameters["ShadowEnabled"].SetValue(true);

            if (settings.ContainsKey("ShadowIntensity"))
                effect.Parameters["ShadowIntensity"].SetValue((float)settings["ShadowIntensity"]);

            var lightPos = new Vector3(1, 1, 0);

            var lightsView = Matrix.CreateLookAt(lightPos, new Vector3(1, 1, 0), new Vector3(0, 1, 0));
            var lightsProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1f, 5f, 100f);

            var lightsViewProjectionMatrix = lightsView * lightsProjection;

            effect.Parameters["LightsWorldViewProjection"].SetValue(Matrix.Identity * lightsViewProjectionMatrix);
        }
    }
}
