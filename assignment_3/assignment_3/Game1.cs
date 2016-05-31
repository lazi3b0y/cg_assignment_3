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
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
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

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _settings = new Settings
            {
                AmbientColor = Color.White,
                AmbientIntensity = 0.3f,
                DiffuseColor = Color.White,
                DiffuseIntensity = 0.5f,
                DiffuseLightDirection = new Vector3(1, 1, 0),
                FogStart = 30f,
                FogEnd = 70f,
                FogColor = Color.Black
            };

            _effectProvider = new EffectProvider();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            ModelRenderSystem mDR = new ModelRenderSystem(_graphics.GraphicsDevice, _effectProvider, _componentHandler, _cameraHandler);
            _systemHandler.AddSystem(new ObserverSystem());
            _systemHandler.AddSystem(new ChaseCameraSystem());
            _systemHandler.AddSystem(new CameraSystem());
            _systemHandler.AddSystem(new TransformSystem());
            _systemHandler.AddSystem(new EnvironmentSystem(_graphics.GraphicsDevice, _cameraHandler, mDR, _componentHandler));
            _systemHandler.AddSystem(mDR);

            _effectProvider.AddEffectProcessor("Matrices", new MatrixEffectProcessor(_cameraHandler, _componentHandler));
            _effectProvider.AddEffectProcessor("Reflection", new ReflectionEffectProcessor(_componentHandler, _cameraHandler));
            _effectProvider.AddEffectProcessor("Transparency", new TransparencyProcessor(_componentHandler));
            _effectProvider.AddEffectProcessor("DirectionLight", new DirectionLightEffectProcessor(_settings));
            _effectProvider.AddEffectProcessor("AmbientLight", new AmbientLightEffectProcessor(_settings));
            _effectProvider.AddEffectProcessor("Specular", new SpecularLightEffectProcessor(_componentHandler));
            _effectProvider.AddEffectProcessor("BumpMapping", new BumpMappingEffectProcessor(_componentHandler));
            _effectProvider.AddEffectProcessor("Fog", new FogEffectProcessor(_settings, _cameraHandler));

            CameraComponent camera = new CameraComponent { CameraNearPlaneDistance = 0.1f, CameraFarPlaneDistance = 500f, CameraAspectRatio = _graphics.GraphicsDevice.Viewport.AspectRatio };
            ChaseCameraComponent chaseCamera = new ChaseCameraComponent { CameraOffset = new Vector3(0, 0.1f, 1) };
            TransformComponent observerTransform = new TransformComponent { Scale = new Vector3(0.001f), Position = new Vector3(600, 150, -450), QuaternionRotation = Quaternion.Identity };
            ObserverComponent observerComponent = new ObserverComponent();

            Entity observer = new Entity();
            Entity hangar1 = new Entity();
            Entity snowplow1 = new Entity();

            _cameraHandler.ActiveCamera = camera;

            observer.AddComponent(camera);
            observer.AddComponent(chaseCamera);
            observer.AddComponent(observerTransform);
            observer.AddComponent(observerComponent);

            //Load 3d texturized model files
            Model hangarModel = Content.Load<Model>("moffett-hangar2");
            Model snowplowModel = Content.Load<Model>("PlayerSphere");
            var effect = Content.Load<Effect>("BumpRefMapShader");

            ModelComponent snowplow1Comp = new ModelComponent { Model = snowplowModel };
            TransformComponent snowplow1Trans = new TransformComponent { Scale = new Vector3(0.1f), Position = new Vector3(600, 150, -300), QuaternionRotation = Quaternion.Identity };
            snowplow1.AddComponent(snowplow1Comp);
            snowplow1.AddComponent(snowplow1Trans);

            ModelComponent hangar1Comp = new ModelComponent { Model = hangarModel };
            TransformComponent hangar1Trans = new TransformComponent { Scale = new Vector3(0.2f), Position = new Vector3(600, 150, -500), QuaternionRotation = Quaternion.Identity };
            hangar1.AddComponent(hangar1Comp);
            hangar1.AddComponent(hangar1Trans);

            EffectComponent hangar1EffectComponent = new EffectComponent();
            hangar1EffectComponent.MeshEffects = new Dictionary<string, MeshEffect>();
            hangar1EffectComponent.MeshEffects.Add("pSphere1", new MeshEffect
            {
                IsTransparent = true,
                IsReflective = false,
                Effect = effect.Clone(),
                Parameters = new List<string>
                {
                    "Matrices",
                    "AmbientLight",
                    "Transparency"
                },
                Settings = new Dictionary<string, object>
                {
                    ["TransparencyAmount"] = .7f,
                    ["ReflectionAmount"] = 1f,
                    ["EnviromentMap"] = null,
                }
            });


            snowplow1.AddComponent(hangar1EffectComponent);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _systemHandler.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _systemHandler.Render(gameTime);
            base.Draw(gameTime);
        }
    }
}
