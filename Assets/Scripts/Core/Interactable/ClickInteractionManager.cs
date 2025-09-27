using System;
using UnityEngine;
using System.Linq;

namespace LikeLoL04
{
    /// <summary>
    /// 处理玩家点击交互的管理器
    /// </summary>


    public class ClickInteractionManager : MonoBehaviour
    {
        public static ClickInteractionManager Instance;

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
            if (player == null) return;

            var mouseMgr = LOLClientMouseEventManager.Instance;
            if (mouseMgr == null) return;

            // Hover：取第一个检测到的对象
            var first = mouseMgr.DetectedObjects.FirstOrDefault();
            if (first != null && first != player)
            {
                player.SetHoverTarget(first);
            }
            else
            {
                player.ClearHoverTarget();
            }

            // 左键（选择单位）逻辑：使用鼠标管理器缓存
            if (mouseMgr.LeftClickThisFrame)
            {
                var l = mouseMgr.LeftClickTarget;
                if (l != null && l != player)
                {
                    OnLeftClickWithTarget?.Invoke(l);
                }
            }

            // 右键点击逻辑（地面移动 / 攻击 / 追击）：使用鼠标管理器缓存
            if (mouseMgr.RightClickThisFrame)
            {
                // 优先地面
                if (mouseMgr.HoveredGround != null && mouseMgr.HasHoveredPoint)
                {
                    player.InteractWithPosition(mouseMgr.HoveredPoint);
                }
                else
                {
                    var r = mouseMgr.RightClickTarget;
                    if (r != null && r != player)
                    {
                        player.InteractWithTarget(r);
                        OnRightClickWithTarget?.Invoke(r);
                    }
                    else if (mouseMgr.HasHoveredPoint)
                    {
                        player.InteractWithPosition(mouseMgr.HoveredPoint);
                    }
                }
            }
        }
    }
}
