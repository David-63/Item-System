#nullable enable

using System.Collections.Generic;
using Dave6.ItemSystem.Application.Container;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Domain.Container
{
    public interface IItemContainer
    {
        string? ContainerName { get; }
        ItemInstance? Owner { get; }                 // 트리 구조 가능
        ContainerLayout Layout { get; }
        IReadOnlyCollection<ItemInstance> Items { get; }
        bool IsExternal { get; }
        void SetExternal(bool isExternal = true);

        bool IsDirty { get; }
        void ClearDirty();

        void SetOwner(ItemInstance? parent);
        ItemPlacement? GetPlacement(ItemInstance item);

        ContainerResult CanAdd(ItemInstance item);
        ContainerResult TryAdd(ItemInstance item);
        ContainerResult CanAdd(ItemInstance item, ItemPlacement context);
        ContainerResult TryAdd(ItemInstance item, ItemPlacement context);
        ContainerResult TryRemove(ItemInstance item);

        bool IsEmpty() => Items.Count == 0;
    }
}
