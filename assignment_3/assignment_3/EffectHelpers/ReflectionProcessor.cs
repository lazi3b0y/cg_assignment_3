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
    public class ReflectionProcessor : EffectProcessor
    {
        private readonly ComponentHandler _componentHandler;
        private readonly CameraHandler _cameraHandler;

        public ReflectionProcessor(ComponentHandler componentHandler, CameraHandler cameraHandler)
        {
            _componentHandler = componentHandler;
            _cameraHandler = cameraHandler;
        }

        public override void Process(Entity entity, ref Effect effect, ModelMesh mesh)
        {
            var ef = _componentHandler.GetComponent<EffectComponent>(entity);
            var settings = ef.MeshEffects[mesh.Name].Settings;

            effect.Parameters["ReflectionEnabled"].SetValue(true);

            if (settings.ContainsKey("ReflectionAmount"))
                effect.Parameters["ReflectionAmount"].SetValue((float)settings["ReflectionAmount"]);
            if (settings.ContainsKey("TintColor"))
                effect.Parameters["TintColor"].SetValue((Vector4)settings["TintColor"]);

            effect.Parameters["CameraPosition"].SetValue(_cameraHandler.ActiveCamera.CameraPosition);
            effect.Parameters["EnvironmentMapTexture"].SetValue((RenderTargetCube)settings["EnviromentMap"]);
        }
    }
}
