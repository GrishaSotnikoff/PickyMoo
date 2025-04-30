using System;
using System.Collections.Generic;

namespace PickyMoo.ESC
{
    public class Entity
    {
        public Guid Id { get; private set; }
        private List<IComponent> _components = new List<IComponent>();

        public Entity() => Id = Guid.NewGuid();

        public void AddComponent(IComponent component)
        {
            component.EntityId = Id;
            _components.Add(component);
        }

        public IEnumerable<IComponent> GetComponents() => _components;
    }
}
