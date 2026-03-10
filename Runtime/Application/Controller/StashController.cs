using Dave6.Foundation.Math;
using Dave6.ItemSystem.Application.Container;
using Dave6.ItemSystem.Application.Item;
using Dave6.ItemSystem.Domain.Container;
using Dave6.ItemSystem.Domain.Item;
using UnityEngine;

namespace Dave6.ItemSystem.Application.Controller
{
    public class StashController : MonoBehaviour
    {
        [SerializeField] RootContainerConfigAsset _Config;
        public StashContext _Context;
        ContainerService _Service;

        // 네이밍 규칙 수정할것
        [SerializeField] ItemDefinitionAsset backpackItem;
        [SerializeField] ItemDefinitionAsset potionItem;
        [SerializeField] ItemDefinitionAsset firearmItem;

        void Awake()
        {
            _Service = new ContainerService();
            //context = new StashControllerContext();
            _Context = _Config.CreateContext();
        }
        void Start()
        {
            // // 아이템 초기화        
            // var backpack = new ItemInstance(backpackItem.Create());
            // var potion = new ItemInstance(potionItem.Create());
            // var firearm = new ItemInstance(firearmItem.Create());
            // var firearm_clone = new ItemInstance(firearmItem.Create());

            
            // // db 데이터를 읽고 컨텍스트에 배치
            // context.LootRoot.TryAdd(backpack);
            // context.LootRoot.TryAdd(potion);
            // context.LootRoot.TryAdd(firearm);
            // context.LootRoot.TryAdd(firearm_clone);
            
            // var lootGrid = context.LootRoot as GridContainer;
            // Debug.Log(lootGrid);
            // var inventoryGrid = context.InventoryRoot as GridContainer;

            // // 플레이어의 조작 반영 테스트
            // service.Move(backpack, context.InventoryRoot);
            // service.Move(firearm, backpack.ownedContainer);
            // service.Move(potion, backpack.ownedContainer);
            // service.Move(potion, context.InventoryRoot);


            // Debug.Log(lootGrid);
            // Debug.Log(inventoryGrid);
            // Debug.Log(backpack.ownedContainer);
        }
    }
}