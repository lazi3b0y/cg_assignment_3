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
    public class MatrixEffectProcessor : EffectProcessor
    {
        private readonly CameraHandler _cameraHandler;
        private readonly ComponentHandler _componentHandler;

        public MatrixEffectProcessor(CameraHandler cameraHandler, ComponentHandler componentHandler)
        {
            _cameraHandler = cameraHandler;
            _componentHandler = componentHandler;
        }

        public override void Process(Entity entity, ref Effect effect, ModelMesh mesh)
        {
            var cam = _cameraHandler.ActiveCamera;
            var transform = _componentHandler.GetComponent<TransformComponent>(entity);

            effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * transform.WorldMatrix);
            effect.Parameters["View"].SetValue(cam.ViewMatrix);
            effect.Parameters["Projection"].SetValue(cam.ProjectionMatrix);
            effect.Parameters["WorldInverseTranspose"].SetValue(
                                            Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * transform.WorldMatrix)));
            effect.Parameters["CameraPosition"].SetValue(cam.CameraPosition);
        }
    }
}
