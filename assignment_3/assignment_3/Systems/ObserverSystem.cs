using assignment_3.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.Systems
{
    public class ObserverSystem : UpdateSystem 
    {
        private MouseState _previousMouseState;
        private MouseState _currentMouseState;

        public override void Update(GameTime gameTime)
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            var observers = ComponentHandler.GetAllComponents<ObserverComponent>();

            if (observers == null)
                return;

            KeyboardState state = Keyboard.GetState();

            foreach (ObserverComponent observer in observers)
            {
                TransformComponent transform = ComponentHandler.GetComponent<TransformComponent>(observer.Owner);

                if (state.IsKeyDown(Keys.W))
                    transform.Position += transform.Forward * 1f;
                if (state.IsKeyDown(Keys.S))
                    transform.Position -= transform.Forward * 1f;
                if (state.IsKeyDown(Keys.A))
                    transform.Position += transform.Right * 1f;
                if (state.IsKeyDown(Keys.D))
                    transform.Position -= transform.Right * 1f;

                var mouseDelta = (_currentMouseState.Position - _previousMouseState.Position).ToVector2();
                transform.Rotation = new Vector3(-mouseDelta.Y, -mouseDelta.X, 0) * 0.05f;
            }
            
         }
    }
}
