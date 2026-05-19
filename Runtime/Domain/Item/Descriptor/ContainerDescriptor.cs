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
    
    /// <summary>
    /// 이 아이템이 직접 소유하는 컨테이너
    /// </summary>
    [Serializable]
    public class OwnershipDescriptor : ContainerDescriptor
    {
        // identity (아직 필요없음)
    }
    /// <summary>
    /// 아이템이 loadout 구조에 영향을 주는 extension
    /// </summary>
    [Serializable]
    public class ExtensionDescriptor : ContainerDescriptor
    {
        // target (Container Collection)
        public ExtensionRole Target;

        // source identity
    }
    /// <summary>
    /// 어디에 장착 가능하는가
    /// </summary>
    [Serializable]
    public class EquipDescriptor
    {
        // allowed slots
        public SlotCategory EquipableSlot;
    }
}