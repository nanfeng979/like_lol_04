using Game.Bag.Model;
using Game.Bag.View;
using UnityEngine;

namespace Game.Bag.Controller
{
    /// <summary>
    /// BagController 的单例与只读访问器。
    /// </summary>
    public partial class BagController : MonoBehaviour
    {
        #region Singleton
        /// <summary>
        /// 全局唯一实例。
        /// </summary>
        public static BagController Instance { get; private set; }
        #endregion

        #region Accessors
        /// <summary>
        /// 只读访问当前背包数据。
        /// </summary>
        public BagDatabase CurrentBag => _bagDatabase;

        /// <summary>
        /// 获取指定索引的展示名（来自静态表 ItemData；无表回退 itemId）。
        /// </summary>
        public string GetDisplayNameAt(int index)
        {
            if (_bagDatabase == null || _bagDatabase.items == null) return null;
            if (index < 0 || index >= _bagDatabase.items.Count) return null;
            BagItem bi = _bagDatabase.items[index];
            if (bi == null) return null;
            if (_itemDatabase != null && _itemDatabase.TryGet(bi.itemId, out var data) && data != null)
            {
                return data.displayName;
            }
            return bi.itemId;
        }

        /// <summary>
        /// 槽位边界（SlotsRoot 的父节点）。
        /// </summary>
        public RectTransform SlotsBoundary => bagView != null && bagView.slotsRoot != null ? bagView.slotsRoot.parent as RectTransform : null;
        #endregion

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("BagController：检测到重复实例，自动销毁新的重复对象。");
                Destroy(this);
                return;
            }
            Instance = this;
        }
    }
}
