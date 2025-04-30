// Source/System/NpcComponent.cs
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    public class NpcComponent : IComponent
    {
        public Guid EntityId { get; set; }

        public List<Vector2> Waypoints { get; set; } = new();
        public int CurrentIndex { get; set; } = 0;

        public float Speed { get; set; } = 40f;
        public float ArrivalThreshold { get; set; } = 4f;

        public bool WantsToBuy { get; set; } = false;
        public bool HasQuest { get; set; } = false;
        public string QuestText { get; set; } = null;
    }
}
