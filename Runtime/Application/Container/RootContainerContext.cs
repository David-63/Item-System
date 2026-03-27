using System.Collections.Generic;
using Dave6.ItemSystem.Domain.Container;

namespace Dave6.ItemSystem.Application.Container
{
    public class RootContainerContext
    {
        Dictionary<RootContainerRole, IItemContainer> _RootContainer = new();

        public RootContainerContext(Dictionary<RootContainerRole, IItemContainer> containers) => _RootContainer = containers;

        public bool TryGetRoot(RootContainerRole role, out IItemContainer container) => _RootContainer.TryGetValue(role, out container);
        public IEnumerable<IItemContainer> GetRootContainers() => _RootContainer.Values;

        public IEnumerable<(RootContainerRole, IItemContainer)> GetAll()
        {
            foreach (var kv in _RootContainer)
            {
                yield return (kv.Key, kv.Value);
            }
        }
    }
}

