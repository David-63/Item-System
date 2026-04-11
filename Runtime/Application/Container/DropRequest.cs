using Dave6.Foundation.Math;
using Dave6.ItemSystem.Domain.Container;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Application.Container
{
    public class DropRequest
    {
        public ItemInstance Item;
        public IItemContainer Source;
        public ItemPlacement SourcePlacement;
        public IItemContainer Target;
        public ItemPlacement TargetPlacement;
    }
}
