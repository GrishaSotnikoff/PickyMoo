// Source/System/TreeComponent.cs
using System;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    /// <summary>
    /// Marks an entity as a tree that can be chopped for wood.
    /// </summary>
    public class TreeComponent : IComponent
    {
        public Guid EntityId { get; set; }
    }
}
