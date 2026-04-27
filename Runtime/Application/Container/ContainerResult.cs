#nullable enable

using Dave6.ItemSystem.Domain.Container;

namespace Dave6.ItemSystem.Application.Container
{
    public enum ContainerError
    {
        None,
        InvalidItem,
        InvalidTarget,
        ItemExists,
        NoSource,
        NoSpaceAvailable,
        InvalidPlacementType,
        CannotAdd,
        RemoveFailed,
        AddFailed,

    }
    public struct ContainerResult
    {
        public bool Success;
        public ContainerAction Action;
        public ContainerError Error;

        public static ContainerResult Ok(ContainerAction action) => new() { Success = true, Action = action };
        public static ContainerResult Fail(ContainerError error) => new() { Success = false, Error = error };
    }
}