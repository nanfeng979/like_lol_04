using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 处理玩家点击交互的管理器
    /// </summary>


    public class ClickInteractionManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private LayerMask interactableLayerMask;

        public MainPlayerInteractable player;

        void Awake()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(1)) // 右键点击
            {
                HandleRightClick();
            }
        }

        private void HandleRightClick()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 10000f, interactableLayerMask))
            {
                Ground ground = hit.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    player.MoveTo(hit.point);
                    return;
                }
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    player.SetInteractionTarget(interactable);
                }
            }
        }
    }
}
