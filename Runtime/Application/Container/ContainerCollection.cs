using System;
using System.Collections.Generic;
using System.Linq;
using Dave6.ItemSystem.Domain.Container;
using Dave6.ItemSystem.Domain.Item;
using UnityEngine;

namespace Dave6.ItemSystem.Application.Container
{
    public class ContainerCollection
    {
        ContainerLayout _Layout;
        IItemContainer _BaseContainer;
        List<IItemContainer> _OrderedExtensions = new();
        Dictionary<ItemInstance, List<IItemContainer>> _ExtensionsBySource = new();
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

        public ContainerCollection(IItemContainer container)
        {
            _BaseContainer = container;
            _Layout = container.Layout;
        }

        public void AddExtension(ItemInstance ext)
        {
            var externals = ext.GetExternalContainers().Where(c => c.Layout == _Layout).ToList();
            if (externals.Count == 0) return;

            _ExtensionsBySource[ext] = externals;
            foreach (var container in externals)
            {
                _OrderedExtensions.Add(container);
                OnContainerAdded?.Invoke(container, this);
            }
        }
        public void RemoveExtension(ItemInstance ext)
        {
            if (!_ExtensionsBySource.TryGetValue(ext, out var containers)) return;

            foreach (var container in containers)
            {
                _OrderedExtensions.Remove(container);
                OnContainerRemoved?.Invoke(container, this);
            }

            _ExtensionsBySource.Remove(ext);
        }
    }
}

