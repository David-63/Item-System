#nullable enable

using System.Collections.Generic;
using Dave6.ItemSystem.Domain.Container;
using UnityEngine;

namespace Dave6.ItemSystem.Application.Container
{
    [CreateAssetMenu(fileName = "RootContainerConfigAsset", menuName = "DaveAssets/ItemSystem/RootContainerConfigAsset")]
    public class RootContainerConfigAsset : ScriptableObject
    {
        [SerializeField] List<RootContainerDefinition> _RootContainers = new();

        public RootContainerContext CreateContext()
        {
            var containers = new Dictionary<RootContainerRole, IItemContainer>();

            foreach (var def in _RootContainers)
            {
                IItemContainer? container = null;
                switch (def.type)
                {
                    case ContainerLayout.Grid:
                    container = new GridContainer(def.id.ToString(), def.gridSize);
                    break;
                    case ContainerLayout.Socket:
                    container = new SocketContainer(def.id.ToString(), def.allowedSlots, def.socketLayout);
                    break;
                }
                if (container != null)
                {
                    containers.Add(def.id, container);
                }
            }

            return new RootContainerContext(containers);
        }
    }
}