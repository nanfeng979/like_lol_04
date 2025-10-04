using System.Collections.Generic;
using Game.Bag.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Bag.View
{
    /// <summary>
    /// 背包视图容器：持有网格布局与槽位/背景预制体的引用。
    /// </summary>
    public class BagView : MonoBehaviour
    {
        [Header("预制体")]
        [Tooltip("用于实例化的槽位视图预制体")]
        public BagSlotView slotPrefab;

        [Header("容器")]
        [Tooltip("槽位父节点（挂有 GridLayoutGroup 的容器）")]
        public RectTransform slotsRoot;

        [Header("每格背景网格")]
        [Tooltip("背景格子父节点（层级上应位于 slotsRoot 之前/后面）")]
        public RectTransform backgroundRoot;           // 背景格子父节点（在层级上位于 slotsRoot 之前）
        [Tooltip("背景格子预制体（仅视觉，不拦截射线）")]
        public GameObject backgroundPrefab;            // 背景格子预制（仅视觉）

        #region Public Methods

        public List<BagSlotView> CreateSlots(int count, BagController controller)
        {
            List<BagSlotView> _slots = new List<BagSlotView>();

            if (slotPrefab == null || slotsRoot == null || backgroundRoot == null || backgroundPrefab == null)
            {
                Debug.LogError("BagView.CreateSlots：缺少必要的预制体或容器引用，无法创建槽位。");
                return _slots;
            }

            // 清空旧的
            foreach (Transform child in slotsRoot)
            {
                Destroy(child.gameObject);
            }

            // 创建新的
            for (int i = 0; i < count; i++)
            {
                // 槽位
                var slotObj = Instantiate(slotPrefab, slotsRoot);
                slotObj.name = $"Slot_{i}";
                var slotView = slotObj.GetComponent<BagSlotView>();
                if (slotView != null)
                {
                    slotView.Bind(i, controller);
                    slotView.SetCount(0); // 初始数量为0
                    _slots.Add(slotView);
                }
            }

            return _slots;
        }

        public List<GameObject> CreateBackgrounds(int count)
        {
            List<GameObject> _backgrounds = new List<GameObject>();

            if (backgroundRoot == null || backgroundPrefab == null)
            {
                Debug.LogError("BagView.CreateBackgrounds：缺少必要的预制体或容器引用，无法创建背景格子。");
                return _backgrounds;
            }

            // 清空旧的
            foreach (Transform child in backgroundRoot)
            {
                Destroy(child.gameObject);
            }

            // 创建新的
            for (int i = 0; i < count; i++)
            {
                var bgObj = Instantiate(backgroundPrefab, backgroundRoot);
                bgObj.name = $"Background_{i}";
                _backgrounds.Add(bgObj);

                Image image = bgObj.GetComponent<Image>();
                if (image != null)
                {
                    image.raycastTarget = false; // 背景不拦截射线
                }
            }

            return _backgrounds;
        }

        #endregion

    }
}
