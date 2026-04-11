using System;
using Dave6.ItemSystem.Domain.Container;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Application.Container
{
    // 안씀
    public class ContainerService
    {
        public event Action<ContainerAction> OnActionExecuted;
        public ContainerResult Move(LoadoutRootContext ctx, ItemInstance item, IItemContainer target, ItemPlacement placement)
        {
            if (item == null) return ContainerResult.Fail(ContainerError.InvalidItem);
            if (target == null) return ContainerResult.Fail(ContainerError.InvalidTarget);

            var source = item.Owner;
            if (source == null) return ContainerResult.Fail(ContainerError.NoSource);

            var originalPlacement = source.GetPlacement(item);

            if (!source.TryRemove(item)) return ContainerResult.Fail(ContainerError.RemoveFailed);
            if (!target.CanAdd(item, placement))
            {
                // 롤백
                source.TryAdd(item, originalPlacement);
                return ContainerResult.Fail(ContainerError.CannotAdd);
            }


            if (!target.TryAdd(item, placement))
            {
                // 롤백
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
            ctx.NotifyItemMoved(item, target);

            return ContainerResult.Ok();
        }

        public ContainerResult Add(LoadoutRootContext ctx, ItemInstance item, IItemContainer target, ItemPlacement placement = null)
        {
            if (item == null) return ContainerResult.Fail(ContainerError.InvalidItem);
            if (target == null) return ContainerResult.Fail(ContainerError.InvalidTarget);

            bool canAdd = placement == null ? target.CanAdd(item) : target.CanAdd(item, placement);

            if (!canAdd) return ContainerResult.Fail(ContainerError.CannotAdd);

            bool added = placement == null ? target.TryAdd(item) : target.TryAdd(item, placement);

            if (!added) return ContainerResult.Fail(ContainerError.AddFailed);

            var action = new ContainerAction
            {
                Type = ContainerAction.ActionType.Add,
                Item = item,
                From = null,
                To = target,
                Placement = placement
            };

            OnActionExecuted?.Invoke(action);
            ctx.NotifyItemAdded(item, target);

            return ContainerResult.Ok();
        }

        public ContainerResult Remove(LoadoutRootContext ctx, ItemInstance item)
        {
            if (item == null) return ContainerResult.Fail(ContainerError.InvalidItem);

            var source = item.Owner;
            if (source == null) return ContainerResult.Fail(ContainerError.NoSource);

            var placement = source.GetPlacement(item);

            if (!source.TryRemove(item)) return ContainerResult.Fail(ContainerError.RemoveFailed);

            var action = new ContainerAction
            {
                Type = ContainerAction.ActionType.Remove,
                Item = item,
                From = source,
                To = null,
                Placement = placement
            };

            OnActionExecuted?.Invoke(action);
            ctx.NotifyItemRemoved(item);

            return ContainerResult.Ok();
        }
        // public bool Move(ItemInstance item, IItemContainer target, ItemPlacement? targetContext = null)
        // {
        //     // source 캐싱
        //     var source = item.Owner;
        //     if (source == null) return false;
        //     var originalPlacement = source.GetPlacement(item);

        //     // target이 받을 수 있는지 확인
        //     bool canAdd = targetContext == null ? target.CanAdd(item) : target.CanAdd(item, targetContext);
        //     if (!canAdd)
        //     {
        //         Debug.Log("Can not add");
        //         return false;
        //     }
        //     if (!source.TryRemove(item))
        //     {
        //         Debug.Log("Remove failed");
        //         return false;
        //     }

        //     bool added = targetContext == null ? target.TryAdd(item) : target.TryAdd(item, targetContext);
        //     // 롤백
        //     if (!added)
        //     {
        //         Debug.Log("Add failed");
        //         if (originalPlacement == null)
        //         {
        //             source.TryAdd(item);
        //         }
        //         else
        //         {
        //             source.TryAdd(item, originalPlacement);                    
        //         }
        //         return false;
        //     }
        //     return true;
        // }
    }

    public class ContainerAction
    {
        public enum ActionType
        {
            Add,
            Remove,
            Move
        }

        public ActionType Type;
        public ItemInstance Item;
        public IItemContainer From;
        public IItemContainer To;
        public ItemPlacement Placement;
    }

    // public class LoadoutService
    // {
        
    // }
}