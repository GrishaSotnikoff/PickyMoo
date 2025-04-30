using System;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    /// <summary>
    /// Marks an entity as player‐controllable.
    /// Speed is in units per second.
    /// </summary>
    public class InputComponent : IComponent
    {
        public Guid EntityId { get; set; }
        public float Speed { get; set; } = 100f;
    }
}
