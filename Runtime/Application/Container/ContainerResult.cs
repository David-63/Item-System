#nullable enable

namespace Dave6.ItemSystem.Application.Container
{
    public enum ContainerError
    {
        None,
        InvalidItem,
        InvalidTarget,
        NoSource,
        CannotAdd,
        RemoveFailed,
        AddFailed,
    }
    public struct ContainerResult
    {
        public bool Success;
        public ContainerError Error;

        public static ContainerResult Ok() => new() { Success = true };
        public static ContainerResult Fail(ContainerError error) => new() { Success = false, Error = error };
    }
}