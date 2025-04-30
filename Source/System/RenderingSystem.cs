using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    public class RenderingSystem : ISystem
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Camera2D _camera;

        public RenderingSystem(SpriteBatch spriteBatch, Camera2D camera)
        {
            _spriteBatch = spriteBatch;
            _camera = camera;
        }

        public void Update(List<IComponent> components, GameTime gameTime)
        {
            var sprites = components.OfType<SpriteComponent>();
            var transforms = components.OfType<TransformComponent>();
            var scales = components.OfType<ScaleComponent>();

            _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());

            foreach (var sprite in sprites)
            {
                var xf = transforms.FirstOrDefault(t => t.EntityId == sprite.EntityId);
                if (xf == null) continue;

                var sc = scales.FirstOrDefault(s => s.EntityId == xf.EntityId);
                var scale = sc?.Scale ?? Vector2.One;

                _spriteBatch.Draw(
                    sprite.Texture,
                    xf.Position,
                    sprite.SourceRectangle,
                    Color.White,
                    sprite.Rotation,
                    sprite.Origin,
                    scale,
                    sprite.Effects,
                    sprite.LayerDepth
                );
            }

            _spriteBatch.End();
        }
    }
}
