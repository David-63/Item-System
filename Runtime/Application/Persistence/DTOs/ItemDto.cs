using System;

namespace Dave6.ItemSystem.Persistence.Dto
{
    [Serializable]
    public class ItemDto
    {
        public string ItemInstanceId;
        public string ItemDefinitionId;
        public string OwnedContainerId;

        // 옵션 추가
    }
}