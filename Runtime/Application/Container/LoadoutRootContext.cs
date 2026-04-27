using System;
using System.Collections.Generic;
using Dave6.ItemSystem.Domain.Container;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Application.Container
{
    /// <summary>
    /// Entry point
    /// </summary>
    public class LoadoutRootContext
    {
        Dictionary<ExtensionRole, ContainerCollection> _RootCollections = new();
        Dictionary<IItemContainer, ContainerCollection> _ContainerToCollection = new();
        #region UI 전용 이벤트
        public event Action<ItemInstance, IItemContainer> OnItemAdded;
        public event Action<ItemInstance> OnItemRemoved;
        public event Action<ItemInstance, IItemContainer> OnItemMoved;
        #endregion

        public LoadoutRootContext(Dictionary<ExtensionRole, ContainerCollection> collections)
        {
            _RootCollections = collections;
            foreach (var collection in _RootCollections.Values)
            {
                RegisterCollection(collection);
            }
        }

        void RegisterCollection(ContainerCollection collection)
        {
            foreach (var container in collection.AllContainers)
            {
                _ContainerToCollection[container] = collection;
            }

            collection.OnContainerAdded += AddToCollection;
            collection.OnContainerRemoved += RemoveFromCollection;
        }

        void AddToCollection(IItemContainer container, ContainerCollection collection)
        {
            _ContainerToCollection[container] = collection;
        }
        void RemoveFromCollection(IItemContainer container, ContainerCollection collection)
        {
            _ContainerToCollection.Remove(container);
        }

        #region Read API
        public bool TryGetCollection(ExtensionRole role, out ContainerCollection collection) => _RootCollections.TryGetValue(role, out collection);
        public bool TryGetCollection(IItemContainer container, out ContainerCollection collection) => _ContainerToCollection.TryGetValue(container, out collection);
        public ContainerCollection GetCollection(IItemContainer container) => _ContainerToCollection[container];
        public IEnumerable<KeyValuePair<ExtensionRole, ContainerCollection>> GetCollections() => _RootCollections;
        public IEnumerable<(ExtensionRole, ContainerCollection)> GetRootContainerPairs()
        {
            foreach (var kv in _RootCollections)
            {
                yield return (kv.Key, kv.Value);
            }
        }
        public ExtensionRole GetRole(ContainerCollection collection)
        {
            foreach (var kv in _RootCollections)
            {
                if (kv.Value == collection) return kv.Key;
            }
            throw new Exception("Collection not found");
        }
        public IEnumerable<ItemInstance> GetItemsAll()
        {
            var visited = new HashSet<ItemInstance>();
            foreach (var collection in _RootCollections.Values)
            {
                foreach (var container in collection.AllContainers)
                {
                    foreach (var item in Traverse(container, visited))
                    {
                        yield return item;
                    }
                }
            }
        }
        IEnumerable<ItemInstance> Traverse(IItemContainer container, HashSet<ItemInstance> visited)
        {
            foreach (var item in container.Items)
            {
                if (!visited.Add(item)) continue;
                yield return item;

                if (item.Containers.Count == 0) continue;

                foreach (var target in item.Containers)
                {
                    foreach (var child in Traverse(target, visited))
                    {
                        yield return child;
                    }
                }
            }
        }
        #endregion

        #region 상호작용 API
        public void AddExtension(ItemInstance ext)
        {
            
        }
        public void RemoveExtension(ItemInstance ext)
        {
            
        }
        #endregion

        #region 외부에서 직접 호출 못하게 internal or public but convention으로 막기
        /// <summary>
        /// UI 생성 이벤트
        /// </summary>
        public void NotifyItemAdded(ItemInstance item, IItemContainer container) => OnItemAdded?.Invoke(item, container);
        /// <summary>
        /// UI 제거 이벤트
        /// </summary>
        public void NotifyItemRemoved(ItemInstance item) => OnItemRemoved?.Invoke(item);
        /// <summary>
        /// UI 갱신 이벤트
        /// </summary>
        public void NotifyItemMoved(ItemInstance item, IItemContainer container) => OnItemMoved?.Invoke(item, container);
        #endregion
    }
}

