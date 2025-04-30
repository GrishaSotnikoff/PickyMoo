// Source/System/CraftingSystem.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using PickyMoo.ESC;
using PickyMoo.Source.System.Menus;
using PickyMoo.Source.System.Player;

namespace PickyMoo.Source.System.World
{
    public class CraftingSystem : ISystem
    {
        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            var menus = comps.OfType<MenuComponent>().Where(m => m.IsOpen);
            var inv = comps.OfType<InventoryComponent>().FirstOrDefault();

            foreach (var menu in menus)
            {
                if (menu.Options[menu.SelectedIndex] == "Crafting")
                {
                    if (inv != null && inv.Items.TryGetValue("Crop", out int cropCount) && cropCount >= 3)
                    {
                        inv.Items["Crop"] -= 3;
                        inv.Items.TryGetValue("Seed", out int seedCount);
                        inv.Items["Seed"] = seedCount + 1;

                        menu.IsOpen = false;
                        Console.WriteLine("🛠️ Crafted 1 Seed from 3 Crops!");
                    }
                    else
                    {
                        Console.WriteLine("❌ Not enough crops to craft.");
                    }
                }
            }
        }
    }
}
