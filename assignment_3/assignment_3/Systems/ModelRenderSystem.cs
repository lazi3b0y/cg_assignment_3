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
        private TextureCube _cubeMap;

        public ModelRenderSystem(TextureCube cubeMap)
        {
            _cubeMap = cubeMap;
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

                BumpEnvironmentMappedComponent envMap = ComponentHandler.GetComponent<BumpEnvironmentMappedComponent>(model.Owner);

                if (envMap != null)
                {
                    foreach (ModelMesh mesh in model.Model.Meshes)
                    {
                        foreach (ModelMeshPart part in mesh.MeshParts)
                        {
                            part.Effect = envMap.Effect;
                            envMap.Effect.Parameters["World"].SetValue(transform.WorldMatrix * mesh.ParentBone.Transform);
                            envMap.Effect.Parameters["View"].SetValue(camera.ViewMatrix);
                            envMap.Effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                            envMap.Effect.Parameters["ReflectiveModelTexture"].SetValue(_cubeMap);
                            envMap.Effect.Parameters["NormalMap"].SetValue(envMap.NormalMap);
                            envMap.Effect.Parameters["CameraPos"].SetValue(camera.CameraPosition);
                            envMap.Effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(transform.WorldMatrix * mesh.ParentBone.Transform)));
                        }
                        mesh.Draw();
                    }
                }
                else
                {
                    foreach (ModelMesh mesh in model.Model.Meshes)
                    {
                        foreach (Effect effect in mesh.Effects)
                        {
                            if ((effect is BasicEffect) == false)
                                continue;

                            BasicEffect effects = (BasicEffect)effect;

                            effects.EnableDefaultLighting();
                            effects.World = transforms[mesh.ParentBone.Index] * transform.WorldMatrix;
                            effects.View = camera.ViewMatrix;
                            effects.Projection = camera.ProjectionMatrix;
                        }
                        mesh.Draw();
                    }
                }
            }
        }
    }
}
