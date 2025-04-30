// Source/System/WetnessMeterSystem.cs
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    /// <summary>
    /// Draws a little water‐level bar above each watered crop.
    /// </summary>
    public class WetnessMeterSystem : ISystem
    {
        private readonly SpriteBatch _batch;
        private readonly Camera2D _camera;
        private readonly Texture2D _pixel;
        private readonly int _tileW, _tileH;
        private readonly float _maxTime;
        private readonly int _barH = 5;
        private readonly int _offsetY;

        public WetnessMeterSystem(
            SpriteBatch batch,
            Camera2D camera,
            Texture2D pixel,
            int tileWidth,
            int tileHeight,
            float maxTime
        )
        {
            _batch = batch;
            _camera = camera;
            _pixel = pixel;
            _tileW = tileWidth;
            _tileH = tileHeight;
            _maxTime = maxTime;
            _offsetY = -(tileHeight / 2) - _barH - 2; // a few pixels above sprite
        }

        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            var waters = comps.OfType<WaterComponent>().ToList();
            var transforms = comps.OfType<TransformComponent>().ToList();

            _batch.Begin(transformMatrix: _camera.GetViewMatrix());
            foreach (var w in waters)
            {
                var tx = transforms.First(t => t.EntityId == w.EntityId);
                var pos = tx.Position;
                var left = (int)(pos.X - _tileW / 2);
                var top = (int)(pos.Y + _offsetY);

                // percentage of water left
                float pct = MathHelper.Clamp(w.TimeRemaining / _maxTime, 0f, 1f);
                int fullW = _tileW;
                int fillW = (int)(fullW * pct);

                // gray background
                _batch.Draw(_pixel,
                    new Rectangle(left, top, fullW, _barH),
                    Color.Gray);

                // blue fill
                _batch.Draw(_pixel,
                    new Rectangle(left, top, fillW, _barH),
                    Color.CornflowerBlue);
            }
            _batch.End();
        }
    }
}
