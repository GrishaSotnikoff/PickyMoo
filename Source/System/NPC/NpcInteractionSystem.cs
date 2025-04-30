// Source/System/NpcInteractionSystem.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PickyMoo.ESC;
using PickyMoo.Source.System.Player;

namespace PickyMoo.Source.System
{
    public class NpcInteractionSystem : ISystem
    {
        private KeyboardState _prevKb;

        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Space) && !_prevKb.IsKeyDown(Keys.Space))
            {
                var player = comps.OfType<InputComponent>().FirstOrDefault();
                var transforms = comps.OfType<TransformComponent>().ToList();
                var npcs = comps.OfType<NpcComponent>().ToList();
                var inv = comps.OfType<InventoryComponent>().FirstOrDefault();

                var pTx = transforms.First(t => t.EntityId == player.EntityId);

                foreach (var npc in npcs)
                {
                    var npcTx = transforms.First(t => t.EntityId == npc.EntityId);
                    if (Vector2.Distance(npcTx.Position, pTx.Position) < 32)
                    {
                        // after checking player-NPC distance
                        var quest = comps.OfType<QuestComponent>()
                                         .FirstOrDefault(q => q.EntityId == npc.EntityId);

                        var log = comps.OfType<QuestLogComponent>().FirstOrDefault(l => l.EntityId == player.EntityId);

                        if (quest != null && log != null && !log.ActiveQuests.ContainsKey(quest.QuestId) && !quest.IsCompleted)
                        {
                            log.ActiveQuests.Add(quest.QuestId, quest);
                            Console.WriteLine($"📝 New Quest: {quest.Title} - {quest.Description}");
                        }

                        if (npc.WantsToBuy && inv.Items.TryGetValue("Crop", out int count) && count > 0)
                        {
                            inv.Items["Crop"] = 0;
                            inv.Items.TryGetValue("Gold", out int g);
                            inv.Items["Gold"] = g + count * 5;
                            Console.WriteLine($"🧑‍🌾 NPC bought {count} crops for {count * 5}g.");
                        }
                        if (npc.HasQuest)
                        {
                            Console.WriteLine($"📜 Quest: {npc.QuestText}");
                        }
                    }
                }
            }

            _prevKb = kb;
        }
    }
}
