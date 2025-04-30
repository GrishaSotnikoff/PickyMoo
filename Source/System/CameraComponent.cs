using System;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    /// <summary>
    /// Marks which entity the camera should follow.
    /// </summary>
    public class CameraComponent : IComponent
    {
        public Guid EntityId { get; set; }
    }
}
