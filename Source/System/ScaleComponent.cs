using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Source/System/ScaleComponent.cs
using System;
using Microsoft.Xna.Framework;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    public class ScaleComponent : IComponent
    {
        public Guid EntityId { get; set; }
        /// <summary>
        /// (1,1) = original size. (0.5f,0.5f) = half-size.
        /// </summary>
        public Vector2 Scale { get; set; } = Vector2.One;
    }
}
