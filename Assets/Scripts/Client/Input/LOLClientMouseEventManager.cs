using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 管理客户端鼠标事件的组件
    /// 负责：检测鼠标悬停的游戏对象
    /// </summary>
    public class LOLClientMouseEventManager : MonoBehaviour
    {
        public static LOLClientMouseEventManager Instance { get; private set; }
        private Camera mainCamera;

        // 缓存每次鼠标检测到的所有 LOLGameObject（只读供外部读取）
        private readonly List<LOLGameObject> cachedDetectedObjects = new List<LOLGameObject>();
        public IReadOnlyList<LOLGameObject> DetectedObjects => cachedDetectedObjects.AsReadOnly();

        // 若本帧没有命中任何 LOLGameObject，则尝试记录命中的 Ground（环境）
        private Ground hoveredGround;
        public Ground HoveredGround => hoveredGround;

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
            hoveredGround = null;

            // 如果鼠标超出游戏窗口范围，则不做射线检测，直接返回（保持缓存为空）
            Vector3 mp = Input.mousePosition;
            if (mp.x < 0 || mp.y < 0 || mp.x > Screen.width || mp.y > Screen.height)
            {
                return;
            }

            // 确保LOLGameConfig实例存在
            if (LOLGameConfig.Instance == null) return;

            // 从鼠标位置发射射线
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            // 获取射线所有命中的物体（按距离排序）
            RaycastHit[] hits = Physics.RaycastAll(ray);
            if (hits == null || hits.Length == 0) return;

            var orderedHits = hits.OrderBy(h => h.distance);
            var seen = new HashSet<LOLGameObject>();

            foreach (var h in orderedHits)
            {
                var go = h.collider.GetComponent<LOLGameObject>();
                if (go != null)
                {
                    if (seen.Add(go))
                    {
                        cachedDetectedObjects.Add(go);
                    }
                }
            }

            // 如果没有任何 LOLGameObject，被视为环境命中，尝试取最近的 Ground
            if (cachedDetectedObjects.Count == 0)
            {
                foreach (var h in orderedHits)
                {
                    var g = h.collider.GetComponent<Ground>();
                    if (g != null)
                    {
                        hoveredGround = g;
                        break;
                    }
                }
            }
        }
    }
}
