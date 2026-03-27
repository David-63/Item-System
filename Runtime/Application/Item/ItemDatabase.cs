#nullable enable

using System.Collections.Generic;
using System.Linq;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Application.Item
{
    public class ItemDatabase
    {
        Dictionary<string, ItemDefinition> _DefinitionDatabase = new();

        public ItemDatabase(IEnumerable<ItemDefinition> definitions)
        {
            _DefinitionDatabase = definitions.ToDictionary(x => x.ItemId, x => x);
        }

        public ItemDefinition? GetDefinition(string itemId)
        {
            return _DefinitionDatabase.TryGetValue(itemId, out var definition) ? definition : null;
        }
    }
}