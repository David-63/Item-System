using System;
using Dave6.Foundation.Math;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Application.Item
{
    /// <summary>
    /// Data Transfer Object
    /// </summary>
    [Serializable]
    public class ItemData
    {
        public string ItemID = default!;
        public string DisplayName = default!;
        public ItemCategory ItemCategory;
        public Int2 ItemSize;
    }
}