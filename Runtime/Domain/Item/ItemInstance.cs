#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Dave6.ItemSystem.Domain.Container;

namespace Dave6.ItemSystem.Domain.Item
{
    public class ItemInstance
    {
        public ItemDefinition Definition { get; }
        public IItemContainer? Owner { get; internal set; }

        readonly List<IItemContainer> _Containers = new();
        public IReadOnlyList<IItemContainer> Containers => _Containers;

        public IEnumerable<IItemContainer> GetExternalContainers() => _Containers.Where(c => c.IsExternal == true);

        public ItemInstance(ItemDefinition definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));

            foreach(var descriptor in Definition.OwnershipDescriptors)
            {
                var container = CreateContainer(descriptor);
                _Containers.Add(container);
            }
            foreach(var descriptor in Definition.InfluenceDescriptors)
            {
                var container = CreateContainer(descriptor);
                container.SetExternal();
                _Containers.Add(container);
            }
        }
        IItemContainer CreateContainer(ContainerDescriptor descriptor)
        {
            string containerName = Definition.DisplayName + " Container";

            ItemContainerBase container = descriptor.Layout switch
            {
                ContainerLayout.Grid => new GridContainer(containerName, descriptor.GridSize),
                ContainerLayout.Socket => new SocketContainer(containerName, descriptor.SocketConfig),
                _ => throw new InvalidOperationException($"Unsupported layout: {descriptor.Layout}"),
            };
            container.SetOwner(this);

            return container;
        }
    }
}
