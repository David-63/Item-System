using System;
using System.Collections.Generic;
using Dave6.Foundation.Math;
using Dave6.ItemSystem.Application.Container;
using Dave6.ItemSystem.Domain.Container;

namespace Dave6.ItemSystem.Domain.Item
{
    public class ContainerDescriptor
    {
        // layout
        public ContainerLayout Layout;
        // shape / rule
        public Int2 GridSize;
        public SocketLayoutConfig SocketConfig;
        // public List<SlotCategory> AllowedSlots;
        // public SocketLabelLayout SocketLayout;
    }

    [Serializable]
    public class OwnershipDescriptor : ContainerDescriptor
    {
        // identity (아직 필요없음)
    }
    [Serializable]
    public class ExtensionDescriptor : ContainerDescriptor
    {
        // target (Container Collection)
        public ExtensionRole Target;

        // source identity
    }
    [Serializable]
    public class EquipDescriptor
    {
        // allowed slots
        public SlotCategory EquipableSlot;
    }
}