using System.Collections.Generic;
using System.Linq;
using Dave6.Foundation.Math;
using Dave6.ItemSystem.Application.Container;
using Dave6.ItemSystem.Domain.Container;
using Mono.Cecil.Cil;

namespace Dave6.ItemSystem.Application.Controller
{
    public class StashContext
    {
        Dictionary<RootContainerRole, IItemContainer> _RootContainer = new();

        public StashContext(Dictionary<RootContainerRole, IItemContainer> containers) => _RootContainer = containers;

        public bool TryGetRoot(RootContainerRole role, out IItemContainer container) => _RootContainer.TryGetValue(role, out container);
        public IEnumerable<IItemContainer> GetRootContainers() => _RootContainer.Values;
    }
}

