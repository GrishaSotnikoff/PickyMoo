// Source/System/MapTransitionSystem.cs
using Microsoft.Xna.Framework;
using PickyMoo.ESC;
using PickyMoo.Source.System.Player;
using System.Collections.Generic;
using System.Linq;

namespace PickyMoo.Source.System
{
    public class MapTransitionSystem : ISystem
    {
        private readonly LocationManager _locations;
        private readonly int _mapWidth;
        private readonly int _mapHeight;
        private readonly int _tileSize;
        private readonly ScreenFadeSystem _fade;

        public MapTransitionSystem(LocationManager locations, int mapWidth, int mapHeight, int tileSize, ScreenFadeSystem fade)
        {
            _locations = locations;
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
            _tileSize = tileSize;
            _fade = fade;
        }

        public void Update(List<IComponent> comps, GameTime gameTime)
        {

            var input = comps.OfType<InputComponent>().FirstOrDefault();
            var tx = comps.OfType<TransformComponent>().First(t => t.EntityId == input.EntityId);
            var pos = tx.Position;
            
            int worldW = _mapWidth * _tileSize;
            int worldH = _mapHeight * _tileSize;

            if (pos.X < 0)
            {
                pos.X = worldW - _tileSize;
                _fade.FadeTo("Forest");
            }
            else if (pos.X > worldW)
            {
                pos.X = _tileSize;
                _fade.FadeTo("Town");
            }
            else if (pos.Y < 0)
            {
                pos.Y = worldH - _tileSize;
                _fade.FadeTo("Beach");
            }
            else if (pos.Y > worldH)
            {
                pos.Y = _tileSize;
                _fade.FadeTo("Farm");
            }

            tx.Position = pos;

        }
    }
}
