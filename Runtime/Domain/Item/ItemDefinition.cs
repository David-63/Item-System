#nullable enable

using System;
using System.Collections.Generic;
using Dave6.Foundation.Math;

namespace Dave6.ItemSystem.Domain.Item
{
    // 아이템 카테고리
    public enum ItemCategory
    {
        Armor,
        Weapon,
        Consumable,
        Ammo,
        Bag,
        Mod,
        Misc
    }

    public class ItemDefinition
    {
        public string ItemId { get;}
        public string DisplayName { get; }
        public ItemCategory ItemCategory { get; }
        public Int2 ItemSize { get; }
        public ItemContainerConfig? ContainerConfig { get; }
        readonly HashSet<SlotCategory> _EquipableSlots;
        public IEnumerable<SlotCategory> EquipableSlots => _EquipableSlots;


        public ItemDefinition(string itemId, string displayName, ItemCategory itemCategory, Int2 itemSize
        , ItemContainerConfig? containerConfig, IEnumerable<SlotCategory>? equipableSlots)
        {
            ItemId = itemId;
            DisplayName = displayName;
            ItemCategory = itemCategory;
            ItemSize = itemSize;
            ContainerConfig = containerConfig;
            _EquipableSlots = equipableSlots != null ? new HashSet<SlotCategory>(equipableSlots) : new HashSet<SlotCategory>();
        }
    }
}