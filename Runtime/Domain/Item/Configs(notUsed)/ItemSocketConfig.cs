#nullable enable

using System.Collections.Generic;
using System.Linq;
using Dave6.ItemSystem.Domain.Container;

namespace Dave6.ItemSystem.Domain.Item
{
    public class ItemSocketConfig : ItemContainerConfig
    {
        public List<SlotCategory> AllowedSlots = new();
        public SocketLabelLayout SocketLayout;

        public ItemSocketConfig(IEnumerable<SlotCategory> allowedSlots, SocketLabelLayout socketLayout)
        {
            AllowedSlots = allowedSlots.ToList();
            SocketLayout = socketLayout;
        }

        public ItemSocketConfig(params SlotCategory[] allowed) : this(allowed, SocketLabelLayout.LabelAbove) { }
    }
}