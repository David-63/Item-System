using System.Collections.Generic;
using Dave6.Foundation.Math;
using Dave6.ItemSystem.Domain.Item;
using UnityEngine;

namespace Dave6.ItemSystem.Application.Item
{
    [CreateAssetMenu(fileName = "ItemDefinitionAsset", menuName = "Dave6/ItemSystem/ItemDefinitionAsset")]
    public class ItemDefinitionAsset : ScriptableObject
    {
        [Header("Core Identification")]
        public string ItemID = default!;
        public string DisplayName = default!;
        public ItemCategory ItemCategory;
        public Int2 ItemSize;

        [Header("Visual")]
        public Texture Image;
        public GameObject WorldPrefab;
        public GameObject ActivePrefab;

        [Header("Descriptor")]
        public List<EquipDescriptor> EquipDescriptor;
        public List<OwnershipDescriptor> OwnershipDescriptor;
        public List<ExtensionDescriptor> InfluenceDescriptor;

        public ItemDefinition Create()
        {
            return new ItemDefinition(ItemID, DisplayName, ItemCategory, ItemSize, EquipDescriptor, OwnershipDescriptor, InfluenceDescriptor);
        }
    }
}
