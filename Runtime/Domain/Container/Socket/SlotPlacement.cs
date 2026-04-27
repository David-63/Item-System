#nullable enable

namespace Dave6.ItemSystem.Domain.Container
{
    public class SocketPlacement : ItemPlacement
    {
        public int SlotId { get; }
        public SocketPlacement(int slotId) => SlotId = slotId;
    }
}
