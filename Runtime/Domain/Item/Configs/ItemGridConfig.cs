#nullable enable

using Dave6.Foundation.Math;

namespace Dave6.ItemSystem.Domain.Item
{
    public class ItemGridConfig : ItemContainerConfig
    {
        public Int2 GridSize;
        public ItemGridConfig(Int2 gridSize)
        {
            GridSize = gridSize;
        }
    }
}