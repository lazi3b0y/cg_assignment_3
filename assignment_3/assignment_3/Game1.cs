using assignment_3.Components;
using assignment_3.EffectHelpers;
using assignment_3.Entites;
using assignment_3.Handlers;
using assignment_3.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace assignment_3
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private CameraHandler _cameraHandler = CameraHandler.Instance;
        private SystemHandler _systemHandler = SystemHandler.Instance;
        private ComponentHandler _componentHandler = ComponentHandler.Instance;
        private Settings _settings;
        private EffectProvider _effectProvider;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _effectProvider = new EffectProvider();

            ModelRenderSystem mDR = new ModelRenderSystem(_graphics.GraphicsDevice, _effectProvider, _componentHandler, _cameraHandler);
            _systemHandler.AddSystem(new ObserverSystem());
            _systemHandler.AddSystem(new ChaseCameraSystem());
            _systemHandler.AddSystem(new CameraSystem());
            _systemHandler.AddSystem(new TransformSystem());
            _systemHandler.AddSystem(new CubeMapSystem(_graphics.GraphicsDevice, _cameraHandler, mDR, _componentHandler));
            _systemHandler.AddSystem(mDR);

            _settings = new Settings
            {
                AmbientColor = Color.White,
                AmbientIntensity = 0.3f,
                DiffuseColor = Color.Red,
                DiffuseIntensity = 0.5f,
                DiffuseLightDirection = new Vector3(1, 1, 0),
                FogStart = 50f,
                FogEnd = 100f,
                FogColor = Color.Blue
            };

            _effectProvider.AddEffectProcessor("TextureMapping", new TextureProcessor(_componentHandler));
            _effectProvider.AddEffectProcessor("SpecularLight", new SpecularProcessor(_componentHandler));
            _effectProvider.AddEffectProcessor("AmbientLight", new AmbientProcessor(_settings));
            _effectProvider.AddEffectProcessor("Matrices", new MatrixProcessor(_cameraHandler, _componentHandler));
            _effectProvider.AddEffectProcessor("Shadow", new ShadowProcessor(_componentHandler, _cameraHandler));
            _effectProvider.AddEffectProcessor("Transparency", new TransparencyProcessor(_componentHandler));
            _effectProvider.AddEffectProcessor("DirectionLight", new DirectionProcessor(_settings));
            _effectProvider.AddEffectProcessor("Fog", new FogProcessor(_settings, _cameraHandler));

            CameraComponent camera = new CameraComponent { CameraNearPlaneDistance = 0.1f, CameraFarPlaneDistance = 500f, CameraAspectRatio = _graphics.GraphicsDevice.Viewport.AspectRatio };
            ChaseCameraComponent chaseCamera = new ChaseCameraComponent { CameraOffset = new Vector3(0, 0.1f, 1) };
            TransformComponent observerTransform = new TransformComponent { Scale = new Vector3(0.001f), Position = new Vector3(600, 150, -300), QuaternionRotation = Quaternion.Identity };
            ObserverComponent observerComponent = new ObserverComponent();

            Entity observer = new Entity();

            _cameraHandler.ActiveCamera = camera;

            observer.AddComponent(camera);
            observer.AddComponent(chaseCamera);
            observer.AddComponent(observerTransform);
            observer.AddComponent(observerComponent);

            //Load 3d model files
            Model snowplowModel = Content.Load<Model>("snowplow");
            Model hangarModel = Content.Load<Model>("moffett-hangar2");
            var effect = Content.Load<Effect>("ShadowMapShader");

            //Snowplow 1 - Regular - Needs texture before being usefull
            Entity snowplow1 = new Entity();
            ModelComponent snowplow1ModelComp = new ModelComponent { Model = snowplowModel };
            TransformComponent snowplow1Trans = new TransformComponent { Scale = new Vector3(0.1f), Position = new Vector3(600, 150, -280), QuaternionRotation = Quaternion.Identity };
            snowplow1.AddComponent(snowplow1ModelComp);
            snowplow1.AddComponent(snowplow1Trans);

            //Sphere 1 - AmbientLight, Direction Light with Fog(Turning object blue at distance)
            Entity sphere1 = new Entity();
            Model sphereModel1 = Content.Load<Model>("sphere_mapped");
            ModelComponent sphere1ModelComp = new ModelComponent { Model = sphereModel1 };
            TransformComponent sphere1TransComp = new TransformComponent { Scale = new Vector3(12f), Position = new Vector3(525, 150, -330), QuaternionRotation = Quaternion.Identity };
            sphere1.AddComponent(sphere1ModelComp);
            sphere1.AddComponent(sphere1TransComp);

            EffectComponent sphere1Effect = new EffectComponent();
            sphere1Effect.MeshEffects = new Dictionary<string, MeshEffect>();
            sphere1Effect.MeshEffects.Add("Sphere", new MeshEffect
            {
                IsTransparent = false,
                IsShadow = false,
                Effect = effect.Clone(),
                Parameters = new List<string>
                {
                    "Matrices",
                    "AmbientLight",
                    "DirectionLight",
                    "Fog",
                    "SpecularLight"
                },
                Settings = new Dictionary<string, object>
                {
                    
                }
            });
            sphere1.AddComponent(sphere1Effect);

            //Sphere 2 - Shadow
            Entity sphere2 = new Entity();
            Model sphereModel2 = Content.Load<Model>("sphere_mapped2");
            ModelComponent sphere2ModelComp = new ModelComponent { Model = sphereModel2 };
            TransformComponent sphere2TransComp = new TransformComponent { Scale = new Vector3(12f), Position = new Vector3(550, 150, -330), QuaternionRotation = Quaternion.Identity };
            sphere2.AddComponent(sphere2ModelComp);
            sphere2.AddComponent(sphere2TransComp);

            EffectComponent sphere2Effect = new EffectComponent();
            sphere2Effect.MeshEffects = new Dictionary<string, MeshEffect>();
            sphere2Effect.MeshEffects.Add("Sphere2", new MeshEffect
            {
                IsTransparent = false,
                IsShadow = false,
                Effect = effect.Clone(),
                Parameters = new List<string>
                {
                    "Matrices",
                    "AmbientLight",
                    "DirectionLight",
                    "SpecularLight",
                    "Shadow"
                },
                Settings = new Dictionary<string, object>
                {
                    {"ShadowIntensity", 0.5f}
                }
            });
            sphere2.AddComponent(sphere2Effect);

            //Sphere 3 - Shadow
            Entity sphere3 = new Entity();
            Model sphereModel3 = Content.Load<Model>("sphere_mapped3");
            ModelComponent sphere3ModelComp = new ModelComponent { Model = sphereModel3 };
            TransformComponent sphere3TransComp = new TransformComponent { Scale = new Vector3(12f), Position = new Vector3(575, 150, -330), QuaternionRotation = Quaternion.Identity };
            sphere3.AddComponent(sphere3ModelComp);
            sphere3.AddComponent(sphere3TransComp);

            EffectComponent sphere3Effect = new EffectComponent();
            sphere3Effect.MeshEffects = new Dictionary<string, MeshEffect>();
            sphere3Effect.MeshEffects.Add("Sphere", new MeshEffect
            {
                IsTransparent = false,
                IsShadow = false,
                Effect = effect.Clone(),
                Parameters = new List<string>
                {
                    "Matrices",
                    "AmbientLight",
                    "DirectionLight",
                    "SpecularLight",
                    "Shadow"
                },
                Settings = new Dictionary<string, object>
                {
                    {"ShadowIntensity", 0.5f}
                }
            });
            sphere3.AddComponent(sphere3Effect);

            //Sphere 4 - Non fog
            Entity sphere4 = new Entity();
            Model sphereModel4 = Content.Load<Model>("sphere_mapped4");
            ModelComponent sphere4ModelComp = new ModelComponent { Model = sphereModel4 };
            TransformComponent sphere4TransComp = new TransformComponent { Scale = new Vector3(12f), Position = new Vector3(500, 150, -330), QuaternionRotation = Quaternion.Identity };
            sphere4.AddComponent(sphere4ModelComp);
            sphere4.AddComponent(sphere4TransComp);

            EffectComponent sphere4Effect = new EffectComponent();
            sphere4Effect.MeshEffects = new Dictionary<string, MeshEffect>();
            sphere4Effect.MeshEffects.Add("Sphere", new MeshEffect
            {
                IsTransparent = false,
                IsShadow = true,
                Effect = effect.Clone(),
                Parameters = new List<string>
                {
                    "Matrices",
                    "AmbientLight",
                    "DirectionLight",
                    "SpecularLight",
                },
                Settings = new Dictionary<string, object>
                {
                    {"ShadowIntensity", 0.5f}
                }
            });
            sphere4.AddComponent(sphere4Effect);

            //Hangar 1 - Transparent roof
            Entity hangar1 = new Entity();
            ModelComponent hangar1ModelComp = new ModelComponent { Model = hangarModel };
            TransformComponent hangar1TransComp = new TransformComponent { Scale = new Vector3(0.01f), Position = new Vector3(600, 150, -400), QuaternionRotation = Quaternion.Identity };
            hangar1.AddComponent(hangar1ModelComp);
            hangar1.AddComponent(hangar1TransComp);

            EffectComponent hangar1Effect = new EffectComponent();
            hangar1Effect.MeshEffects = new Dictionary<string, MeshEffect>();
            hangar1Effect.MeshEffects.Add("object.002", new MeshEffect
            {
                IsTransparent = true,
                IsShadow = false,
                Effect = effect.Clone(),
                Parameters = new List<string>
                {
                    "Matrices",
                    "Transparency",
                    "AmbientLight",
                },
                Settings = new Dictionary<string, object>
                {

                }
            });
            hangar1.AddComponent(hangar1Effect);
            
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _systemHandler.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _systemHandler.Render(gameTime);
            base.Draw(gameTime);
        }
    }
}
