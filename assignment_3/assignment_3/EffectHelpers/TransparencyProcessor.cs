using assignment_3.Components;
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
    public class TransparencyProcessor : EffectProcessor
    {
        private readonly ComponentHandler _componentHandler;
        public TransparencyProcessor(ComponentHandler componentHandler)
        {
            _componentHandler = componentHandler;
        }

        public override void Process(Entity entity, ref Effect effect, ModelMesh mesh)
        {
            var ef = _componentHandler.GetComponent<EffectComponent>(entity);
            var settings = ef.MeshEffects[mesh.Name].Settings;

            effect.Parameters["TransparencyEnabled"].SetValue(true);

            if (settings.ContainsKey("TransparencyAmount"))
                effect.Parameters["Transparency"].SetValue((float)settings["TransparencyAmount"]);
            if (settings.ContainsKey("TransparencyThreshold"))
                effect.Parameters["TransparencyThreshold"].SetValue((float)settings["TransparencyThreshold"]);
        }
    }
}
