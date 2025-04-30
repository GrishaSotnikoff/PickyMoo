using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    public class TilemapSystem : ISystem
    {
        private readonly SpriteBatch _batch;
        private readonly Camera2D _camera;

        public TilemapSystem(SpriteBatch batch, Camera2D camera)
        {
            _batch = batch;
            _camera = camera;
        }

        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            var maps = comps.OfType<TilemapComponent>();
            foreach (var map in maps)
            {
                int rows = map.Map.GetLength(0);
                int cols = map.Map.GetLength(1);
                int tilesPerRow = map.Tileset.Width / map.TileWidth;

                _batch.Begin(transformMatrix: _camera.GetViewMatrix());
                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < cols; x++)
                    {
                        int id = map.Map[y, x];
                        // compute source rectangle in atlas
                        int sx = (id % tilesPerRow) * map.TileWidth;
                        int sy = (id / tilesPerRow) * map.TileHeight;
                        var src = new Rectangle(sx, sy, map.TileWidth, map.TileHeight);

                        // world position
                        var dest = new Rectangle(
                            x * map.TileWidth,
                            y * map.TileHeight,
                            map.TileWidth,
                            map.TileHeight
                        );

                        _batch.Draw(map.Tileset, dest, src, Color.White);
                    }
                }
                _batch.End();
            }
        }
    }
}
