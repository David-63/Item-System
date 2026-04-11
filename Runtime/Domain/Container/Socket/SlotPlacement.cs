#nullable enable

namespace Dave6.ItemSystem.Domain.Container
{
    public class SoketPlacement : ItemPlacement
    {
        public int SlotId { get; }
        public SoketPlacement(int slotId) => SlotId = slotId;
    }
}
