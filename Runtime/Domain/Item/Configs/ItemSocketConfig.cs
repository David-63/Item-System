#nullable enable

using System.Collections.Generic;
using System.Linq;
using Dave6.ItemSystem.Domain.Container;

namespace Dave6.ItemSystem.Domain.Item
{
    public enum SlotCategory
    {
        //Equipment
        Armor,
        Weapon,
        Storage,

        // Weapon Attachment
        Muzzle,
        Sight,
        Magazine,
        Grip,
        Barrel,
        UnderRail,

        None,
    }
    public class ItemSocketConfig : ItemContainerConfig
    {
        public List<SlotCategory> AllowedSlots = new();
        public SocketLayout SocketLayout;

        public ItemSocketConfig(IEnumerable<SlotCategory> allowedSlots, SocketLayout socketLayout)
        {
            AllowedSlots = allowedSlots.ToList();
            SocketLayout = socketLayout;
        }

        public ItemSocketConfig(params SlotCategory[] allowed) : this(allowed, SocketLayout.LabelAbove) { }
    }
}