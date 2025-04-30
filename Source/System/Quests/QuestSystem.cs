// Source/System/QuestSystem.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using PickyMoo.ESC;
using PickyMoo.Source.System.Player;

namespace PickyMoo.Source.System
{
    public class QuestSystem : ISystem
    {
        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            var questLogs = comps.OfType<QuestLogComponent>().ToList();
            var inventories = comps.OfType<InventoryComponent>().ToList();

            foreach (var log in questLogs)
            {
                foreach (var q in log.ActiveQuests.Values.Where(q => !q.IsCompleted).ToList())
                {
                    switch (q.ObjectiveType)
                    {
                        case QuestObjectiveType.DeliverItem:
                            var inv = inventories.FirstOrDefault(i => i.EntityId == log.EntityId);
                            if (inv != null && inv.Items.TryGetValue(q.RequiredItem, out int count) && count >= q.RequiredAmount)
                            {
                                inv.Items[q.RequiredItem] = count;
// (continuation of QuestSystem.cs)
                                // Complete quest: remove items
                                inv.Items[q.RequiredItem] -= q.RequiredAmount;
                                if (inv.Items[q.RequiredItem] <= 0)
                                    inv.Items.Remove(q.RequiredItem);

                                // Give rewards
                                foreach (var reward in q.Rewards)
                                {
                                    inv.Items.TryGetValue(reward.Key, out int existing);
                                    inv.Items[reward.Key] = existing + reward.Value;
                                }

                                // Mark complete
                                q.IsCompleted = true;
                                log.CompletedQuests.Add(q.QuestId);

                               Console.WriteLine($"✅ Quest Complete: {q.Title}");
                            }
                            break;

                        case QuestObjectiveType.VisitLocation:
                            // Example: implement based on location manager
                            var playerTransform = comps.OfType<TransformComponent>()
                                .FirstOrDefault(t => t.EntityId == log.EntityId);
                            if (playerTransform != null && q.Target == "Forest")
                            {
                                // Add logic: check proximity or active location
                                // Example logic:
                                if (LocationManager.CurrentLocation == q.Target)
                                {
                                    foreach (var reward in q.Rewards)
                                    {
                                        inventories[0].Items.TryGetValue(reward.Key, out int existing);
                                        inventories[0].Items[reward.Key] = existing + reward.Value;
                                    }

                                    q.IsCompleted = true;
                                    log.CompletedQuests.Add(q.QuestId);
                                    Console.WriteLine($"📍 Quest Complete: {q.Title}");
                                }
                            }
                            break;

                        case QuestObjectiveType.TalkToNPC:
                            // Handle in NPCInteractionSystem.cs when player talks to specific NPC
                            break;
                    }
                }
            }
        }
    }
}
