#nullable enable

using System.Linq;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Domain.Container
{
    public class SocketSlot
    {
        public SlotCategory SlotCategory { get; }
        public int SlotIndex { get; }
        public ItemInstance? Item { get; private set; }

        public SocketSlot(SlotCategory category, int index)
        {
            SlotCategory = category;
            SlotIndex = index;
        }

        public bool IsEmpty() => Item == null;
        public void SetItem(ItemInstance item) => Item = item;
        public bool CanEquip(ItemInstance item) => item.Definition.EquipableSlots.Contains(SlotCategory);
        public void Clear() => Item = null;
    }
}
