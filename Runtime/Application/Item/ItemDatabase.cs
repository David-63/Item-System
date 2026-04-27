#nullable enable

using System;
using System.Collections.Generic;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Application.Item
{
    public interface IItemFactory
    {
        ItemInstance CreateInstance(string itemId);
    }
    public class ItemEntry
    {
        public ItemDefinitionAsset? ItemDefinitionAsset;
        public ItemDefinition? ItemDefinition;

        public ItemEntry(ItemDefinitionAsset asset)
        {
            ItemDefinitionAsset = asset;
            ItemDefinition = ItemDefinitionAsset.Create();
        }
    }
    public class ItemDatabase
    {
        Dictionary<string, ItemEntry> _ItemEntries = new();

        public ItemDatabase(ItemDatabaseAsset asset)
        {
            _ItemEntries = new();
            foreach (var defAsset in asset.DefinitionAssets)
            {
                var id = defAsset.ItemID;
                _ItemEntries[id] = new ItemEntry(defAsset);
            }
            
        }

        public ItemEntry? GetItemEntry(string itemId)
        {
            if (!_ItemEntries.TryGetValue(itemId, out var itemEntry))
            {
                throw new Exception($"ItemDefinition not found: {itemId}");
            }
            return itemEntry;
        }
    }
}