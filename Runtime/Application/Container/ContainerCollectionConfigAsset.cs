#nullable enable

using System;
using System.Collections.Generic;
using Dave6.ItemSystem.Domain.Container;
using UnityEngine;

namespace Dave6.ItemSystem.Application.Container
{
    [CreateAssetMenu(fileName = "RootContainerConfigAsset", menuName = "DaveAssets/ItemSystem/RootContainerConfigAsset")]
    public class ContainerCollectionConfigAsset : ScriptableObject
    {
        [SerializeField] List<ContainerCollectionDefinition> _RootCollections = new();

        public LoadoutRootContext CreateContext()
        {
            var collections = new Dictionary<ExtensionRole, ContainerCollection>();

            foreach (var def in _RootCollections)
            {
                // base 생ㅇ성
                IItemContainer baseContainer = def.Type switch
                {
                    ContainerLayout.Grid => new GridContainer(def.Id.ToString(), def.GridSize),
                    ContainerLayout.Socket => new SocketContainer(def.Id.ToString(), def.AllowedSlots, def.SocketLayout),
                    _ => throw new InvalidOperationException($"Unsupported layout: {def.Type}"),
                };

                // collection 생성
                var collection = new ContainerCollection(baseContainer);
                collections.Add(def.Id, collection);
            }

            return new LoadoutRootContext(collections);
        }
    }
}