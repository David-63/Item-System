#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace Dave6.ItemSystem.Application.Item
{
    [CreateAssetMenu(fileName = "ItemDatabaseAsset", menuName = "Dave6/ItemSystem/ItemDatabaseAsset")]
    public class ItemDatabaseAsset : ScriptableObject
    {
        [SerializeField] List<ItemDefinitionAsset> _DefinitionAssets = new();
        public IReadOnlyList<ItemDefinitionAsset> DefinitionAssets => _DefinitionAssets;
    }
}