
using System;
using Dave6.Foundation.Math;

namespace Dave6.ItemSystem.Domain.Item
{

    public class ItemDefinition
    {
        public string displayName { get; }
        bool isStackable { get; }
        public int maxStack { get; }
        public Int2 size { get; }

        public ItemDefinition(string displayName, bool isStackable, int maxStack, Int2 size)
        {
            this.displayName = displayName;
            this.isStackable = isStackable;
            this.maxStack = isStackable ? Math.Max(1, maxStack) : 1;
            this.size = size;
        }

        public bool CanStackWith(ItemDefinition other)
        {
            return isStackable && displayName == other.displayName;
        }

        public bool CanStack() => isStackable;
        public int ClampStack(int value)
        {
            return isStackable ? Math.Clamp(value, 1, maxStack) : 1;
        }
        public int GetSpaceLeft(int currentStack)
        {
            return isStackable ? maxStack - currentStack : 0;
        }
    }
}