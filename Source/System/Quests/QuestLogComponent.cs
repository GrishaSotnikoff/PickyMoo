// Source/System/QuestLogComponent.cs
using System;
using System.Collections.Generic;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    public class QuestLogComponent : IComponent
    {
        public Guid EntityId { get; set; }
        public Dictionary<string, QuestComponent> ActiveQuests { get; set; } = new();
        public List<string> CompletedQuests { get; set; } = new();
    }
}
