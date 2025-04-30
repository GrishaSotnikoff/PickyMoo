using System;
using Microsoft.Xna.Framework.Graphics;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    /// <summary>
    /// Holds a tile ID grid plus tileset info.
    /// </summary>
    public class TilemapComponent : IComponent
    {
        public Guid EntityId { get; set; }
        public int[,] Map { get; set; }    // [rows, cols] of tile indices
        public Texture2D Tileset { get; set; }    // atlas texture
        public int TileWidth { get; set; }    // e.g. 32
        public int TileHeight { get; set; }    // e.g. 32
    }
}
