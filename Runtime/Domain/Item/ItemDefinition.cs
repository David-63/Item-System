using System.Collections.Generic;
using Dave6.Foundation.Math;

namespace Dave6.ItemSystem.Domain.Item
{
    public class ItemDefinition
    {
        #region Core
        public string ItemId { get;}
        public string DisplayName { get; }
        public ItemCategory ItemCategory { get; }
        public Int2 ItemSize { get; }
        #endregion
        #region Equip condition
        public IEnumerable<EquipDescriptor> EquipDescriptors { get; }
        #endregion
        #region Ownership
        public IEnumerable<OwnershipDescriptor> OwnershipDescriptors { get; }
        #endregion
        #region Influence
        public IEnumerable<ExtensionDescriptor> InfluenceDescriptors { get; }
        #endregion

        public ItemDefinition(string itemId, string displayName, ItemCategory itemCategory, Int2 itemSize
        , IEnumerable<EquipDescriptor> equipDescriptors, IEnumerable<OwnershipDescriptor> ownershipDescriptors, IEnumerable<ExtensionDescriptor> influenceDescriptors)
        {
            ItemId = itemId;
            DisplayName = displayName;
            ItemCategory = itemCategory;
            ItemSize = itemSize;

            EquipDescriptors = equipDescriptors;
            OwnershipDescriptors = ownershipDescriptors;
            InfluenceDescriptors = influenceDescriptors;
        }
    }
}