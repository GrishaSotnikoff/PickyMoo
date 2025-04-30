using System;

namespace PickyMoo.ESC
{
    public interface IComponent
    {
        Guid EntityId { get; set; }
    }
}
