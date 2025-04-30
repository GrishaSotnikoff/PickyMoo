// Source/System/SpriteComponent.cs
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    public class SpriteComponent : IComponent
    {
        public Guid EntityId { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle? SourceRectangle { get; set; }
        public Vector2 Origin { get; set; } = Vector2.Zero;    // pivot point
        public float Rotation { get; set; } = 0f;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;
        public float LayerDepth { get; set; } = 0f;
    }
}
