
using System;

namespace Dave6.ItemSystem.Domain.Item
{
    public class ItemInstance
    {
        public ItemDefinition definition { get; }
        public int stack { get; private set; }

        public ItemInstance(ItemDefinition definition, int stack = 1)
        {
            this.definition = definition ?? throw new ArgumentNullException(nameof(definition));
            this.stack = definition.ClampStack(stack);
        }

        public int AddStack(int amount)
        {
            if (!definition.CanStack()) return amount;
            
            int spaceLeft = definition.GetSpaceLeft(stack);
            int toAdd = Math.Min(spaceLeft, amount);
            stack += toAdd;

            return amount - toAdd;
        }

        public void SetStack(int value)
        {
            stack = definition.ClampStack(value);
        }
    }
}