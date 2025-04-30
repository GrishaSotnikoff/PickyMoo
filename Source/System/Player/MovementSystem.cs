using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PickyMoo.ESC;

namespace PickyMoo.Source.System.Player
{
    /// <summary>
    /// Reads keyboard state each frame and moves any entity
    /// with both InputComponent and TransformComponent.
    /// </summary>
    public class MovementSystem : ISystem
    {
        public void Update(List<IComponent> components, GameTime gameTime)
        {
            var inputs = components.OfType<InputComponent>();
            var transforms = components.OfType<TransformComponent>();
            var deltaSec = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var ks = Keyboard.GetState();

            foreach (var inp in inputs)
            {
                var t = transforms.FirstOrDefault(x => x.EntityId == inp.EntityId);
                if (t == null) continue;

                // build direction vector
                var dir = Vector2.Zero;
                if (ks.IsKeyDown(Keys.W) || ks.IsKeyDown(Keys.Up)) dir.Y -= 1;
                if (ks.IsKeyDown(Keys.S) || ks.IsKeyDown(Keys.Down)) dir.Y += 1;
                if (ks.IsKeyDown(Keys.A) || ks.IsKeyDown(Keys.Left)) dir.X -= 1;
                if (ks.IsKeyDown(Keys.D) || ks.IsKeyDown(Keys.Right)) dir.X += 1;

                if (dir != Vector2.Zero)
                {
                    dir.Normalize();
                    t.Position += dir * inp.Speed * deltaSec;
                }
            }
        }
    }
}
