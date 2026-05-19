using System;
using Dave6.ItemSystem.Domain.Container;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Application.Container
{
    public class ContainerService
    {
        /// <summary>
        /// 사운드, 로그, 이펙트 같은 트리거에 사용
        /// </summary>
        public event Action<ContainerAction> OnActionExecuted;
        public ContainerResult Move(ItemInstance item, IItemContainer target, ItemPlacement placement)
        {
            // 널 체크
            if (item == null) return ContainerResult.Fail(ContainerError.InvalidItem);
            if (target == null) return ContainerResult.Fail(ContainerError.InvalidTarget);

            var source = item.Owner;
            if (source == null) return ContainerResult.Fail(ContainerError.NoSource);
            if (IsSelfOrDescendant(item, target)) return ContainerResult.Fail(ContainerError.InvalidTarget);

            var originalPlacement = source.GetPlacement(item);

            // 기존 삭제
            var removed = source.TryRemove(item);
            if (!removed.Success) return ContainerResult.Fail(ContainerError.RemoveFailed);

            var canAdd = target.CanAdd(item, placement);
            if (!canAdd.Success)
            {
                // 롤백
                source.TryAdd(item, originalPlacement);
                return ContainerResult.Fail(ContainerError.CannotAdd);
            }

            var added = target.TryAdd(item, placement);
            if (!added.Success)
            {
                source.TryAdd(item, originalPlacement);
                return ContainerResult.Fail(ContainerError.AddFailed);
            }

            // 액션 생성
            var action = new ContainerAction
            {
                Type = ContainerAction.ActionType.Move,
                Item = item,
                From = source,
                To = target,
                Placement = placement
            };

            // 이벤트 발생
            OnActionExecuted?.Invoke(action);

            return ContainerResult.Ok(action);
        }
        public ContainerResult Add(ItemInstance item, ContainerCollection collection)
        {
            if (item == null) return ContainerResult.Fail(ContainerError.InvalidItem);
            if (collection == null) return ContainerResult.Fail(ContainerError.InvalidTarget);

            // 모든 후보 순회
            foreach (var container in collection.AllContainers)
            {
                var canAdd = container.CanAdd(item);
                if (!canAdd.Success) continue;

                // 성공시 기존로직 재사용
                return Add(item, container);
            }
            return ContainerResult.Fail(ContainerError.AddFailed);
        }

        public ContainerResult Add(ItemInstance item, IItemContainer target, ItemPlacement placement = null)
        {
            if (item == null) return ContainerResult.Fail(ContainerError.InvalidItem);
            if (target == null) return ContainerResult.Fail(ContainerError.InvalidTarget);

            if (IsSelfOrDescendant(item, target)) return ContainerResult.Fail(ContainerError.InvalidTarget);

            ContainerResult canAdd;
            if (placement == null) canAdd = target.CanAdd(item);
            else canAdd = target.CanAdd(item, placement);
            if (!canAdd.Success) return ContainerResult.Fail(ContainerError.CannotAdd);

            ContainerResult added;
            if (placement == null) added = target.TryAdd(item);
            else added = target.TryAdd(item, placement);
            if (!added.Success) return ContainerResult.Fail(ContainerError.AddFailed);

            var action = new ContainerAction
            {
                Type = ContainerAction.ActionType.Add,
                Item = item,
                From = null,
                To = target,
                Placement = placement
            };

            OnActionExecuted?.Invoke(action);

            return ContainerResult.Ok(action);
        }

        public ContainerResult Remove(ItemInstance item)
        {
            if (item == null) return ContainerResult.Fail(ContainerError.InvalidItem);

            var source = item.Owner;
            if (source == null) return ContainerResult.Fail(ContainerError.NoSource);

            var placement = source.GetPlacement(item);

            var removed = source.TryRemove(item);
            if (!removed.Success) return ContainerResult.Fail(ContainerError.RemoveFailed);

            var action = new ContainerAction
            {
                Type = ContainerAction.ActionType.Remove,
                Item = item,
                From = source,
                To = null,
                Placement = placement
            };

            OnActionExecuted?.Invoke(action);

            return ContainerResult.Ok(action);
        }

        bool IsSelfOrDescendant(ItemInstance item, IItemContainer target)
        {
            var current = target;
            while (current != null)
            {
                var ownerItem = current.Owner;

                if (ownerItem == null) break;

                if (ownerItem == item)
                    return true;

                current = ownerItem.Owner;
            }

            return false;
        }
    }

}