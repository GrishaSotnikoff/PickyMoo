using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PickyMoo.ESC
{
    public interface ISystem
    {
        void Update(List<IComponent> components, GameTime gameTime);
    }
}
