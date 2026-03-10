#nullable enable

using Dave6.ItemSystem.Domain.Container;
using Dave6.ItemSystem.Domain.Item;
using UnityEngine;

namespace Dave6.ItemSystem.Application.Item
{
    [CreateAssetMenu(fileName = "ItemDefinitionAsset", menuName = "DaveAssets/ItemSystem/ItemDefinitionAsset")]
    public class ItemDefinitionAsset : ScriptableObject
    {
        [Header("Visual")]
        public Texture? Image;
        public GameObject? WorldPrefab;
        public GameObject? ActivePrefab;

        [Header("Core Data")]
        public ItemData Data = default!;

        public ItemDefinition Create()
        {
            ItemContainerConfig? containerConfig = null;
            switch (Data.ContainerType)
            {
                case ContainerLayout.Grid:
                containerConfig = new ItemGridConfig(Data.GridSize);
                break;
                case ContainerLayout.Socket:
                containerConfig = new ItemSocketConfig(Data.AllowedSlots, Data.SocketLayout);
                break;
            }
            return new ItemDefinition(Data.DisplayName, Data.ItemCategory, Data.ItemSize, containerConfig, Data.EquipableSlots);
        }
    }
}
