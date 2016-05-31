using assignment_3.Components;
using assignment_3.EffectHelpers;
using assignment_3.Entites;
using assignment_3.Handlers;
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
        private readonly CameraHandler _cameraHandler;
        private readonly ComponentHandler _componentHandler;
        private readonly EffectProvider _effectProvider;
        private readonly GraphicsDevice _graphics;
        private SortedList<float, Tuple<ModelMesh, MeshEffect, Entity>> _sortedTransparent;

        public ModelRenderSystem(GraphicsDevice graphicsDevice, EffectProvider effectProvider, ComponentHandler componentHandler, CameraHandler cameraHandler)
        {
            _componentHandler = componentHandler;
            _cameraHandler = cameraHandler;
            _graphics = graphicsDevice;
            _effectProvider = effectProvider;
            _sortedTransparent = new SortedList<float, Tuple<ModelMesh, MeshEffect, Entity>>(new InvertedComparer());
        }

        public void DoRender(Entity exculdedEntity = null)
        {
            var camera = _cameraHandler.ActiveCamera;
            var models = _componentHandler.GetAllComponents<ModelComponent>();
            if (models == null) return;
            _sortedTransparent.Clear();
            foreach (var m in models)
            {
                if (exculdedEntity != null && m.Owner == exculdedEntity)
                    continue;

                var effect = _componentHandler.GetComponent<EffectComponent>(m.Owner);
                var transform = _componentHandler.GetComponent<TransformComponent>(m.Owner);

                if (effect == null)
                {
                    RenderModelWithBasicEffect(m.Model, transform.WorldMatrix, camera.ViewMatrix, camera.ProjectionMatrix);
                }
                else
                {
                    foreach (var mesh in m.Model.Meshes)
                    {
                        if (effect.MeshEffects.ContainsKey(mesh.Name))
                        {
                            if (effect.MeshEffects[mesh.Name].IsTransparent)
                            {
                                var meshEffect = effect.MeshEffects[mesh.Name];
                                var distanceFromCamera = Vector3.Distance(camera.CameraPosition, transform.Position);
                                _sortedTransparent.Add(distanceFromCamera, new Tuple<ModelMesh, MeshEffect, Entity>(mesh, meshEffect, effect.Owner));
                            }
                            else
                            {
                                RenderMeshWithEffect(mesh, effect.MeshEffects[mesh.Name], effect.Owner);
                            }
                        }
                    }
                }
            }

            foreach (var transparentThings in _sortedTransparent)
            {
                var entity = transparentThings.Value.Item3;
                var meshEffect = transparentThings.Value.Item2;
                var mesh = transparentThings.Value.Item1;

                RenderMeshWithEffect(mesh, meshEffect, entity, true);
            }

        }

        public void RenderModelWithBasicEffect(Model model, Matrix world, Matrix view, Matrix projection)
        {
            _graphics.BlendState = BlendState.Opaque;
            var boneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = boneTransforms[mesh.ParentBone.Index] * world;
                    effect.View = view;

                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }

        public void RenderMeshWithEffect(ModelMesh mesh, MeshEffect effect, Entity e, bool transparent = false)
        {
            _graphics.BlendState = transparent ? BlendState.AlphaBlend : BlendState.Opaque;

            var orgEffects = mesh.Effects;

            foreach (var part in mesh.MeshParts)
            {
                part.Effect = _effectProvider.GetEffect(effect, mesh, e);
            }
            mesh.Draw();

            mesh.Effects = orgEffects;

            _graphics.BlendState = BlendState.Opaque;
        }

        public override void Render(GameTime gameTime)
        {
            DoRender();
        }

        internal class InvertedComparer : IComparer<float>
        {
            public InvertedComparer()
            {

            }
            public int Compare(float x, float y)
            {
                return y.CompareTo(x);
            }
        }
    }
}
