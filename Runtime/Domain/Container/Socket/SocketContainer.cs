#nullable enable

using System.Collections.Generic;
using Dave6.ItemSystem.Application.Container;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Domain.Container
{
    public class SocketContainer : ItemContainerBase
    {
        protected override ContainerLayout _Layout => ContainerLayout.Socket;

        readonly List<SocketSlot> _SocketSlots = new();                                    // 슬롯 상태
        public IReadOnlyList<SocketSlot> SocketSlots => _SocketSlots;
        public SocketLabelLayout SocketLabelLayout { get; }
        public SocketFlowLayout SocketFlowLayout { get; }

        public SocketContainer(string containerName, SocketLayoutConfig config)//IEnumerable<SlotCategory> slotCategories, SocketLabelLayout socketLayout = SocketLabelLayout.LabelAbove)
        {
            ContainerName = containerName;
            SocketLabelLayout = config.Label;
            SocketFlowLayout = config.Flow;

            int localId = 0;

            foreach (var type in config.AllowedSlots)
            {
                _SocketSlots.Add(new SocketSlot(type, localId++));
            }
        }

        #region Socket API
        public override ItemPlacement? GetPlacement(ItemInstance item)
        {
            var slot = FindSlot(item);
            if (slot == null) return null;

            return new SocketPlacement(slot.SlotId);
        }

        public override ContainerResult CanAdd(ItemInstance item)
        {
            if (item == null) return ContainerResult.Fail(ContainerError.InvalidItem);
            if (_Items.Contains(item)) return ContainerResult.Fail(ContainerError.ItemExists);

            foreach (var slot in _SocketSlots)
            {
                if (!slot.IsEmpty()) continue;
                if (!slot.CanEquip(item)) continue;
                return ContainerResult.Ok(null!);
            }
            return ContainerResult.Fail(ContainerError.NoSpaceAvailable);
        }

        public override ContainerResult CanAdd(ItemInstance item, ItemPlacement? placement)
        {
            if (item == null) return ContainerResult.Fail(ContainerError.InvalidItem);
            if (placement is not SocketPlacement sp)
            {
                return ContainerResult.Fail(ContainerError.InvalidPlacementType);
            }

            if (sp.SlotId < 0 || sp.SlotId >= _SocketSlots.Count)
            {
                return ContainerResult.Fail(ContainerError.NoSpaceAvailable);
            }

            var slot = _SocketSlots[sp.SlotId];
            if (!slot.IsEmpty())
            {
                return ContainerResult.Fail(ContainerError.NoSpaceAvailable);
            }

            if (!slot.CanEquip(item))
            {
                return ContainerResult.Fail(ContainerError.CannotAdd);
            }

            return ContainerResult.Ok(null!);
        }

        public override ContainerResult TryAdd(ItemInstance item)
        {
            if (item == null) return ContainerResult.Fail(ContainerError.InvalidItem);
            if (_Items.Contains(item)) return ContainerResult.Fail(ContainerError.ItemExists);

            foreach (var slot in _SocketSlots)
            {
                if (!slot.IsEmpty()) continue;
                if (!slot.CanEquip(item)) continue;

                return TryAdd(item, new SocketPlacement(slot.SlotId));
            }
            return ContainerResult.Fail(ContainerError.NoSpaceAvailable);
        }

        public override ContainerResult TryAdd(ItemInstance item, ItemPlacement? placement)
        {
            if (item == null) return ContainerResult.Fail(ContainerError.InvalidItem);
            if (placement is not SocketPlacement sp) return ContainerResult.Fail(ContainerError.InvalidPlacementType);
            if (_Items.Contains(item)) return ContainerResult.Fail(ContainerError.ItemExists);

            var canAdd = CanAdd(item, placement);
            if (!canAdd.Success) return canAdd;

            var slot = _SocketSlots[sp.SlotId];
            var result = base.TryAdd(item);
            if (!result.Success) return result;

            slot.SetItem(item);
            return result;
        }

        public override ContainerResult TryRemove(ItemInstance item)
        {
            if (item == null) return ContainerResult.Fail(ContainerError.InvalidItem);

            var slot = FindSlot(item);
            if (slot == null) return ContainerResult.Fail(ContainerError.InvalidItem);

            var result = base.TryRemove(item);
            if (!result.Success) return result;

            slot.Clear();
            return result;
        }
        #endregion

        #region Inner Logic
        SocketSlot? FindSlot(ItemInstance item)
        {
            foreach (var slot in _SocketSlots)
            {
                if (slot.Item == item) return slot;
            }
            return null;
        }
        #endregion
    }
}
