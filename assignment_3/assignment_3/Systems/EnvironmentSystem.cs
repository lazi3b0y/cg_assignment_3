using assignment_3.Components;
using assignment_3.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.Systems
{
    public class EnvironmentSystem : UpdateSystem
    {
        private readonly ComponentHandler _componentHandler;
        private readonly CameraHandler _cameraManager;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly ModelRenderSystem _modelRenderSystem;

        private readonly RenderTargetCube _refCube;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;


        public EnvironmentSystem(GraphicsDevice graphicsDevice, CameraHandler cameraHandler,
            ModelRenderSystem modelRenderSystem, ComponentHandler componentHandler)
        {
            _graphicsDevice = graphicsDevice;
            _cameraManager = cameraHandler;
            _modelRenderSystem = modelRenderSystem;
            _componentHandler = componentHandler;
            _refCube = new RenderTargetCube(_graphicsDevice, 256, false, SurfaceFormat.Color, DepthFormat.Depth24);
        }
        public override void Update(GameTime gameTime)
        {
            var effectComponents = _componentHandler.GetAllComponents<EffectComponent>();
            var camera = _cameraManager.ActiveCamera;

            foreach (var effect in effectComponents)
            {
                foreach (var e in effect.MeshEffects)
                {
                    if (e.Value.IsReflective)
                    {
                        var settings = e.Value.Settings;

                        var transform = _componentHandler.GetComponent<TransformComponent>(effect.Owner);

                        var orgAspectRatio = camera.CameraAspectRatio;
                        var orgView = _cameraManager.ActiveCamera.ViewMatrix;
                        var orgProjection = camera.ProjectionMatrix;

                        camera.CameraAspectRatio = 1;
                        camera.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), camera.CameraAspectRatio,
                            camera.CameraNearPlaneDistance, camera.CameraFarPlaneDistance);

                        for (int i = 0; i < 6; i++)
                        {
                            CubeMapFace cubeMapFace = (CubeMapFace)i;
                            switch (cubeMapFace)
                            {
                                case CubeMapFace.NegativeX:
                                    {
                                        camera.ViewMatrix = Matrix.CreateLookAt(transform.Position, transform.Position + Vector3.Left, Vector3.Up);
                                        break;
                                    }
                                case CubeMapFace.NegativeY:
                                    {
                                        camera.ViewMatrix = Matrix.CreateLookAt(transform.Position, transform.Position + Vector3.Down, Vector3.Forward);
                                        break;
                                    }
                                case CubeMapFace.NegativeZ:
                                    {
                                        camera.ViewMatrix = Matrix.CreateLookAt(transform.Position, transform.Position + Vector3.Backward, Vector3.Up);
                                        break;
                                    }
                                case CubeMapFace.PositiveX:
                                    {
                                        camera.ViewMatrix = Matrix.CreateLookAt(transform.Position, transform.Position + Vector3.Right, Vector3.Up);
                                        break;
                                    }
                                case CubeMapFace.PositiveY:
                                    {
                                        camera.ViewMatrix = Matrix.CreateLookAt(transform.Position, transform.Position + Vector3.Up, Vector3.Backward);
                                        break;
                                    }
                                case CubeMapFace.PositiveZ:
                                    {
                                        camera.ViewMatrix = Matrix.CreateLookAt(transform.Position, transform.Position + Vector3.Forward, Vector3.Up);
                                        break;
                                    }
                            }

                            _graphicsDevice.SetRenderTarget(_refCube, cubeMapFace);
                            _graphicsDevice.Clear(Color.White);

                            _modelRenderSystem.DoRender(effect.Owner);

                        }
                        _graphicsDevice.SetRenderTarget(null);

                        settings["EnviromentMap"] = _refCube;
                        camera.ViewMatrix = orgView;
                        camera.CameraAspectRatio = orgAspectRatio;
                        camera.ProjectionMatrix = orgProjection;
                    }
                }

            }
        }

    }
}
