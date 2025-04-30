using System;
using PickyMoo.ESC;

namespace PickyMoo.Source.System.World
{
    /// <summary>
    /// Tracks water status on crops: how long until they dry out.
    /// </summary>
    public class WaterComponent : IComponent
    {
        public Guid EntityId { get; set; }
        /// <summary>
        /// Seconds left until this crop goes bone-dry.
        /// </summary>
        public float TimeRemaining { get; set; }
    }
}
