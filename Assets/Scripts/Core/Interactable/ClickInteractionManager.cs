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

        public LOLGameObject player;

        void Awake()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
        }

        void Update()
        {
            UpdateInteraction();
        }

        /// <summary>
        /// 合并点击与悬停：单次 Raycast 同时处理右键交互与 Hover 目标
        /// </summary>
        private void UpdateInteraction()
        {
            if (player == null || mainCamera == null) return;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            bool hasHit = Physics.Raycast(ray, out RaycastHit hit, 10000f, interactableLayerMask);

            // Hover 逻辑
            if (hasHit)
            {
                var hoverLol = hit.collider.GetComponent<LOLGameObject>();
                if (hoverLol != null && hoverLol != player)
                {
                    player.SetHoverTarget(hoverLol);
                }
                else
                {
                    player.ClearHoverTarget();
                }
            }
            else
            {
                player.ClearHoverTarget();
            }

            // 右键点击逻辑（仅在按下当帧执行，不需要第二次 Raycast）
            if (Input.GetMouseButtonDown(1) && hasHit)
            {
                // 地面点击优先
                Ground ground = hit.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    player.InteractWithPosition(hit.point);
                    return;
                }

                LOLGameObject lol = hit.collider.GetComponent<LOLGameObject>();
                if (lol != null && lol != player)
                {
                    player.InteractWithTarget(lol);
                }
            }
        }
    }
}
