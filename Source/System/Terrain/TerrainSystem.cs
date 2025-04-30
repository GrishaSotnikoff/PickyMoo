// Source/System/TerrainSystem.cs
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PickyMoo.ESC;
using PickyMoo.Source.System.Camera;

namespace PickyMoo.Source.System
{
    public class TerrainSystem : ISystem
    {
        private readonly SpriteBatch _batch;
        private readonly Camera2D _camera;
        private readonly Dictionary<string, int> _tileMap; // type -> tile index
        private readonly LocationManager _locationManager;

        public TerrainSystem(SpriteBatch batch, Camera2D camera, LocationManager locMgr)
        {
            _batch = batch;
            _camera = camera;
            _locationManager = locMgr;
            _tileMap = new Dictionary<string, int>
    {
        { "grass", 0 },
        { "dirt",  1 },
        { "water", 2 },
        { "path",  3 },
        { "tilled", 4 },   // optional for hoed soil
        { "dry",    5 },   // optional for dried out dirt
        { "stone",  6 },   // optional for decorations
        { "sand",   7 }    // for beach
    };
        }


        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            var terrain = _locationManager.GetCurrentTerrain();
            if (terrain == null) return;
            var map = terrain.TileTypes;
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            _batch.Begin(transformMatrix: _camera.GetViewMatrix());
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    string type = map[y, x];
                    if(type == null) continue; // no tilemap available
                    if (!_tileMap.TryGetValue(type, out int index)) continue;

                    int sx = (index % terrain.TilesPerRow) * terrain.TileSize;
                    int sy = (index / terrain.TilesPerRow) * terrain.TileSize;

                    var src = new Rectangle(sx, sy, terrain.TileSize, terrain.TileSize);
                    var dest = new Vector2(x * terrain.TileSize, y * terrain.TileSize);

                    _batch.Draw(terrain.Tileset, dest, src, Color.White);
                }
            }
            _batch.End();
        }
    }
}
