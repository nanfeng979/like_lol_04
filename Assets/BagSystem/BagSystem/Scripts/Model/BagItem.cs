using System;

namespace Game.Bag.Model
{
    /// <summary>
    /// 背包中的一个条目（itemId + 数量）。
    /// </summary>
    [Serializable]
    public class BagItem
    {
        public int index;       // 槽位索引（行优先），与 JSON 对齐
        public string itemId;
        public int quantity;
    }
}
