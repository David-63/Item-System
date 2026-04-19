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
    public class ItemDatabase
    {
        Dictionary<string, ItemDefinition> _Definitions = new();

        public ItemDatabase(ItemDatabaseAsset asset)
        {
            _Definitions = new Dictionary<string, ItemDefinition>();
            foreach (var defAsset in asset.Definitions)
            {
                var def = defAsset.Create();
                _Definitions[def.ItemId] = def;
            }
        }

        public ItemDefinition? GetDefinition(string itemId)
        {
            if (!_Definitions.TryGetValue(itemId, out var def))
            {
                throw new Exception($"ItemDefinition not found: {itemId}");
            }
            return def;
        }
    }
}