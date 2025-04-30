using System;
using Microsoft.Xna.Framework;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    public class TransformComponent : IComponent
    {
        public Guid EntityId { get; set; }
        public Vector2 Position { get; set; }
    }
}
