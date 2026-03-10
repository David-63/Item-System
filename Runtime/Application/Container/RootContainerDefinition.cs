#nullable enable

using System;
using System.Collections.Generic;
using Dave6.Foundation.Math;
using Dave6.ItemSystem.Domain.Container;
using Dave6.ItemSystem.Domain.Item;

namespace Dave6.ItemSystem.Application.Container
{
    [Serializable]
    public enum RootContainerRole
    {
        Equipment,
        Inventory,
        Loot,
    }
    [Serializable]
    public class RootContainerDefinition
    {
        public RootContainerRole id;
        public ContainerLayout type;
        public Int2 gridSize;
        public SocketLayout socketLayout;
        public List<SlotCategory> allowedSlots  = new();
    }
}