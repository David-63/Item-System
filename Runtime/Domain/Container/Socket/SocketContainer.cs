#nullable enable

using System.Collections.Generic;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Domain.Container
{
    public class SocketContainer : ItemContainerBase
    {
        readonly List<SocketSlot> _SocketSlots = new();                                    // 슬롯 상태
        public IReadOnlyList<SocketSlot> SocketSlots => _SocketSlots;
        public SocketLayout SocketLayout { get; }

        public SocketContainer(string containerName, IEnumerable<SlotCategory> slotCategories, SocketLayout socketLayout = SocketLayout.LabelAbove)
        {
            ContainerName = containerName;
            SocketLayout = socketLayout;

            var typeCount = new Dictionary<SlotCategory, int>();

            foreach (var type in slotCategories)
            {
                if (!typeCount.ContainsKey(type)) typeCount[type] = 0;
                int index = typeCount[type]++;
                _SocketSlots.Add(new SocketSlot(type, index));
            }
        }
        public override ItemPlacement? GetPlacement(ItemInstance item)
        {
            var slot = FindSlot(item);
            if (slot == null) return null;

            return new SlotPlacement(slot.SlotIndex);
        }
        public override bool CanAdd(ItemInstance item)
        {
            foreach (var slot in _SocketSlots)
            {
                if (!slot.IsEmpty()) continue;
                if (!slot.CanEquip(item)) continue;
                return true;
            }
            return false;
        }
        public override bool CanAdd(ItemInstance item, ItemPlacement context)
        {
            if (context is not SlotPlacement sp) return false;
            if (sp.SlotIndex < 0 || sp.SlotIndex >= _SocketSlots.Count) return false;

            var slot = _SocketSlots[sp.SlotIndex];
            if (!slot.IsEmpty()) return false;
            if (!slot.CanEquip(item)) return false;

            return true;
        }
        public override bool TryAdd(ItemInstance item)
        {
            foreach (var slot in _SocketSlots)
            {
                if (!slot.IsEmpty()) continue;
                if (!slot.CanEquip(item)) continue;

                return TryAdd(item, new SlotPlacement(slot.SlotIndex));
            }
            return false;
        }
        public override bool TryAdd(ItemInstance item, ItemPlacement context)
        {
            if (context is not SlotPlacement sp) return false;
            if (!CanAdd(item, context)) return false;

            var slot = _SocketSlots[sp.SlotIndex];
            if (!base.TryAdd(item)) return false;
            slot.SetItem(item);

            return true;
        }
        public override bool TryRemove(ItemInstance item)
        {
            var slot = FindSlot(item);
            if (slot == null) return false;

            if (!base.TryRemove(item)) return false;
            slot.Clear();

            return true;
        }

        SocketSlot? FindSlot(ItemInstance item)
        {
            foreach (var slot in _SocketSlots)
            {
                if (slot.Item == item) return slot;
            }
            return null;
        }
    }
}
