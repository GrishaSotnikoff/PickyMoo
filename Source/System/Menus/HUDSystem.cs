// Source/System/HUDSystem.cs
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PickyMoo.ESC;
using PickyMoo.Source.System.Player;

namespace PickyMoo.Source.System.Menus
{
    public class HUDSystem : ISystem
    {
        private readonly SpriteBatch _batch;
        private readonly SpriteFont _font;
        private readonly Dictionary<ToolType, Texture2D> _icons;
        private readonly int _iconSize;
        private readonly int _padding;

        public HUDSystem(
            SpriteBatch batch,
            SpriteFont font,
            Texture2D hoeIcon,
            Texture2D wateringCanIcon,
            Texture2D axeIcon,
            int iconSize = 32,
            int padding = 10
        )
        {
            _batch = batch;
            _font = font;
            _iconSize = iconSize;
            _padding = padding;
            _icons = new Dictionary<ToolType, Texture2D> {
                { ToolType.Hoe,         hoeIcon },
                { ToolType.WateringCan, wateringCanIcon },
                { ToolType.Axe,         axeIcon }
            };
        }

        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            // always try to draw the current tool
            var belt = comps.OfType<ToolbeltComponent>().FirstOrDefault();
            if (belt == null)
                return;        // nothing to draw if no toolbelt

            var inv = comps.OfType<InventoryComponent>().FirstOrDefault();

            _batch.Begin();

            // 1) tool icon
            var tool = belt.CurrentTool;
            var icon = _icons[tool];
            float scale = (float)_iconSize / icon.Width;
            _batch.Draw(icon,
                        new Vector2(_padding, _padding),
                        null, Color.White,
                        0f, Vector2.Zero,
                        scale,
                        SpriteEffects.None,
                        0f);

            // 2) if we have inventory, draw counts
            if (inv != null)
            {
                float textY = _padding + _iconSize + _padding;
                foreach (var kv in inv.Items)
                {
                    string line = $"{kv.Key}: {kv.Value}";
                    _batch.DrawString(_font,
                                      line,
                                      new Vector2(_padding, textY),
                                      Color.White);
                    textY += _font.LineSpacing + 2;
                }
            }

            _batch.End();
        }
    }
}
