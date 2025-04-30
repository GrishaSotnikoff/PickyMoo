// Source/System/MenuComponent.cs
using System;
using System.Collections.Generic;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    /// <summary>
    /// Tracks state for a simple toggleable menu.
    /// </summary>
    public class MenuComponent : IComponent
    {
        public Guid EntityId { get; set; }
        public bool IsOpen { get; set; } = false;
        public int SelectedIndex { get; set; } = 0;
        public List<string> Options { get; set; } = new List<string> {
            "Crafting", "Selling", "Quests"
        };
    }
}
