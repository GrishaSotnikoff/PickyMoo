using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PickyMoo.Source.System
{
    /// <summary>
    /// Simple 2D camera: tracks position, zoom, rotation.
    /// </summary>
    public class Camera2D
    {
        public Vector2 Position { get; set; } = Vector2.Zero;
        public float Zoom { get; set; } = 1f;
        public float Rotation { get; set; } = 0f;

        private readonly Viewport _viewport;

        public Camera2D(Viewport viewport)
        {
            _viewport = viewport;
        }

        public Matrix GetViewMatrix()
        {
            // Move world opposite camera, apply rotation & zoom, then center on screen
            return
                Matrix.CreateTranslation(new Vector3(-Position, 0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom) *
                Matrix.CreateTranslation(new Vector3(
                    _viewport.Width * 0.5f,
                    _viewport.Height * 0.5f,
                    0f
                ));
        }
    }
}
