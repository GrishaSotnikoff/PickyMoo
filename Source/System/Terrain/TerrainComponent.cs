// Source/System/TerrainComponent.cs
using System;
using Microsoft.Xna.Framework.Graphics;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    public class TerrainComponent : IComponent
    {
        public Guid EntityId { get; set; }
        public string[,] TileTypes { get; set; }
        public Texture2D Tileset { get; set; }
        public int TileSize { get; set; }
        public int TilesPerRow { get; set; }
    }
}
