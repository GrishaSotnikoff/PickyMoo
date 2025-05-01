// Source/System/CollisionComponent.cs
using System;
using Microsoft.Xna.Framework;
using PickyMoo.ESC;

namespace PickyMoo.Source.System.Player
{
    /// <summary>
    /// Marks an entity as solid. 
    /// Uses a local rectangle (offset from Transform.Position).
    /// </summary>
    public class CollisionComponent : IComponent
    {
        public Guid EntityId { get; set; }

        /// <summary>
        /// Bounds relative to the entity’s position.
        /// e.g. new Rectangle(-16, -16, 32, 32) for a centered 32×32 hitbox.
        /// </summary>
        public Rectangle LocalBounds { get; set; }
    }
}
