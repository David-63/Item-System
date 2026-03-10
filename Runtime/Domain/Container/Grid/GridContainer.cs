#nullable enable

using System.Collections.Generic;
using Dave6.Foundation.Math;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Domain.Container
{
    public class GridContainer : ItemContainerBase
    {
        readonly Int2 _Size;
        readonly ItemInstance?[,] _Grid;
        readonly Dictionary<ItemInstance, GridPlacement> _Placements = new(); // 아이템별 배치정보 저장

        public GridContainer(string containerName, Int2 size)
        {
            ContainerName = containerName;
            _Size = size;
            _Grid = new ItemInstance[size.x, size.y];
        }

        #region Grid API
        public Int2 GetGridSize() => _Size;

        public override bool TryAdd(ItemInstance item)
        {
            if (item == null) return false;
            if (_Items.Contains(item)) return false;
            if (!TryFindAutoPlacement(item, out var placement)) return false;
            return TryAdd(item, placement);
        }
        public override bool TryAdd(ItemInstance item, ItemPlacement? context)
        {
            if (item == null) return false;
            if (context is not GridPlacement gp) return false;
            if (_Items.Contains(item)) return false;
            // if (!CanAdd(item, context)) return false;

            var size = GetItemSize(item, gp.Rotated);

            if (!IsAreaFree(gp.Position, size)) return false;

            // base 먼저 수행
            if (!base.TryAdd(item)) return false;

            // grid 반영 및 내부 상태 업데이트
            FillGrid(item, gp.Position, size);
            _Placements[item] = gp;

            return true;
        }

        /// <summary>
        /// placement 조회 (롤백에 쓰임)
        /// </summary>
        public override ItemPlacement? GetPlacement(ItemInstance item)
        {
            if (_Placements.TryGetValue(item, out var placement)) return placement;
            return null;
        }
        public override bool CanAdd(ItemInstance item)
        {
            if (item == null) return false;
            if (_Items.Contains(item)) return false;
            return TryFindAutoPlacement(item, out var placement);
        }
        public override bool CanAdd(ItemInstance item, ItemPlacement context)
        {
            if (context is not GridPlacement gp) return false;
            var size = GetItemSize(item, gp.Rotated);
            return IsAreaFree(gp.Position, size);
        }

        public override bool TryRemove(ItemInstance item)
        {
            if (!_Placements.TryGetValue(item, out var placement)) return false;

            if (!base.TryRemove(item)) return false;

            var size = GetItemSize(item, placement.Rotated);
            ClearGrid(placement.Position, size);
            _Placements.Remove(item);

            return true;
        }
        // Debug
        public string GetDebugState()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"--- Grid: {ContainerName} ---");

            for (int y = 0; y < _Size.y; y++)
            {
                for (int x = 0; x < _Size.x; x++)
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
        bool TryFindAutoPlacement(ItemInstance item, out GridPlacement? placement)
        {
            if (TryFindPlacement(item, rotated:false, out placement)) return true;
            if (TryFindPlacement(item, rotated:true, out placement)) return true;

            placement = null;
            return false;
        }
        bool TryFindPlacement(ItemInstance item, bool rotated, out GridPlacement? placement)
        {
            var size = GetItemSize(item, rotated);
            for (int y = 0; y < _Size.y; y++)
            for (int x = 0; x < _Size.x; x++)
            {
                var pos = new Int2(x, y);
                if (!IsAreaFree(pos, size)) continue;

                placement = new GridPlacement(pos, rotated);
                return true;
            }
            placement = null;
            return false;
        }

        // Grid Logic
        bool IsAreaFree(Int2 position, Int2 size)
        {
            // 범위 검사
            if (position.x < 0 || position.y < 0) return false;
            if (position.x + size.x > _Size.x) return false;
            if (position.y + size.y > _Size.y) return false;

            // 충돌 검사
            for (int y = 0; y < size.y; y++)
            for (int x = 0; x < size.x; x++)
            {
                if (_Grid[position.x + x, position.y + y] != null) return false;
            }

            return true;
        }
        void FillGrid(ItemInstance item, Int2 position, Int2 size)
        {
            for (int y = 0; y < size.y; y++)
            for (int x = 0; x < size.x; x++)
            {
                _Grid[position.x + x, position.y + y] = item;
            }
        }
        void ClearGrid(Int2 position, Int2 size)
        {
            for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
            {
                _Grid[position.x + x, position.y + y] = null;
            }
        }

        Int2 GetItemSize(ItemInstance item, bool rotated)
        {
            var original = item.Definition.ItemSize;

            if (!rotated) return original;

            return new Int2(original.y, original.x);
        }
        #endregion
    }
}
