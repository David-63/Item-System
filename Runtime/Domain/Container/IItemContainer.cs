#nullable enable

using System.Collections.Generic;
using Dave6.Foundation.Math;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Domain.Container
{
    public interface IItemContainer
    {
        string? ContainerName { get; }
        ItemInstance? Owner { get; }                 // 트리 구조 가능
        IReadOnlyCollection<ItemInstance> Items { get; }
        bool IsDirty { get; }
        void ClearDirty();

        void SetOwner(ItemInstance? parent);
        ItemPlacement? GetPlacement(ItemInstance item);

        bool CanAdd(ItemInstance item);
        bool TryAdd(ItemInstance item);
        bool CanAdd(ItemInstance item, ItemPlacement context);
        bool TryAdd(ItemInstance item, ItemPlacement context);
        bool TryRemove(ItemInstance item);

        bool IsEmpty() => Items.Count == 0;
    }
}
