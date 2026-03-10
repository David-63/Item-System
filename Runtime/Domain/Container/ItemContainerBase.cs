#nullable enable

using System.Collections.Generic;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Domain.Container
{
    public abstract class ItemContainerBase : IItemContainer
    {
        public string? ContainerName { get; protected set; }
        protected readonly List<ItemInstance> _Items = new();
        public ItemInstance? Owner { get; protected set; }
        public IReadOnlyCollection<ItemInstance> Items => _Items;

        public void SetOwner(ItemInstance? owner) => Owner = owner;

        public abstract ItemPlacement? GetPlacement(ItemInstance item);
        public abstract bool CanAdd(ItemInstance item);
        public virtual bool TryAdd(ItemInstance item)
        {
            if (_Items.Contains(item)) return false;
            _Items.Add(item);
            item.Owner = this;
            return true;
        }

        public abstract bool CanAdd(ItemInstance item, ItemPlacement context);
        public virtual bool TryAdd(ItemInstance item, ItemPlacement context)
        {
            return TryAdd(item);
        }
        public virtual bool TryRemove(ItemInstance item)
        {
            if (!_Items.Remove(item)) return false;
            item.Owner = null;
            return true;
        }
    }
}
