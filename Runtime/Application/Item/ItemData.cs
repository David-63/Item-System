using System;
using System.Collections.Generic;
using Dave6.Foundation.Math;
using Dave6.ItemSystem.Domain.Container;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Application.Item
{
    /// <summary>
    /// Data Transfer Object
    /// </summary>
    [Serializable]
    public class ItemData
    {
        public string DisplayName;
        public ItemCategory ItemCategory;
        public Int2 ItemSize;
        public ContainerLayout ContainerType;
        public Int2 GridSize;
        public SocketLayout SocketLayout;
        public List<SlotCategory> AllowedSlots;
        public List<SlotCategory> EquipableSlots;
    }
}