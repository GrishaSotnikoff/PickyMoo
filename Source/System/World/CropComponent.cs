// Source/System/CropComponent.cs
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using PickyMoo.ESC;

namespace PickyMoo.Source.System.World
{
    /// <summary>
    /// Tracks a crop’s current stage, textures for each stage, and timings.
    /// </summary>
    public class CropComponent : IComponent
    {
        public Guid EntityId { get; set; }
        public List<Texture2D> StageTextures { get; set; }    // one texture per stage
        public int CurrentStage { get; set; } = 0;
        public float[] StageDurations { get; set; }    // seconds per stage
        public float TimeInCurrentStage { get; set; } = 0f;
    }
}
