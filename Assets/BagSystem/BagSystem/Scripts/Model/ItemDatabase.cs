using System;
using System.Collections.Generic;

namespace Game.Bag.Model
{
    /// <summary>
    /// 道具静态表（items.json）的容器与索引。
    /// </summary>
    [Serializable]
    public class ItemDatabase
    {
        public List<ItemData> items = new List<ItemData>();

        private Dictionary<string, ItemData> map = new Dictionary<string, ItemData>();

        public ItemDatabase BuildIndex()
        {
            map.Clear();
            if (items == null) return this;
            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item?.itemId)) continue;
                if (!map.ContainsKey(item.itemId))
                {
                    map.Add(item.itemId, item);
                }
            }

            return this;
        }

        public bool TryGet(string itemId, out ItemData data)
        {
            return map.TryGetValue(itemId, out data);
        }
    }
}
