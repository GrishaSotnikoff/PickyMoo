// Source/System/InteractionSystem.cs
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    /// <summary>
    /// E = plant if empty; if crop exists then harvest (reset to stage 0).
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
            // on E key down edge
            if (kb.IsKeyDown(Keys.E) && !_prevKb.IsKeyDown(Keys.E))
            {
                // find player
                var input = comps.OfType<InputComponent>().FirstOrDefault();
                var transforms = comps.OfType<TransformComponent>();
                if (input == null) return;
                var pTx = transforms.First(t => t.EntityId == input.EntityId);

                // calculate tile coords
                var tx = (int)(pTx.Position.X / _tileWidth);
                var ty = (int)(pTx.Position.Y / _tileHeight);
                var center = new Vector2(
                    tx * _tileWidth + _tileWidth / 2f,
                    ty * _tileHeight + _tileHeight / 2f
                );

                // see if a crop already occupies this tile
                var crops = comps.OfType<CropComponent>();
                var cropTxs = transforms.Where(t => crops.Any(c => c.EntityId == t.EntityId));
                var existing = cropTxs.FirstOrDefault(t =>
                    (int)(t.Position.X / _tileWidth) == tx &&
                    (int)(t.Position.Y / _tileHeight) == ty
                );

                if (existing == null)
                {
                    // **plant**
                    var crop = new Entity();

                    // transform
                    var cTx = new TransformComponent { EntityId = crop.Id, Position = center };
                    crop.AddComponent(cTx);
                    _ecs.AddComponent(cTx);

                    // sprite for stage 0
                    var cSpr = new SpriteComponent
                    {
                        EntityId = crop.Id,
                        Texture = _content.Load<Texture2D>("Tut"),
                        Origin = new Vector2(16, 16),
                        SourceRectangle = null
                    };
                    crop.AddComponent(cSpr);
                    _ecs.AddComponent(cSpr);

                    // crop logic
                    var durations = new float[] { 5f, 5f, 5f }; // 3 stages at 5s each
                    var stages = new List<Texture2D> {
                        _content.Load<Texture2D>("cropStage0"),
                        _content.Load<Texture2D>("cropStage1"),
                        _content.Load<Texture2D>("cropStage2"),
                        _content.Load<Texture2D>("cropStage3")
                    };
                    var cComp = new CropComponent
                    {
                        EntityId = crop.Id,
                        StageDurations = durations,
                        StageTextures = stages
                    };
                    crop.AddComponent(cComp);
                    _ecs.AddComponent(cComp);

                    // scale
                    var cScale = new ScaleComponent { EntityId = crop.Id, Scale = new Vector2(0.5f) };
                    crop.AddComponent(cScale);
                    _ecs.AddComponent(cScale);
                }
                else
                {
                    // **harvest = reset**
                    var cComp = comps.OfType<CropComponent>()
                                     .First(c => c.EntityId == existing.EntityId);
                    var cSpr = comps.OfType<SpriteComponent>()
                                     .First(s => s.EntityId == existing.EntityId);

                    cComp.CurrentStage = 0;
                    cComp.TimeInCurrentStage = 0f;
                    cSpr.Texture = cComp.StageTextures[0];
                }
            }

            _prevKb = kb;
        }
    }
}
