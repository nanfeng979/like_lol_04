using System;
using UnityEngine;

namespace Game.Bag.Model
{
    /// <summary>
    /// 道具静态数据（来自 items.json）。非 MonoBehaviour，纯数据模型。
    /// </summary>
    [Serializable]
    public class ItemData
    {
        #region Fields
        public string itemId;              // 唯一 ID
        public string displayName;         // 展示名（首版不渲染，未来用于 Tooltip）
        public string iconAddress;         // Addressables Key
        public int maxStack = 99;          // 最大叠堆数，缺省 99
        #endregion
    }
}
