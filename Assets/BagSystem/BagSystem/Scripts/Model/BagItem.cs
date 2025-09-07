using System;

namespace Game.Bag.Model
{
    /// <summary>
    /// 表示背包中的一个物品信息。
    /// Model 层纯数据模型。
    /// </summary>
    [Serializable]
    public class BagItem
    {
        public int index;
        public string itemId;
        public int quantity;
    }
}
