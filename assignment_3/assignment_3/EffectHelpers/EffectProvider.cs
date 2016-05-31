using assignment_3.Components;
using assignment_3.Entites;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.EffectHelpers
{
    public class EffectProvider
    {
        private readonly IDictionary<string, EffectProcessor> _effectProcessors =
            new Dictionary<string, EffectProcessor>();

        public void AddEffectProcessor(string key, EffectProcessor effectProcessor)
        {
            _effectProcessors.Add(key, effectProcessor);
        }

        public Effect GetEffect(MeshEffect effect, ModelMesh mesh, Entity entity)
        {
            var e = effect.Effect;
            foreach (var parameter in effect.Parameters)
            {
                _effectProcessors[parameter].Process(entity, ref e, mesh);
            }

            return e;
        }
    }
}
