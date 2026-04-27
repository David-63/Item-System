#nullable enable

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
        public SocketLayout SocketLayout;
        public List<SlotCategory> AllowedSlots  = new();
    }
}