// Source/System/SellingSystem.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using PickyMoo.ESC;
using PickyMoo.Source.System.Menus;

namespace PickyMoo.Source.System.Player
{
    public class SellingSystem : ISystem
    {
        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            var menus = comps.OfType<MenuComponent>().Where(m => m.IsOpen);
            var inv = comps.OfType<InventoryComponent>().FirstOrDefault();

            foreach (var menu in menus)
            {
                if (menu.Options[menu.SelectedIndex] == "Selling")
                {
                    if (inv != null && inv.Items.TryGetValue("Crop", out int cropCount) && cropCount > 0)
                    {
                        inv.Items.Remove("Crop");
                        inv.Items.TryGetValue("Gold", out int gold);
                        inv.Items["Gold"] = gold + cropCount * 5;

                        menu.IsOpen = false;
                        Console.WriteLine($"💰 Sold {cropCount} crops for {cropCount * 5} gold!");
                    }
                    else
                    {
                        Console.WriteLine("😢 Nothing to sell.");
                    }
                }
            }
        }
    }
}
