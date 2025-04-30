using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using PickyMoo.ESC;

namespace PickyMoo.Source.System.Camera
{
    /// <summary>
    /// Moves the Camera2D.Position to match the entity with CameraComponent.
    /// </summary>
    public class CameraFollowSystem : ISystem
    {
        private readonly Camera2D _camera;

        public CameraFollowSystem(Camera2D camera)
        {
            _camera = camera;
        }

        public void Update(List<IComponent> components, GameTime gameTime)
        {
            // find the tagged entity…
            var camComp = components.OfType<CameraComponent>().FirstOrDefault();
            var transforms = components.OfType<TransformComponent>();

            if (camComp != null)
            {
                var target = transforms.FirstOrDefault(t => t.EntityId == camComp.EntityId);
                if (target != null)
                {
                    // snap camera center to player position
                    _camera.Position = target.Position;
                }
            }
        }
    }
}
