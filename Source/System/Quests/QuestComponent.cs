// Source/System/QuestComponent.cs
using System;
using System.Collections.Generic;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    public enum QuestObjectiveType
    {
        DeliverItem,
        VisitLocation,
        TalkToNPC
    }

    public class QuestComponent : IComponent
    {
        public Guid EntityId { get; set; }

        public string QuestId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public QuestObjectiveType ObjectiveType { get; set; }

        // Used for DeliverItem
        public string RequiredItem { get; set; }
        public int RequiredAmount { get; set; }

        // Used for VisitLocation or TalkToNPC
        public string Target { get; set; }

        public Dictionary<string, int> Rewards { get; set; } = new();
        public bool IsCompleted { get; set; } = false;
    }
}
