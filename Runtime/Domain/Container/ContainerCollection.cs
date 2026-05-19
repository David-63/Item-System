using System;
using System.Collections.Generic;
using System.Linq;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Domain.Container
{
    public class ContainerCollection
    {
        ContainerLayout _Layout;
        IItemContainer _BaseContainer;
        List<IItemContainer> _OrderedExtensions = new();
        Dictionary<IItemContainer, ItemInstance> _ContainerSource = new();
        Dictionary<ItemInstance, List<IItemContainer>> _ExtensionsBySource = new();
        public ExtensionRole Role {get;private set;}

        public IEnumerable<IItemContainer> AllContainers
        {
            get
            { 
                yield return _BaseContainer;
                foreach (var ext in _OrderedExtensions)
                {
                    yield return ext;
                }
            }
        }
        public event Action<IItemContainer, ContainerCollection> OnContainerAdded;
        public event Action<IItemContainer, ContainerCollection> OnContainerRemoved;

        public ContainerCollection(IItemContainer container, ExtensionRole role)
        {
            _BaseContainer = container;
            _Layout = container.Layout;
            Role = role;
            if (container is ItemContainerBase baseContainer)
            {
                baseContainer.SetCollection(this);
            }
        }

        public IEnumerable<ItemInstance> AttachExtension(ItemInstance ext)
        {
            var externals = ext.GetExternalContainers().Where(c => c.Layout == _Layout).ToList();
            if (externals.Count == 0) yield break;

            _ExtensionsBySource[ext] = externals;
            foreach (var container in externals)
            {
                foreach (var item in container.Items)
                {
                    yield return item;
                }
                _OrderedExtensions.Add(container);
                _ContainerSource[container] = ext;
                if (container is ItemContainerBase baseContainer)
                {
                    baseContainer.SetCollection(this);
                }
                OnContainerAdded?.Invoke(container, this);
            }
        }
        public IEnumerable<ItemInstance> DetachExtension(ItemInstance ext)
        {
            if (!_ExtensionsBySource.TryGetValue(ext, out var containers)) yield break;

            foreach (var container in containers)
            {
                foreach (var item in container.Items)
                {
                    yield return item;
                }
                _OrderedExtensions.Remove(container);
                _ContainerSource.Remove(container);
                if (container is ItemContainerBase baseContainer)
                {
                    baseContainer.SetCollection(null);
                }
                OnContainerRemoved?.Invoke(container, this);
            }

            _ExtensionsBySource.Remove(ext);
        }

        public ItemInstance GetSource(IItemContainer container)
        {
            return _ContainerSource.TryGetValue(container, out var source) ? source : null;
        }
    }
}

