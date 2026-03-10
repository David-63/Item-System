#nullable enable

using Dave6.Foundation.Math;

namespace Dave6.ItemSystem.Domain.Container
{
    public class GridPlacement : ItemPlacement
    {
        public Int2 Position { get; }
        public bool Rotated { get; }
        public GridPlacement(Int2 position, bool rotated)
        {
            Position = position;
            Rotated = rotated;
        }
    }
}
