// Source/System/InventoryComponent.cs
using System;
using System.Collections.Generic;
using PickyMoo.ESC;

namespace PickyMoo.Source.System.Player
{
    public class InventoryComponent : IComponent
    {
        public Guid EntityId { get; set; }
        // e.g. { "Seed":5, "Crop":2 }
        public Dictionary<string, int> Items { get; set; }
            = new Dictionary<string, int>();
    }
}
