using assignment_3.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.Systems
{
    public class ModelRenderSystem : RenderSystem
    {
        private GraphicsDevice _graphics;

        public ModelRenderSystem(GraphicsDevice graphics)
        {
            _graphics = graphics;
        }

        public override void Render(GameTime gameTime)
        {
            CameraComponent camera = CameraHandler.ActiveCamera;
            var models = ComponentHandler.GetAllComponents<ModelComponent>();

            if (camera == null || models == null)
                return;

            foreach (ModelComponent model in models)
            {
                TransformComponent transform = ComponentHandler.GetComponent<TransformComponent>(model.Owner);

                Matrix[] transforms = new Matrix[model.Model.Bones.Count];
                model.Model.CopyAbsoluteBoneTransformsTo(transforms);


                foreach (ModelMesh mesh in model.Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.World = transforms[mesh.ParentBone.Index] * transform.WorldMatrix;
                        effect.View = camera.ViewMatrix;
                        effect.Projection = camera.ProjectionMatrix;
                    }
                    mesh.Draw();
                }
            }
        }
    }
}
