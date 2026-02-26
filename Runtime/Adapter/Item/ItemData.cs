
using System;
using Dave6.Foundation.Math;

namespace Dave6.ItemSystem.Adapter.Item
{
    /// <summary>
    /// Data Transfer Object
    /// </summary>
    [Serializable]
    public class ItemData
    {
        public string displayName;
        public bool isStackable;
        public int maxStack = 1;
        public Int2 size = new Int2(1, 1);

        // 카테고리 추가
    }
}