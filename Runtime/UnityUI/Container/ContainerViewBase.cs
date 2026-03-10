using Dave6.ItemSystem.Domain.Container;
using UnityEngine.UIElements;

namespace Dave6.ItemSystem.UnityUI
{
    [UxmlElement]
    public abstract partial class ContainerBaseView : VisualElement
    {
        protected IItemContainer _container;

        public abstract void Bind(IItemContainer container);
    }
}
