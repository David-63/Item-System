#nullable enable

using System.Collections.Generic;
using Dave6.ItemSystem.Application.Container;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Domain.Container
{
    public abstract class ItemContainerBase : IItemContainer
    {
        public string? ContainerName { get; protected set; }
        protected readonly List<ItemInstance> _Items = new();
        public IReadOnlyCollection<ItemInstance> Items => _Items;


        public ItemInstance? Owner { get; protected set; }
        public void SetOwner(ItemInstance? owner) => Owner = owner;

        public abstract ItemPlacement? GetPlacement(ItemInstance item);
        public abstract ContainerResult CanAdd(ItemInstance item);
        public virtual ContainerResult TryAdd(ItemInstance item)
        {
            if (_Items.Contains(item)) return ContainerResult.Fail(ContainerError.ItemExists);
            _Items.Add(item);
            item.Owner = this;
            _IsDirty = true;
            return ContainerResult.Ok(null!);
        }

        public abstract ContainerResult CanAdd(ItemInstance item, ItemPlacement context);
        public virtual ContainerResult TryAdd(ItemInstance item, ItemPlacement context)
        {
            return TryAdd(item);
        }
        public virtual ContainerResult TryRemove(ItemInstance item)
        {
            if (!_Items.Remove(item)) return ContainerResult.Fail(ContainerError.InvalidItem);
            item.Owner = null;
            _IsDirty = true;
            return ContainerResult.Ok(null!);
        }

        protected bool _IsDirty = false;
        public bool IsDirty => _IsDirty;
        public void ClearDirty() => _IsDirty = false;

        bool _IsExternal = false;

        public bool IsExternal => _IsExternal;

        protected virtual ContainerLayout _Layout { get; set; }

        public ContainerLayout Layout => _Layout;

        public void SetExternal(bool isExternal = true) => _IsExternal = isExternal;
    }
}
