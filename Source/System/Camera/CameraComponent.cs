using System;
using PickyMoo.ESC;

namespace PickyMoo.Source.System.Camera
{
    /// <summary>
    /// Marks which entity the camera should follow.
    /// </summary>
    public class CameraComponent : IComponent
    {
        public Guid EntityId { get; set; }
    }
}
