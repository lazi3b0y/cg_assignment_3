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
    public class SpecularProcessor : EffectProcessor
    {
        private readonly ComponentHandler _componentHandler;

        public SpecularProcessor(ComponentHandler componentHandler)
        {
            _componentHandler = componentHandler;
        }

        public override void Process(Entity entity, ref Effect effect, ModelMesh mesh)
        {
            var ef = _componentHandler.GetComponent<EffectComponent>(entity);
            var settings = ef.MeshEffects[mesh.Name].Settings;

            effect.Parameters["specularEnabled"].SetValue(true);
            if (settings.ContainsKey("ShininessAmount"))
                effect.Parameters["Shininess"].SetValue((float)settings["ShininessAmount"]);

            if (settings.ContainsKey("SpecularColor"))
                effect.Parameters["SpecularColor"].SetValue((Vector4)settings["SpecularColor"]);

            if (settings.ContainsKey("SpecularIntensity"))
                effect.Parameters["SpecularIntensity"].SetValue((float)settings["SpecularIntensity"]);

            if (settings.ContainsKey("ViewVector"))
                effect.Parameters["ViewVector"].SetValue((Vector3)settings["ViewVector"]);
        }
    }
}
