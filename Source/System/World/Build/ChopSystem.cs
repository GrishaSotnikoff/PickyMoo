// Source/System/ChopSystem.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PickyMoo.ESC;
using PickyMoo.Source.System.Player;

namespace PickyMoo.Source.System
{
    /// <summary>
    /// Handles chopping nearby trees when the Axe is selected and E is pressed.
    /// Removes the tree entity and grants Wood to inventory.
    /// </summary>
    public class ChopSystem : ISystem
    {
        private readonly ECSManager _ecs;
        private readonly float _chopRange;
        private KeyboardState _prevKb;

        /// <param name="ecs">Reference so we can remove entities.</param>
        /// <param name="chopRange">Max distance (in world units) to chop.</param>
        public ChopSystem(ECSManager ecs, float chopRange = 32f)
        {
            _ecs = ecs;
            _chopRange = chopRange;
        }

        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            // on E down edge
            if (kb.IsKeyDown(Keys.E) && !_prevKb.IsKeyDown(Keys.E))
            {
                // must have Axe selected
                var belt = comps.OfType<ToolbeltComponent>().FirstOrDefault();
                if (belt?.CurrentTool != ToolType.Axe) return;

                // find player position
                var input = comps.OfType<InputComponent>().FirstOrDefault();
                var transforms = comps.OfType<TransformComponent>().ToList();
                if (input == null) return;
                var pTx = transforms.First(t => t.EntityId == input.EntityId);

                // snapshot trees so we can safely remove
                var trees = comps.OfType<TreeComponent>().ToList();
                foreach (var tree in trees)
                {
                    var tTx = transforms.First(t => t.EntityId == tree.EntityId);
                    if (Vector2.Distance(pTx.Position, tTx.Position) <= _chopRange)
                    {
                        // remove all components of that tree
                        _ecs.RemoveEntity(tree.EntityId);

                        // give wood
                        var inv = comps.OfType<InventoryComponent>().FirstOrDefault();
                        if (inv != null)
                        {
                            inv.Items.TryGetValue("Wood", out int have);
                            inv.Items["Wood"] = have + 1;
                            Console.WriteLine("🪓 You chopped a tree and got 1 Wood!");
                        }

                        break; // chop only one per press
                    }
                }
            }
            _prevKb = kb;
        }
    }
}
