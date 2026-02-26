using Dave6.ItemSystem.Domain.Item;
using UnityEngine;

namespace Dave6.ItemSystem.Adapter.Item
{
    [CreateAssetMenu(fileName = "ItemDefinitionAsset", menuName = "DaveAssets/ItemSystem/ItemDefinitionAsset")]
    public class ItemDefinitionAsset : ScriptableObject
    {
        [Header("Visual")]
        public Texture image;
        public GameObject worldPrefab;
        public GameObject activePrefab;

        [Header("Core Data")]
        public ItemData data;

        // item category
        // allow Slot

        public ItemDefinition Create()
        {
            return new ItemDefinition(data.displayName, data.isStackable, data.maxStack, data.size);
        }
    }
}