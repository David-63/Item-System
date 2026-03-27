
using System;
using System.Collections.Generic;
using Dave6.ItemSystem.Application.Container;
using Dave6.ItemSystem.Application.Item;
using Dave6.ItemSystem.Domain.Container;
using Dave6.ItemSystem.Domain.Item;
using Dave6.ItemSystem.Persistence.Dto;
using UnityEngine;

namespace Dave6.ItemSystem.Persistence.Mapper
{
    public class LoadoutService
    {
        ItemDatabase _ItemDatabase;

        Dictionary<ItemInstance, string> _ItemIds = new();
        Dictionary<IItemContainer, string> _ContainerIds = new();

        Dictionary<string, ItemInstance> _ItemDict = new();
        Dictionary<string, IItemContainer> _ContainerDict = new();
        SaveData _SaveData;

        public LoadoutService(ItemDatabase database) => _ItemDatabase = database;


        #region Export (Domain -> DTO)
        public SaveData ExportLoadout(RootContainerContext context)
        {
            // 데이터 초기화
            _SaveData = new SaveData
            {
                Items = new List<ItemDto>(),
                Containers = new List<ContainerDto>(),
                Placements = new List<ItemPlaceDto>()
            };
            // id값 초기화
            _ItemIds.Clear();
            _ContainerIds.Clear();

            // Root container 등록
            foreach (var (role, container) in context.GetAll())
            {
                if (role == RootContainerRole.Loot) continue;

                string containerId = role.ToString();   // guid 대신 고정된 키 사용
                _ContainerIds[container] = containerId;

                _SaveData.Containers.Add(new ContainerDto
                {
                    ContainerId = containerId,
                    ContainerType = ResolveType(container)
                });
                ExportContainer(container);
            }

            return _SaveData;
        }
        /// <summary>
        /// 컨테이너 내부에 아이템을 순회하면서 저장
        /// </summary>
        void ExportContainer(IItemContainer container)
        {
            // 중복 방지 / 순환 구조 대응
            bool isNew = !_ContainerIds.ContainsKey(container);

            if (isNew)
            {
                Debug.Log("새 컨테이너 생성");
                string containerId = GenerateId();          // guid 생성 및 할당
                _ContainerIds[container] = containerId;

                _SaveData.Containers.Add(new ContainerDto
                {
                    ContainerId = containerId,
                    ContainerType = ResolveType(container)
                });
            }
            string id = _ContainerIds[container];
            // 세이브 데이터에 컨테이너가 가지고 있는 아이템 저장
            foreach (var item in container.Items)
            {
                ExportItem(item);

                var placement = container.GetPlacement(item);
                if (placement == null) continue;
                ExportPlacement(id, item, placement);
            }
        }
        /// <summary>
        /// 아이템 저장 및 내부 컨테이너 재귀 처리
        /// </summary>
        void ExportItem(ItemInstance item)
        {
            // 중복 방지
            if (_ItemIds.ContainsKey(item)) return;
            Debug.Log($"{item.Definition.DisplayName} 저장");
            string itemId = GenerateId();
            _ItemIds[item] = itemId;

            // 내부 컨테이너 재귀 호출
            string ownedContainerId = null;
            if (item.OwnedContainer != null)
            {
                ExportContainer(item.OwnedContainer);
                ownedContainerId = _ContainerIds[item.OwnedContainer];
            }
            _SaveData.Items.Add(new ItemDto
            {
                ItemInstanceId = itemId,
                ItemDefinitionId = item.Definition.ItemId.ToString(),
                OwnedContainerId = ownedContainerId
            });
        }
        /// <summary>
        /// 아이템 배치정보 저장
        /// </summary>
        void ExportPlacement(string containerId, ItemInstance item, ItemPlacement placement)
        {
            _SaveData.Placements.Add(new ItemPlaceDto
            {
                ItemInstanceId = _ItemIds[item],
                ContainerId = containerId,
                Position = placement is GridPlacement gp ? gp.Position : default,
                Rotated = placement is GridPlacement gp2 && gp2.Rotated,
                SlotIndex = placement is SlotPlacement sp ? sp.SlotIndex : -1
            });
        }

        string GenerateId() => Guid.NewGuid().ToString();
        string ResolveType(IItemContainer container)
        {
            return container switch
            {
                GridContainer => "Grid",
                SocketContainer => "Socket",
                _ => throw new NotImplementedException()
            };
        }
        #endregion
        #region Import (DTO -> Domain)

        public void ImportLoadout(RootContainerContext context, SaveData saveData)
        {
            _ItemDict.Clear();
            _ContainerDict.Clear();

            // context 맵핑
            foreach (var (role, container)in context.GetAll())
            {
                string id = role.ToString();
                _ContainerDict[id] = container;
                //container.Clear();                // 컨테이너 내부 초기화 기능이 필요하면 추가하기
            }
            // item 생성
            foreach (var iDto in saveData.Items)
            {
                ItemInstance item = CreateItem(iDto);
                _ItemDict[iDto.ItemInstanceId] = item;

                if (item.OwnedContainer == null || iDto.OwnedContainerId == null) continue;
                _ContainerDict[iDto.OwnedContainerId] = item.OwnedContainer;
            }
            // placement 적용
            foreach (var pDto in saveData.Placements)
            {
                var item = _ItemDict[pDto.ItemInstanceId];
                var container = _ContainerDict[pDto.ContainerId];

                container.TryAdd(item, CreatePlacement(pDto));
            }
        }

        ItemInstance CreateItem(ItemDto itemDto)
        {
            var definition = _ItemDatabase.GetDefinition(itemDto.ItemDefinitionId);
            return new ItemInstance(definition);
        }
        ItemPlacement CreatePlacement(ItemPlaceDto placementDto)
        {
            if (placementDto.SlotIndex >= 0) return new SlotPlacement(placementDto.SlotIndex);
            return new GridPlacement(placementDto.Position, placementDto.Rotated);
        }

        #endregion
    }
}