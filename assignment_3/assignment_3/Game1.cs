using assignment_3.Components;
using assignment_3.Entites;
using assignment_3.Handlers;
using assignment_3.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _systemHandler.AddSystem(new ObserverSystem());
            _systemHandler.AddSystem(new CameraSystem());
            _systemHandler.AddSystem(new TransformSystem());
            _systemHandler.AddSystem(new ModelRenderSystem(_graphics.GraphicsDevice));

            CameraComponent camera = new CameraComponent { CameraNearPlaneDistance = 0.1f, CameraFarPlaneDistance = 500f, CameraAspectRatio = _graphics.GraphicsDevice.Viewport.AspectRatio };
            TransformComponent observerTransform = new TransformComponent { Scale = new Vector3(0.001f), Position = new Vector3(600, 150, -450), QuaternionRotation = Quaternion.Identity };
            ObserverComponent observerComponent = new ObserverComponent();

            Entity observer = new Entity();

            Model hangarModel = Content.Load<Model>("moffett-hangar2");

            Entity hangar1 = new Entity();
            ModelComponent hangar1Comp = new ModelComponent { Model = hangarModel };
            TransformComponent hangar1Trans = new TransformComponent { Scale = new Vector3(0.001f), Position = new Vector3(600, 150, -425), QuaternionRotation = Quaternion.Identity };
            hangar1.AddComponent(hangar1Comp);
            hangar1.AddComponent(hangar1Trans);

            _cameraHandler.ActiveCamera = camera;

            observer.AddComponent(camera);
            observer.AddComponent(observerTransform);
            observer.AddComponent(observerComponent);

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
