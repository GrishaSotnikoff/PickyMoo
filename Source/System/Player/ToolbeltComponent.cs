// Source/System/ToolbeltComponent.cs
using System;
using System.Collections.Generic;
using PickyMoo.ESC;

namespace PickyMoo.Source.System.Player
{
    public enum ToolType { Hoe, WateringCan, Axe }

    public class ToolbeltComponent : IComponent
    {
        public Guid EntityId { get; set; }
        public List<ToolType> Tools { get; set; }
            = new List<ToolType> { ToolType.Hoe, ToolType.WateringCan, ToolType.Axe };
        public int SelectedIndex { get; set; } = 0;
        public ToolType CurrentTool => Tools[SelectedIndex];
    }
}
