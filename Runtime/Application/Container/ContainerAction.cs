using Dave6.ItemSystem.Domain.Container;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Application.Container
{
    public class ContainerAction
    {
        public enum ActionType
        {
            Add,
            Remove,
            Move
        }

        public ActionType Type;
        public ItemInstance Item;
        public IItemContainer From;
        public IItemContainer To;
        public ItemPlacement Placement;
    }

}