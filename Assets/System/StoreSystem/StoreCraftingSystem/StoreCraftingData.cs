
using System.Collections.Generic;

namespace LikeLoL04
{
    /// <summary>
    /// 商店物品合成数据
    /// </summary>
    [System.Serializable]
    public class StoreCraftingData
    {
        public string Name;
        public List<string> Parents = new List<string>();
        public List<string> Children = new List<string>();
    }
}
