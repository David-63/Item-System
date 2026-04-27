#nullable enable

using System.Linq;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Domain.Container
{
    public class SocketSlot
    {
        public SlotCategory SlotCategory { get; }
        public int SlotId { get; }
        public ItemInstance? Item { get; private set; }

        public SocketSlot(SlotCategory category, int id)
        {
            SlotCategory = category;
            SlotId = id;
        }

        public bool IsEmpty() => Item == null;
        public void SetItem(ItemInstance item) => Item = item;
        public bool CanEquip(ItemInstance item)
        {
            foreach (var descriptor in item.Definition.EquipDescriptors)
            {
                if (descriptor.EquipableSlot == SlotCategory) return true;
            }
            return false;
        }
        public void Clear() => Item = null;
    }
}
