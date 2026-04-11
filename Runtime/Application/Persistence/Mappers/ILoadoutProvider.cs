

using Dave6.ItemSystem.Application.Container;
using Dave6.ItemSystem.Domain.Container;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Application.Mapper
{
    public interface ILoadoutProvider
    {
        ContainerService GetService();
        LoadoutRootContext GetContext();

        ContainerResult Move(ItemInstance item, IItemContainer target, ItemPlacement placement);

        ContainerResult Add(ItemInstance item, RootContainerRole role);
        ContainerResult Add(ItemInstance item, IItemContainer target, ItemPlacement placement = null);

        ContainerResult Remove(ItemInstance item);
    }
}
