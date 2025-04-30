// Source/System/GrowthSystem.cs
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    /// <summary>
    /// Advances any planted crops through their stages over time.
    /// </summary>
    public class GrowthSystem : ISystem
    {
        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var crops = comps.OfType<CropComponent>();
            var sprites = comps.OfType<SpriteComponent>();

            foreach (var c in crops)
            {
                // accumulate time
                c.TimeInCurrentStage += delta;

                // if time to grow…
                if (c.CurrentStage < c.StageDurations.Length &&
                    c.TimeInCurrentStage >= c.StageDurations[c.CurrentStage])
                {
                    c.TimeInCurrentStage = 0f;
                    c.CurrentStage++;

                    // update sprite
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
