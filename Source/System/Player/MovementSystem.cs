// Source/System/MovementSystem.cs
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PickyMoo.ESC;
using PickyMoo.Source.System.Player;

namespace PickyMoo.Source.System.Player
{
    public class MovementSystem : ISystem
    {
        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var ks = Keyboard.GetState();

            // Cache component lists
            var inputs = comps.OfType<InputComponent>().ToList();
            var transforms = comps.OfType<TransformComponent>().ToList();
            var colliders = comps.OfType<CollisionComponent>().ToList();

            foreach (var inp in inputs)
            {
                var t = transforms.First(x => x.EntityId == inp.EntityId);
                var dir = Vector2.Zero;
                if (ks.IsKeyDown(Keys.W)) dir.Y -= 1;
                if (ks.IsKeyDown(Keys.S)) dir.Y += 1;
                if (ks.IsKeyDown(Keys.A)) dir.X -= 1;
                if (ks.IsKeyDown(Keys.D)) dir.X += 1;
                if (dir == Vector2.Zero) continue;

                dir.Normalize();
                var proposed = t.Position + dir * inp.Speed * delta;

                // Build the moving entity’s proposed hitbox
                var selfCol = colliders.FirstOrDefault(c => c.EntityId == inp.EntityId);
                Rectangle selfBox;
                if (selfCol != null)
                {
                    selfBox = new Rectangle(
                        (int)(proposed.X + selfCol.LocalBounds.X),
                        (int)(proposed.Y + selfCol.LocalBounds.Y),
                        selfCol.LocalBounds.Width,
                        selfCol.LocalBounds.Height
                    );
                }
                else
                {
                    // no collider = free to move
                    t.Position = proposed;
                    continue;
                }

                // Check against all other colliders
                bool hit = false;
                foreach (var oc in colliders.Where(c => c.EntityId != inp.EntityId))
                {
                    var ot = transforms.First(x => x.EntityId == oc.EntityId);
                    var oBox = new Rectangle(
                        (int)(ot.Position.X + oc.LocalBounds.X),
                        (int)(ot.Position.Y + oc.LocalBounds.Y),
                        oc.LocalBounds.Width,
                        oc.LocalBounds.Height
                    );
                    if (selfBox.Intersects(oBox))
                    {
                        hit = true;
                        break;
                    }
                }

                if (!hit)
                {
                    t.Position = proposed;
                }
                // else block movement
            }
        }
    }
}
