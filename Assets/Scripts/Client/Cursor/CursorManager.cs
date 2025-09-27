using System.Linq;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 管理鼠标光标的组件
    /// </summary>
    public class CursorManager : MonoBehaviour
    {
        public static CursorManager Instance { get; private set; }

        private Camp m_camp = new Camp { Type = CampType.Blue };

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
            HandleCursor();
        }

        private void HandleCursor()
        {
            // 获取当前鼠标悬停的游戏对象
            var mouseMgr = LOLClientMouseEventManager.Instance;
            var hoveredObject = mouseMgr.DetectedObjects?.FirstOrDefault();

            if (hoveredObject != null)
            {
                if (IsEnemyCamp(hoveredObject.Camp))
                {
                    SetCursor(LOLGameConfig.Instance.AttackCursor);
                    return;
                }
                SetCursor(LOLGameConfig.Instance.DefaultCursor);
                return;
            }

            // 无单位，检查是否命中 Ground
            if (mouseMgr.HoveredGround != null)
            {
                // 这里先沿用默认光标（或根据需要替换为移动指示光标）
                SetCursor(LOLGameConfig.Instance.DefaultCursor);
                return;
            }

            // 无任何命中
            SetCursor(null);
        }

        private void SetCursor(Texture2D cursorTexture)
        {
            if (cursorTexture != null && LOLGameConfig.Instance != null)
            {
                Cursor.SetCursor(cursorTexture, LOLGameConfig.Instance.CursorHotspot, CursorMode.Auto);
            }
            else
            {
                // 如果没有指定的光标纹理，恢复默认系统光标
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }

        private bool IsEnemyCamp(Camp targetCamp)
        {
            // 判断目标阵营是否为敌对
            // 这里假设Blue和Red互为敌对，Neutral为中立
            if (m_camp.Type == CampType.Blue && targetCamp.Type == CampType.Red)
                return true;
            if (m_camp.Type == CampType.Red && targetCamp.Type == CampType.Blue)
                return true;

            return false;
        }
    }
}
