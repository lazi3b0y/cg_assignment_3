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
        public override void Update(GameTime gameTime)
        {
            var observers = ComponentHandler.GetAllComponents<ObserverComponent>();

            if (observers == null)
                return;

            KeyboardState state = Keyboard.GetState();

            foreach (ObserverComponent observer in observers)
            {
                TransformComponent transform = ComponentHandler.GetComponent<TransformComponent>(observer.Owner);

                Vector3 rotation = Vector3.Zero;

                if (state.IsKeyDown(Keys.W))
                    rotation.X += -0.02f;
                if (state.IsKeyDown(Keys.A))
                    rotation.Y += 0.02f;
                if (state.IsKeyDown(Keys.S))
                    rotation.X += 0.02f;
                if (state.IsKeyDown(Keys.D))
                    rotation.Y += -0.02f;
                if (state.IsKeyDown(Keys.Up))
                    transform.Position += transform.Forward * 1f;
                if (state.IsKeyDown(Keys.Down))
                    transform.Position -= transform.Forward * 1f;
                if (state.IsKeyDown(Keys.Right))
                    rotation.Z = -0.02f;
                if (state.IsKeyDown(Keys.Left))
                    rotation.Z = 0.02f;

                transform.Rotation = rotation;
            }
        }
    }
}
