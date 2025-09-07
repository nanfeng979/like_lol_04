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
    }
}
