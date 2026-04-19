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

            int localId = 0;

            foreach (var type in slotCategories)
            {
                _SocketSlots.Add(new SocketSlot(type, localId++));
            }
        }

        public override ItemPlacement? GetPlacement(ItemInstance item)
        {
            var slot = FindSlot(item);
            if (slot == null) return null;

            return new SoketPlacement(slot.SlotId);
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
        public override bool CanAdd(ItemInstance item, ItemPlacement placement)
        {
            if (placement is not SoketPlacement sp)
            {
                //Debug.Log("타입 불일치");
                return false;
            }
            if (sp.SlotId < 0 || sp.SlotId >= _SocketSlots.Count)
            {
                //Debug.Log("슬롯 인덱스 범위 벗어남");
                return false;
            }

            var slot = _SocketSlots[sp.SlotId];
            if (!slot.IsEmpty())
            {
                //Debug.Log("슬롯이 이미 차있음");
                return false;
            }
            if (!slot.CanEquip(item))
            {
                //Debug.Log("장착 불가능한 아이템");
                return false;
            }

            return true;
        }
        public override bool TryAdd(ItemInstance item)
        {
            foreach (var slot in _SocketSlots)
            {
                if (!slot.IsEmpty()) continue;
                if (!slot.CanEquip(item)) continue;

                return TryAdd(item, new SoketPlacement(slot.SlotId));
            }
            return false;
        }
        public override bool TryAdd(ItemInstance item, ItemPlacement placement)
        {
            if (placement is not SoketPlacement sp) return false;
            if (!CanAdd(item, placement)) return false;

            var slot = _SocketSlots[sp.SlotId];
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
