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
        [NonSerialized]
        public Action usedAction;          // 从 Lua 绑定过来的使用函数（可为空）
        #endregion

        /// <summary>
        /// 使用该物品（触发 Lua 中配置的 usedAction）。
        /// </summary>
        public void Use()
        {
            if (usedAction == null)
            {
                return;
            }
            
            try { usedAction.Invoke(); }
            catch (Exception ex)
            {
                Debug.LogError($"ItemData.Use 执行 usedAction 异常：{ex}");
            }
        }
    }
}
