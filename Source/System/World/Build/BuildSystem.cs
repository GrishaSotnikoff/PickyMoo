// Source/System/BuildSystem.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PickyMoo.ESC;
using PickyMoo.Source.System.Camera;
using PickyMoo.Source.System.Player;

namespace PickyMoo.Source.System
{
    /// <summary>
    /// Toggles build mode (B), shows a ghost preview,  
    /// checks collisions, deducts resources, and places structures.
    /// </summary>
    public class BuildSystem : ISystem
    {
        private readonly SpriteBatch _batch;
        private readonly Texture2D _ghostTexture;
        private readonly Texture2D _shedTexture;
        private readonly Camera2D _camera;
        private readonly int _tileSize;
        private readonly float _ghostScale;
        private KeyboardState _prevKb;
        private MouseState _prevMouse;

        public BuildSystem(
            SpriteBatch batch,
            Texture2D ghostTexture,
            Texture2D shedTexture,
            Camera2D camera,
            int tileSize,
            float ghostScale = 0.5f     // draw ghost at half‐size
        )
        {
            _batch = batch;
            _ghostTexture = ghostTexture;
            _shedTexture = shedTexture;
            _camera = camera;
            _tileSize = tileSize;
            _ghostScale = ghostScale;
        }

        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            var ms = Mouse.GetState();
            var builders = comps.OfType<BuildModeComponent>().ToList();
            var inv = comps.OfType<InventoryComponent>().FirstOrDefault();
            var transforms = comps.OfType<TransformComponent>().ToList();
            var colliders = comps.OfType<CollisionComponent>().ToList();

            foreach (var builder in builders)
            {
                // 1️⃣ Toggle build mode with B
                if (kb.IsKeyDown(Keys.B) && !_prevKb.IsKeyDown(Keys.B))
                    builder.IsBuilding = !builder.IsBuilding;

                if (!builder.IsBuilding)
                    continue;

                // 2️⃣ Compute tile‐aligned world position under mouse
                Vector2 screenPos = new Vector2(ms.X, ms.Y);
                Vector2 worldPos = Vector2.Transform(screenPos, Matrix.Invert(_camera.GetViewMatrix()));
                Vector2 tilePos = new Vector2(
                    (int)(worldPos.X / _tileSize) * _tileSize,
                    (int)(worldPos.Y / _tileSize) * _tileSize
                );

                // 3️⃣ Draw semi‐transparent ghost centered on tile
                _batch.Begin(transformMatrix: _camera.GetViewMatrix());
                _batch.Draw(
                    _ghostTexture,
                    tilePos + new Vector2(_tileSize / 2f, _tileSize / 2f),
                    null,
                    Color.White * 0.5f,
                    0f,
                    new Vector2(_ghostTexture.Width / 2f, _ghostTexture.Height / 2f),
                    _ghostScale,
                    SpriteEffects.None,
                    0f
                );
                _batch.End();

                // 4️⃣ On click, attempt to place
                if (ms.LeftButton == ButtonState.Pressed && _prevMouse.LeftButton == ButtonState.Released)
                {
                    // a) Compute proposed collision box
                    var newBounds = new Rectangle(
                        (int)(tilePos.X - _tileSize / 2),
                        (int)(tilePos.Y - _tileSize / 2),
                        _tileSize,
                        _tileSize
                    );

                    // b) Check against existing colliders
                    bool canPlace = true;
                    foreach (var oc in colliders)
                    {
                        var ot = transforms.First(t => t.EntityId == oc.EntityId);
                        var oBox = new Rectangle(
                            (int)(ot.Position.X + oc.LocalBounds.X),
                            (int)(ot.Position.Y + oc.LocalBounds.Y),
                            oc.LocalBounds.Width,
                            oc.LocalBounds.Height
                        );
                        if (newBounds.Intersects(oBox))
                        {
                            canPlace = false;
                            break;
                        }
                    }

                    if (!canPlace)
                    {
                        Console.WriteLine("🚧 Cannot build here—space occupied.");
                    }
                    // c) Check resource cost (5 Wood)
                    else if (inv != null && inv.Items.TryGetValue("Wood", out int wood) && wood >= 5)
                    {
                        inv.Items["Wood"] = wood - 5;

                        // d) Create structure entity
                        var structure = new Entity();

                        // Transform
                        var tx = new TransformComponent
                        {
                            EntityId = structure.Id,
                            Position = tilePos
                        };
                        structure.AddComponent(tx);
                        comps.Add(tx);

                        // Sprite
                        var spr = new SpriteComponent
                        {
                            EntityId = structure.Id,
                            Texture = _shedTexture,
                            Origin = Vector2.Zero
                        };
                        structure.AddComponent(spr);
                        comps.Add(spr);

                        // Scale
                        var scale = new ScaleComponent
                        {
                            EntityId = structure.Id,
                            Scale = Vector2.One
                        };
                        structure.AddComponent(scale);
                        comps.Add(scale);

                        // Collision: centered 32×32 box
                        var col = new CollisionComponent
                        {
                            EntityId = structure.Id,
                            LocalBounds = new Rectangle(-_tileSize / 2, -_tileSize / 2, _tileSize, _tileSize)
                        };
                        structure.AddComponent(col);
                        comps.Add(col);

                        Console.WriteLine("🏠 Placed a shed! (-5 Wood)");
                    }
                    else
                    {
                        Console.WriteLine("❌ Not enough wood to build.");
                    }
                }
            }

            _prevKb = kb;
            _prevMouse = ms;
        }
    }
}
