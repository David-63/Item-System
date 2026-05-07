using System;
using System.Collections.Generic;
using Dave6.Foundation.Math;
using Dave6.ItemSystem.Domain.Container;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Application.Container
{
    [Serializable]
    public enum ExtensionRole
    {
        Equipment,
        Inventory,
    }
    [Serializable]
    public class ContainerCollectionDefinition
    {
        public ExtensionRole Id;
        public ContainerLayout Type;
        public Int2 GridSize;
        public SocketLayoutConfig SocketConfig;
    }

    // 나중에 레이아웃을 각각 SO로 만든다면?
    public interface IContainerLayoutConfig {}

    // [Serializable]
    // public class GridLayoutConfig : IContainerLayoutConfig
    // {
    //     public Int2 GridSize;
    // }

    [Serializable]
    public class SocketLayoutConfig : IContainerLayoutConfig
    {
        public SocketFlowLayout Flow;
        public SocketLabelLayout Label;
        public List<SlotCategory> AllowedSlots = new();
    }
}