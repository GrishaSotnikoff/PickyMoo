// Source/System/InventorySystem.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PickyMoo.ESC;

namespace PickyMoo.Source.System.Player
{
    public class InventorySystem : ISystem
    {
        private KeyboardState _prevKb;

        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.I) && !_prevKb.IsKeyDown(Keys.I))
            {
                var inv = comps.OfType<InventoryComponent>().FirstOrDefault();
                if (inv != null)
                {
                    Console.WriteLine("=== INVENTORY ===");
                    foreach (var kv in inv.Items)
                        Console.WriteLine($"{kv.Key}: {kv.Value}");
                    Console.WriteLine("=================");
                }
            }
            _prevKb = kb;
        }
    }
}
