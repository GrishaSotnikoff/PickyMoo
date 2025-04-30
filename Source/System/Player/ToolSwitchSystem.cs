// Source/System/ToolSwitchSystem.cs
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PickyMoo.ESC;

namespace PickyMoo.Source.System.Player
{
    public class ToolSwitchSystem : ISystem
    {
        private KeyboardState _prevKb;

        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            var belts = comps.OfType<ToolbeltComponent>().ToList();
            foreach (var belt in belts)
            {
                // map 1->index0, 2->1, 3->2
                if (kb.IsKeyDown(Keys.D1) && !_prevKb.IsKeyDown(Keys.D1)) belt.SelectedIndex = 0;
                if (kb.IsKeyDown(Keys.D2) && !_prevKb.IsKeyDown(Keys.D2) && belt.Tools.Count > 1) belt.SelectedIndex = 1;
                if (kb.IsKeyDown(Keys.D3) && !_prevKb.IsKeyDown(Keys.D3) && belt.Tools.Count > 2) belt.SelectedIndex = 2;
            }
            _prevKb = kb;
        }
    }
}
