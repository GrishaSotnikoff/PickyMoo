// Source/System/BuildModeComponent.cs
using System;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    public class BuildModeComponent : IComponent
    {
        public Guid EntityId { get; set; }
        public bool IsBuilding { get; set; } = false;
        public string StructureType { get; set; } = "shed"; // or "fence", etc.
    }
}
