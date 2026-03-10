#nullable enable

using Dave6.ItemSystem.Domain.Container;
using Dave6.ItemSystem.Domain.Item;
using UnityEngine;

namespace Dave6.ItemSystem.Application.Container
{
    public class ContainerService
    {
        public bool Move(ItemInstance item, IItemContainer target, ItemPlacement? targetContext = null)
        {
            // source 캐싱
            var source = item.Owner;
            if (source == null) return false;
            var originalPlacement = source.GetPlacement(item);

            // target이 받을 수 있는지 확인
            bool canAdd = targetContext == null ? target.CanAdd(item) : target.CanAdd(item, targetContext);
            if (!canAdd)
            {
                Debug.Log("Can not add");
                return false;
            }
            if (!source.TryRemove(item))
            {
                Debug.Log("Remove failed");
                return false;
            }

            bool added = targetContext == null ? target.TryAdd(item) : target.TryAdd(item, targetContext);
            // 롤백
            if (!added)
            {
                Debug.Log("Add failed");
                if (originalPlacement == null)
                {
                    source.TryAdd(item);
                }
                else
                {
                    source.TryAdd(item, originalPlacement);                    
                }
                return false;
            }
            return true;
        }
    }
}