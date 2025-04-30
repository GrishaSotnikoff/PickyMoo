// Source/System/WateringSystem.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    /// <summary>
    /// Waters all crops within a tile‐radius around the player.
    /// </summary>
    public class WateringSystem : ISystem
    {
        private KeyboardState _prevKb;
        private readonly ECSManager _ecs;
        private readonly float _waterDuration;
        private readonly int _tileW, _tileH, _radius;

        /// <param name="radius">how many tiles out from player to water</param>
        public WateringSystem(ECSManager ecs, float waterDuration, int tileWidth, int tileHeight, int radius)
        {
            _ecs = ecs;
            _waterDuration = waterDuration;
            _tileW = tileWidth;
            _tileH = tileHeight;
            _radius = radius;
        }

        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.R) && !_prevKb.IsKeyDown(Keys.R))
            {
                // snapshot the crops so adding water won’t mutate this list
                var crops = comps.OfType<CropComponent>().ToList();

                // grab transforms once
                var transforms = comps.OfType<TransformComponent>().ToList();

                // center tile coords under player
                var inp = comps.OfType<InputComponent>().FirstOrDefault();
                if (inp == null) return;
                var pTx = transforms.First(t => t.EntityId == inp.EntityId);
                int centerX = (int)(pTx.Position.X / _tileW);
                int centerY = (int)(pTx.Position.Y / _tileH);

                foreach (var c in crops)
                {
                    var cTx = transforms.First(t => t.EntityId == c.EntityId);
                    int cx = (int)(cTx.Position.X / _tileW);
                    int cy = (int)(cTx.Position.Y / _tileH);

                    if (Math.Abs(cx - centerX) <= _radius && Math.Abs(cy - centerY) <= _radius)
                    {
                        var water = comps
                            .OfType<WaterComponent>()
                            .FirstOrDefault(w => w.EntityId == c.EntityId);

                        if (water == null)
                        {
                            water = new WaterComponent
                            {
                                EntityId = c.EntityId,
                                TimeRemaining = _waterDuration
                            };
                            _ecs.AddComponent(water);
                        }
                        else
                        {
                            water.TimeRemaining = _waterDuration;
                        }
                    }
                }
            }
            _prevKb = kb;
        }

    }
}
