#nullable enable

using System.Collections.Generic;
using System.Linq;
using Dave6.ItemSystem.Application.Item;
using Dave6.ItemSystem.Domain.Item;
using UnityEngine;

namespace Dave6.ItemSystem.Application.Item
{
    [CreateAssetMenu(fileName = "ItemDatabaseAsset", menuName = "DaveAssets/ItemSystem/ItemDatabaseAsset")]
    public class ItemDatabaseAsset : ScriptableObject
    {
        [SerializeField] List<ItemDefinitionAsset> _DefinitionAssets = new();
        Dictionary<string, ItemDefinitionAsset> _Database = new();

        public void Initialize()
        {
            _Database = _DefinitionAssets.ToDictionary(x => x.ItemID, x => x);
        }
        public ItemDefinitionAsset? GetAsset(string itemId)
        {
            if (_Database.Count == 0) Initialize();

            return _Database.TryGetValue(itemId, out var asset) ? asset : null;
        }
        public IEnumerable<ItemDefinition> Create()
        {
            foreach (var definition in _DefinitionAssets)
            {
                yield return definition.Create();
            }
        }
    }
}