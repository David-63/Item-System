using Dave6.ItemSystem.Application.Controller;
using Dave6.ItemSystem.Domain.Container;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dave6.ItemSystem.UnityUI
{
    public class StashPanel : MonoBehaviour
    {
        VisualElement _Root;
        [Header("Stash Controller")]
        [SerializeField] StashController _StashController;

        [Header("Visual Elements")]
        [SerializeField] VisualTreeAsset _GridContainer;
        [SerializeField] VisualTreeAsset _SocketContainer;

        VisualElement _ContentsContainer;
        VisualElement _DragLayer;

        void Awake()
        {
            var doc = GetComponent<UIDocument>();
            _Root = doc.rootVisualElement.Q<VisualElement>("main-root");
            Initialize();
        }
        void Start()
        {
            SetRootContainerView();
        }

        void Initialize()
        {
            _ContentsContainer = _Root.Q<VisualElement>("contents-container");

            _DragLayer = _Root.Q<VisualElement>("drag-layer");
            _DragLayer.pickingMode = PickingMode.Ignore;
            _DragLayer.style.position = Position.Absolute;
            _DragLayer.style.top = 0;
            _DragLayer.style.bottom = 0;
            _DragLayer.style.left = 0;
            _DragLayer.style.right = 0;
        }

        void SetRootContainerView()
        {
            // 컨테이너 생성
            foreach (var container in _StashController._Context.GetRootContainers())
            {
                if (container is GridContainer)
                {
                    var visualElement = new GridContainerView();
                    visualElement.Initialize(_GridContainer);
                    visualElement.Bind(container);
                    _ContentsContainer.Add(visualElement);
                }
                else if (container is SocketContainer)
                {
                    var visualElement = new SocketContainerView();
                    visualElement.Initialize(_SocketContainer);
                    visualElement.Bind(container);
                    _ContentsContainer.Add(visualElement);
                }
            }
        }
    }
}
