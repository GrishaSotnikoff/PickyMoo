using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    /// <summary>
    /// Crops only grow if they’ve been watered. Water dries over time.
    /// </summary>
    public class GrowthSystem : ISystem
    {
        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var crops = comps.OfType<CropComponent>();
            var sprites = comps.OfType<SpriteComponent>();
            var waters = comps.OfType<WaterComponent>();

            // dry out water
            foreach (var w in waters)
                w.TimeRemaining -= delta;

            foreach (var c in crops)
            {
                // skip if never watered or already dry
                var w = waters.FirstOrDefault(x => x.EntityId == c.EntityId);
                if (w == null || w.TimeRemaining <= 0f)
                    continue;

                // accumulate and grow
                c.TimeInCurrentStage += delta;
                if (c.CurrentStage < c.StageDurations.Length &&
                    c.TimeInCurrentStage >= c.StageDurations[c.CurrentStage])
                {
                    c.TimeInCurrentStage = 0f;
                    c.CurrentStage++;
                    if (c.CurrentStage < c.StageTextures.Count)
                    {
                        var spr = sprites.First(s => s.EntityId == c.EntityId);
                        spr.Texture = c.StageTextures[c.CurrentStage];
                    }
                }
            }
        }
    }
}
