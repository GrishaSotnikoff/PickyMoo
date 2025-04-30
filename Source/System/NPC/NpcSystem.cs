// Source/System/NpcSystem.cs
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    public class NpcSystem : ISystem
    {
        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            var npcs = comps.OfType<NpcComponent>().ToList();
            var transforms = comps.OfType<TransformComponent>().ToList();

            foreach (var npc in npcs)
            {
                if (npc.Waypoints.Count == 0) continue;
                var t = transforms.FirstOrDefault(x => x.EntityId == npc.EntityId);
                if (t == null) continue;

                var target = npc.Waypoints[npc.CurrentIndex];
                var dir = target - t.Position;
                var dist = dir.Length();

                if (dist < npc.ArrivalThreshold)
                {
                    npc.CurrentIndex = (npc.CurrentIndex + 1) % npc.Waypoints.Count;
                }
                else
                {
                    dir.Normalize();
                    t.Position += dir * npc.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }
    }
}
