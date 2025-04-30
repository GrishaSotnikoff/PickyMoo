// Source/System/MenuSystem.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PickyMoo.ESC;

namespace PickyMoo.Source.System.Menus
{
    /// <summary>
    /// Toggles (M), navigates (↑↓), and draws a popup menu.
    /// </summary>
    public class MenuSystem : ISystem
    {
        private KeyboardState _prevKb;
        private readonly SpriteBatch _batch;
        private readonly SpriteFont _font;
        private readonly Texture2D _pixel;
        private readonly int _pad;

        public MenuSystem(
            SpriteBatch batch,
            SpriteFont font,
            Texture2D pixel,
            int padding = 10
        )
        {
            _batch = batch;
            _font = font;
            _pixel = pixel;
            _pad = padding;
        }

        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            var menu = comps.OfType<MenuComponent>().FirstOrDefault();
            if (menu == null) return;

            var kb = Keyboard.GetState();
            // toggle
            if (kb.IsKeyDown(Keys.M) && !_prevKb.IsKeyDown(Keys.M))
                menu.IsOpen = !menu.IsOpen;

            if (menu.IsOpen)
            {
                // navigate
                if (kb.IsKeyDown(Keys.Up) && !_prevKb.IsKeyDown(Keys.Up))
                    menu.SelectedIndex =
                        (menu.SelectedIndex + menu.Options.Count - 1)
                        % menu.Options.Count;
                if (kb.IsKeyDown(Keys.Down) && !_prevKb.IsKeyDown(Keys.Down))
                    menu.SelectedIndex =
                        (menu.SelectedIndex + 1)
                        % menu.Options.Count;

                // select
                if (kb.IsKeyDown(Keys.Enter) && !_prevKb.IsKeyDown(Keys.Enter))
                {
                    string choice = menu.Options[menu.SelectedIndex];
                    // placeholder: in future hook real actions
                    Console.WriteLine($"🌟 Menu selected: {choice}");
                }

                // draw the popup
                int w = 200;
                int h = menu.Options.Count * (_font.LineSpacing + 5) + _pad * 2;
                int x = _pad * 2, y = _pad * 2;

                _batch.Begin();
                // semi‐opaque background
                _batch.Draw(_pixel, new Rectangle(x, y, w, h), new Color(0, 0, 0, 180));
                // options
                for (int i = 0; i < menu.Options.Count; i++)
                {
                    var col = i == menu.SelectedIndex ? Color.Yellow : Color.White;
                    _batch.DrawString(
                        _font,
                        menu.Options[i],
                        new Vector2(x + _pad, y + _pad + i * (_font.LineSpacing + 5)),
                        col
                    );
                }
                _batch.End();
            }

            _prevKb = kb;
        }
    }
}
