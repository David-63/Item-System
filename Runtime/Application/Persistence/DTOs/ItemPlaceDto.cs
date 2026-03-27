using System;
using Dave6.Foundation.Math;

namespace Dave6.ItemSystem.Persistence.Dto
{
    [Serializable]
    public class ItemPlaceDto
    {
        public string ItemInstanceId;
        public string ContainerId;
        public int SlotIndex;
        public Int2 Position;
        public bool Rotated;
    }
}