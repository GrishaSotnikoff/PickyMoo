// Source/System/MinimapSystem.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PickyMoo.ESC;
using PickyMoo.Source.System.Player;
using PickyMoo.Source.System.World;

namespace PickyMoo.Source.System
{
    public class MinimapSystem : ISystem
    {
        private readonly SpriteBatch _batch;
        private readonly Texture2D _pixel;
        private readonly int _mapWidth;
        private readonly int _mapHeight;
        private readonly int _tileSize;
        private readonly int _miniW;
        private readonly int _miniH;
        private readonly Rectangle _minimapRect;

        public MinimapSystem(SpriteBatch batch, Texture2D pixel, int mapWidth, int mapHeight, int tileSize)
        {
            _batch = batch;
            _pixel = pixel;
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
            _tileSize = tileSize;
            _miniW = 120;
            _miniH = 120;
            _minimapRect = new Rectangle(10, 10, _miniW, _miniH);
        }

        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            _batch.Begin();

            // background
            _batch.Draw(_pixel, _minimapRect, new Color(0, 0, 0, 180));

            // draw player, NPCs, crops
            var transforms = comps.OfType<TransformComponent>().ToList();

            foreach (var t in transforms)
            {
                var miniX = _minimapRect.X + (int)((t.Position.X / (_mapWidth * _tileSize)) * _miniW);
                var miniY = _minimapRect.Y + (int)((t.Position.Y / (_mapHeight * _tileSize)) * _miniH);

                Color dotColor = Color.LightGreen;

                if (comps.OfType<InputComponent>().Any(c => c.EntityId == t.EntityId))
                    dotColor = Color.White;
                else if (comps.OfType<NpcComponent>().Any(c => c.EntityId == t.EntityId))
                    dotColor = Color.Orange;
                else if (comps.OfType<CropComponent>().Any(c => c.EntityId == t.EntityId))
                    dotColor = Color.ForestGreen;

                _batch.Draw(_pixel, new Rectangle(miniX, miniY, 2, 2), dotColor);
            }

            _batch.End();
        }
    }
}
