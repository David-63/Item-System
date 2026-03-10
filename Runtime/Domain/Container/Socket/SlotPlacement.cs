#nullable enable

namespace Dave6.ItemSystem.Domain.Container
{
    public class SlotPlacement : ItemPlacement
    {
        public int SlotIndex { get; }
        public SlotPlacement(int slotIndex) => SlotIndex = slotIndex;
    }
}
