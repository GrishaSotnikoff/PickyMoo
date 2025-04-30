// Source/System/InteractionSystem.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PickyMoo.ESC;
using PickyMoo.Source.System;

namespace PickyMoo.Source.System
{
    /// <summary>
    /// E = plant if you’re wielding the Hoe;  
    /// harvest (reset + add to inventory) if crop exists.
    /// </summary>
    public class InteractionSystem : ISystem
    {
        private KeyboardState _prevKb;
        private readonly ECSManager _ecs;
        private readonly ContentManager _content;
        private readonly int _tileWidth, _tileHeight;

        public InteractionSystem(ECSManager ecs, ContentManager content, int tileWidth, int tileHeight)
        {
            _ecs = ecs;
            _content = content;
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;
        }

        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            // on E key-down edge
            if (kb.IsKeyDown(Keys.E))
            {
                // find player transform
                var input = comps.OfType<InputComponent>().FirstOrDefault();
                var transforms = comps.OfType<TransformComponent>().ToList();
                if (input == null) return;
                var pTx = transforms.First(t => t.EntityId == input.EntityId);

                // tile coords under player
                int tx = (int)(pTx.Position.X / _tileWidth);
                int ty = (int)(pTx.Position.Y / _tileHeight);
                var center = new Vector2(
                    tx * _tileWidth + _tileWidth / 2f,
                    ty * _tileHeight + _tileHeight / 2f
                );

                // only plant/harvest if holding the Hoe
                var belt = comps.OfType<ToolbeltComponent>().FirstOrDefault();
                if (belt?.CurrentTool != ToolType.Hoe)
                    return;

                // detect existing crop on this tile
                var crops = comps.OfType<CropComponent>();
                var cropTxs = transforms.Where(t => crops.Any(c => c.EntityId == t.EntityId));
                var existing = cropTxs.FirstOrDefault(t =>
                    (int)(t.Position.X / _tileWidth) == tx &&
                    (int)(t.Position.Y / _tileHeight) == ty
                );

                if (existing == null)
                {
                    // 🌱 PLANT
                    var crop = new Entity();

                    // Transform
                    var cTx = new TransformComponent
                    {
                        EntityId = crop.Id,
                        Position = center
                    };
                    crop.AddComponent(cTx);
                    _ecs.AddComponent(cTx);

                    // Sprite (stage 0)
                    var cSpr = new SpriteComponent
                    {
                        EntityId = crop.Id,
                        Texture = _content.Load<Texture2D>("cropStage0"),
                        Origin = new Vector2(16, 16),
                        SourceRectangle = null
                    };

                    var cropSize = new ScaleComponent {
                        EntityId = crop.Id,
                        Scale = new Vector2(0.1f)
                    };
                    crop.AddComponent(cropSize);
                    _ecs.AddComponent(cropSize);
                    crop.AddComponent(cSpr);
                    _ecs.AddComponent(cSpr);

                    // Crop logic
                    var durations = new float[] { 5f, 5f, 5f };
                    var stages = new List<Texture2D> {
                        _content.Load<Texture2D>("cropStage0"),
                        _content.Load<Texture2D>("cropStage1"),
                        _content.Load<Texture2D>("cropStage2"),
                        _content.Load<Texture2D>("cropStage3"),
                        _content.Load<Texture2D>("cropStage4")
                    };
                    var cComp = new CropComponent
                    {
                        EntityId = crop.Id,
                        StageDurations = durations,
                        StageTextures = stages
                    };
                    crop.AddComponent(cComp);
                    _ecs.AddComponent(cComp);

                    // Scale
                    var cScale = new ScaleComponent
                    {
                        EntityId = crop.Id,
                        Scale = new Vector2(0.5f)
                    };
                    crop.AddComponent(cScale);
                    _ecs.AddComponent(cScale);
                }
                else
                {
                    // 🚜 HARVEST (reset + inventory)
                    var cComp = comps.OfType<CropComponent>()
                                     .First(c => c.EntityId == existing.EntityId);
                    var cSpr = comps.OfType<SpriteComponent>()
                                     .First(s => s.EntityId == existing.EntityId);

                    // reset growth
                    cComp.CurrentStage = 0;
                    cComp.TimeInCurrentStage = 0f;
                    cSpr.Texture = cComp.StageTextures[0];

                    // add to inventory
                    var inv = comps.OfType<InventoryComponent>().FirstOrDefault();
                    if (inv != null)
                    {
                        inv.Items.TryGetValue("Crop", out var cnt);
                        inv.Items["Crop"] = cnt + 1;
                    }
                }
            }

            _prevKb = kb;
        }
    }
}
