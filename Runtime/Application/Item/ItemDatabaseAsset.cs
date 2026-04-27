#nullable enable

using System.Collections.Generic;
using System.Linq;
using Dave6.ItemSystem.Domain.Item;
using UnityEngine;

namespace Dave6.ItemSystem.Application.Item
{
    [CreateAssetMenu(fileName = "ItemDatabaseAsset", menuName = "DaveAssets/ItemSystem/ItemDatabaseAsset")]
    public class ItemDatabaseAsset : ScriptableObject
    {
        [SerializeField] List<ItemDefinitionAsset> _DefinitionAssets = new();
        public IReadOnlyList<ItemDefinitionAsset> DefinitionAssets => _DefinitionAssets;
    }
}