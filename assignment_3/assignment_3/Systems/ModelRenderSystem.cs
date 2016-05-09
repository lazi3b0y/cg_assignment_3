using assignment_3.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    BumpEnvironmentMappedComponent envMap = ComponentHandler.GetComponent<BumpEnvironmentMappedComponent>(model.Owner);

                    if (envMap != null)
                    {
                        foreach(ModelMeshPart part in mesh.MeshParts)
                        {
                            part.Effect = envMap.Effect;
                            envMap.Effect.Parameters["World"].SetValue(transform.WorldMatrix * mesh.ParentBone.Transform);
                            envMap.Effect.Parameters["View"].SetValue(camera.ViewMatrix);
                            envMap.Effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);

                            TextureCube tC = new TextureCube(_graphics, 256, true, SurfaceFormat.Color);
                            envMap.Effect.Parameters["ReflectiveModelTexture"].SetValue(tC);

                            //normal texture in normal-tangent-binormal-frame ??
                            envMap.Effect.Parameters["NormalMap"].SetValue(envMap.NormalMap);

                            envMap.Effect.Parameters["CameraPos"].SetValue(camera.CameraPosition);
                            envMap.Effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(transform.WorldMatrix * mesh.ParentBone.Transform)));
                        }
                    }
                    else
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.World = transforms[mesh.ParentBone.Index] * transform.WorldMatrix;
                            effect.View = camera.ViewMatrix;
                            effect.Projection = camera.ProjectionMatrix;
                        }
                    }
                    mesh.Draw();
                }
            }
        }
    }
}
