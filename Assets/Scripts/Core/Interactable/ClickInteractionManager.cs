using System;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 处理玩家点击交互的管理器
    /// </summary>


    public class ClickInteractionManager : MonoBehaviour
    {
        public static ClickInteractionManager Instance;

        [SerializeField] private Camera mainCamera;
        [SerializeField] private LayerMask interactableLayerMask;

        public LOLGameObject player;
        // 左键点击到单位事件（不包括自己）
        public event Action<LOLGameObject> OnLeftClickWithTarget;
        // 右键点击到单位事件（不包括自己）
        public event Action<LOLGameObject> OnRightClickWithTarget;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

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

            // 左键（选择单位）逻辑
            if (Input.GetMouseButtonDown(0) && hasHit)
            {
                LOLGameObject leftTarget = hit.collider.GetComponent<LOLGameObject>();
                if (leftTarget != null && leftTarget != player)
                {
                    OnLeftClickWithTarget?.Invoke(leftTarget);
                }
            }

            // 右键点击逻辑（地面移动 / 攻击 / 追击）
            if (Input.GetMouseButtonDown(1) && hasHit)
            {
                // 地面点击优先
                Ground ground = hit.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    player.InteractWithPosition(hit.point);
                }
                else
                {
                    LOLGameObject rightTarget = hit.collider.GetComponent<LOLGameObject>();
                    if (rightTarget != null && rightTarget != player)
                    {
                        player.InteractWithTarget(rightTarget);
                        OnRightClickWithTarget?.Invoke(rightTarget);
                    }
                }
            }
        }
    }
}
