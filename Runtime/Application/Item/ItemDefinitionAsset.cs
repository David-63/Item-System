#nullable enable

using System.Collections.Generic;
using Dave6.Foundation.Math;
using Dave6.ItemSystem.Domain.Container;
using Dave6.ItemSystem.Domain.Item;
using UnityEngine;

namespace Dave6.ItemSystem.Application.Item
{
    [CreateAssetMenu(fileName = "ItemDefinitionAsset", menuName = "DaveAssets/ItemSystem/ItemDefinitionAsset")]
    public class ItemDefinitionAsset : ScriptableObject
    {
        [Header("Core Identification")]
        public string ItemID = default!;
        public string DisplayName = default!;
        public ItemCategory ItemCategory;
        public Int2 ItemSize;

        [Header("Visual")]
        public Texture? Image;
        public GameObject? WorldPrefab;
        public GameObject? ActivePrefab;

        [Header("Container Configuration")]
        public ContainerLayout ContainerType;
        public Int2 GridSize;
        public SocketLayout SocketLayout;
        public List<SlotCategory> AllowedSlots = new();
        public List<SlotCategory> EquipableSlots = new();

        public ItemDefinition Create()
        {
            ItemContainerConfig? containerConfig = null;
            switch (ContainerType)
            {
                case ContainerLayout.Grid:
                containerConfig = new ItemGridConfig(GridSize);
                break;
                case ContainerLayout.Socket:
                containerConfig = new ItemSocketConfig(AllowedSlots, SocketLayout);
                break;
            }
            return new ItemDefinition(ItemID, DisplayName, ItemCategory, ItemSize, containerConfig, EquipableSlots);
        }
    }
}
