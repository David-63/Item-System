#nullable enable

using System;
using Dave6.ItemSystem.Domain.Container;

namespace Dave6.ItemSystem.Domain.Item
{
    public class ItemInstance
    {
        public ItemDefinition Definition { get; }
        public IItemContainer? Owner { get; internal set; }
        public IItemContainer? OwnedContainer { get; }

        public ItemInstance(ItemDefinition definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));

            if (definition.ContainerConfig != null)
            {
                OwnedContainer = CreateOwnedContainer(definition);
                OwnedContainer.SetOwner(this);
            }
        }
        
        IItemContainer CreateOwnedContainer(ItemDefinition def)
        {
            return def.ContainerConfig switch
            {
                ItemGridConfig grid => new GridContainer(def.DisplayName + " Container", grid.GridSize),
                ItemSocketConfig slot => new SocketContainer(def.DisplayName + " Container", slot.AllowedSlots, slot.SocketLayout),
                null => throw new InvalidOperationException(),
                _ => throw new InvalidOperationException()
            };
        }
    }
}