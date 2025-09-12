using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using System.Linq;

namespace LikeLoL04
{
    /// <summary>
    /// LOL客户端控制器
    /// </summary>
    public class LOLClientController : MonoBehaviour
    {

        public static LOLClientController Instance { get; private set; }

        private Camera mainCamera;

        private Camp Camp = new Camp { Type = CampType.Blue };

        // 缓存每次鼠标检测到的所有 LOLGameObject（只读供外部读取）
        private readonly List<LOLGameObject> cachedDetectedObjects = new List<LOLGameObject>();
        public IReadOnlyList<LOLGameObject> DetectedObjects => cachedDetectedObjects.AsReadOnly();

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
            HandleMouseDetection();
        }

        private void HandleMouseDetection()
        {
            // 每次检测开始前清空上一次的缓存
            cachedDetectedObjects.Clear();

            // 确保LOLGameConfig实例存在
            if (LOLGameConfig.Instance == null) return;

            // 从鼠标位置发射射线
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            // 获取射线所有命中的物体（按距离排序）
            RaycastHit[] hits = Physics.RaycastAll(ray);
            if (hits != null && hits.Length > 0)
            {
                var orderedHits = hits.OrderBy(h => h.distance);
                bool anyEnemy = false;
                var seen = new HashSet<LOLGameObject>();

                foreach (var h in orderedHits)
                {
                    var go = h.collider.GetComponent<LOLGameObject>();
                    if (go == null) continue;
                    // 避免重复添加同一个游戏对象
                    if (seen.Add(go))
                    {
                        cachedDetectedObjects.Add(go);
                        if (IsEnemyCamp(go.Camp)) anyEnemy = true;
                    }
                }

                // 根据是否存在敌对对象切换光标
                if (anyEnemy)
                    SetCursor(LOLGameConfig.Instance.AttackCursor);
                else
                    SetCursor(LOLGameConfig.Instance.DefaultCursor);
            }
            else
            {
                // 没有碰到任何物体，使用默认光标
                SetCursor(LOLGameConfig.Instance.DefaultCursor);
            }
        }

        private bool IsEnemyCamp(Camp targetCamp)
        {
            // 判断目标阵营是否为敌对
            // 这里假设Blue和Red互为敌对，Neutral为中立
            if (Camp.Type == CampType.Blue && targetCamp.Type == CampType.Red)
                return true;
            if (Camp.Type == CampType.Red && targetCamp.Type == CampType.Blue)
                return true;

            return false;
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

    }
}
