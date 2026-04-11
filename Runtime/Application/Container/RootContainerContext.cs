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
        Dictionary<RootContainerRole, IItemContainer> _RootContainers = new();

        public event Action<ItemInstance, IItemContainer> OnItemAdded;
        public event Action<ItemInstance> OnItemRemoved;
        public event Action<ItemInstance, IItemContainer> OnItemMoved;

        public LoadoutRootContext(Dictionary<RootContainerRole, IItemContainer> containers) => _RootContainers = containers;


        #region Read API
        public bool TryGetRoot(RootContainerRole role, out IItemContainer container) => _RootContainers.TryGetValue(role, out container);
        public IEnumerable<KeyValuePair<RootContainerRole, IItemContainer>> GetRootContainers() => _RootContainers;
        public IEnumerable<(RootContainerRole, IItemContainer)> GetRootContainerPairs()
        {
            foreach (var kv in _RootContainers)
            {
                yield return (kv.Key, kv.Value);
            }
        }
        public IEnumerable<ItemInstance> GetItemsAll()
        {
            foreach (var container in _RootContainers.Values)
            {
                foreach (var item in Traverse(container))
                {
                    yield return item;
                }
            }
        }
        IEnumerable<ItemInstance> Traverse(IItemContainer container)
        {
            foreach (var item in container.Items)
            {
                yield return item;

                if (item.OwnedContainer == null) continue;
                foreach (var child in Traverse(item.OwnedContainer))
                {
                    yield return child;
                }
            }
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

