#nullable enable

using System.Collections.Generic;
using Dave6.Foundation.Math;
using Dave6.ItemSystem.Application.Container;
using Dave6.ItemSystem.Domain.Item;
using UnityEngine;

namespace Dave6.ItemSystem.Domain.Container
{
    public class GridContainer : ItemContainerBase
    {
        protected override ContainerLayout _Layout => ContainerLayout.Grid;

        readonly Int2 _Size;
        readonly ItemInstance?[,] _Grid;
        readonly Dictionary<ItemInstance, GridPlacement> _Placements = new(); // 아이템별 배치정보 저장

        public GridContainer(string containerName, Int2 size)
        {
            ContainerName = containerName;
            _Size = size;
            _Grid = new ItemInstance[size.X, size.Y];
        }

        #region Grid API
        public Int2 GetGridSize() => _Size;

        public override ContainerResult TryAdd(ItemInstance item)
        {
            if (item == null) return ContainerResult.Fail(ContainerError.InvalidItem);
            if (_Items.Contains(item)) return ContainerResult.Fail(ContainerError.ItemExists);

            var result = TryFindAutoPlacement(item, out var placement);
            if (!result.Success) return result;
            return TryAdd(item, placement);
        }
        public override ContainerResult TryAdd(ItemInstance item, ItemPlacement? context)
        {
            if (item == null) return ContainerResult.Fail(ContainerError.InvalidItem);
            if (context is not GridPlacement gp) return ContainerResult.Fail(ContainerError.InvalidPlacementType);
            if (_Items.Contains(item)) return ContainerResult.Fail(ContainerError.ItemExists);
            // if (!CanAdd(item, context)) return false;

            var size = GetItemSize(item, gp.Rotated);

            if (!IsAreaFree(gp.Position, size)) return ContainerResult.Fail(ContainerError.NoSpaceAvailable);

            // base 먼저 수행
            var result = base.TryAdd(item);
            if (!result.Success) return result;

            // grid 반영 및 내부 상태 업데이트
            FillGrid(item, gp.Position, size);
            _Placements[item] = gp;

            return result;
        }

        /// <summary>
        /// placement 조회 (롤백에 쓰임)
        /// </summary>
        public override ItemPlacement? GetPlacement(ItemInstance item)
        {
            if (_Placements.TryGetValue(item, out var placement)) return placement;
            return null;
        }
        public override ContainerResult CanAdd(ItemInstance item)
        {
            if (item == null) return ContainerResult.Fail(ContainerError.InvalidItem);
            if (_Items.Contains(item)) return ContainerResult.Fail(ContainerError.ItemExists);
            return TryFindAutoPlacement(item, out var placement);
        }
        public override ContainerResult CanAdd(ItemInstance item, ItemPlacement context)
        {
            if (context is not GridPlacement gp)
            {
                Debug.Log("Grid Placement 타입 불일치");
                return ContainerResult.Fail(ContainerError.InvalidPlacementType);
            }
            var size = GetItemSize(item, gp.Rotated);
            if (IsAreaFree(gp.Position, size)) return ContainerResult.Ok(null!);
            else return ContainerResult.Fail(ContainerError.NoSpaceAvailable);
        }

        public override ContainerResult TryRemove(ItemInstance item)
        {
            if (!_Placements.TryGetValue(item, out var placement)) return ContainerResult.Fail(ContainerError.InvalidItem);

            var result = base.TryRemove(item);
            if (!result.Success) return result;

            var size = GetItemSize(item, placement.Rotated);
            ClearGrid(placement.Position, size);
            _Placements.Remove(item);

            return result;
        }
        // Debug
        public string GetDebugState()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"--- Grid: {ContainerName} ---");

            for (int y = 0; y < _Size.Y; y++)
            {
                for (int x = 0; x < _Size.X; x++)
                {
                    var item = _Grid[x, y];

                    if (item == null) sb.Append("[  ]");
                    else sb.Append($"[{item.Definition.DisplayName[0]}{item.Definition.DisplayName[1]}]");
                }
                sb.AppendLine();
            }

            sb.AppendLine("Placements:");

            foreach (var kvp in _Placements)
            {
                var p = kvp.Value;
                sb.AppendLine($"{kvp.Key.Definition.DisplayName} at {p.Position} rotated:{p.Rotated}");
            }

            return sb.ToString();
        }
        public override string ToString() => GetDebugState();
        #endregion
        #region Inner Logic
        // Auto Placement
        ContainerResult TryFindAutoPlacement(ItemInstance item, out GridPlacement? placement)
        {
            var result = TryFindPlacement(item, rotated:false, out placement);
            if (result.Success) return result;
            result = TryFindPlacement(item, rotated:true, out placement);
            if (result.Success) return result;

            placement = null;
            return result;
        }
        ContainerResult TryFindPlacement(ItemInstance item, bool rotated, out GridPlacement? placement)
        {
            var size = GetItemSize(item, rotated);
            for (int y = 0; y < _Size.Y; y++)
            for (int x = 0; x < _Size.X; x++)
            {
                var pos = new Int2(x, y);
                if (!IsAreaFree(pos, size)) continue;

                placement = new GridPlacement(pos, rotated);
                return ContainerResult.Ok(null!);
            }
            placement = null;
            return ContainerResult.Fail(ContainerError.NoSpaceAvailable);
        }

        // Grid Logic
        bool IsAreaFree(Int2 position, Int2 size)
        {
            // 자기 자신이랑 충돌하는지 체크
            
            // 범위 검사
            if (position.X < 0 || position.Y < 0) return false;
            if (position.X + size.X > _Size.X) return false;
            if (position.Y + size.Y > _Size.Y) return false;

            // 충돌 검사
            for (int y = 0; y < size.Y; y++)
            for (int x = 0; x < size.X; x++)
            {
                if (_Grid[position.X + x, position.Y + y] != null) return false;
            }

            return true;
        }
        void FillGrid(ItemInstance item, Int2 position, Int2 size)
        {
            for (int y = 0; y < size.Y; y++)
            for (int x = 0; x < size.X; x++)
            {
                _Grid[position.X + x, position.Y + y] = item;
            }
        }
        void ClearGrid(Int2 position, Int2 size)
        {
            for (int x = 0; x < size.X; x++)
            for (int y = 0; y < size.Y; y++)
            {
                _Grid[position.X + x, position.Y + y] = null;
            }
        }

        Int2 GetItemSize(ItemInstance item, bool rotated)
        {
            var original = item.Definition.ItemSize;

            if (!rotated) return original;

            return new Int2(original.Y, original.X);
        }
        #endregion
    }
}
