using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 商店物品合成数据模型
    /// </summary>
    [System.Serializable]
    public class StoreCraftingModel : IMVC_Model
    {
        private List<StoreCraftingData> m_craftingTable = new List<StoreCraftingData>();

        public List<StoreCraftingData> CraftingTable
        {
            get => m_craftingTable;
            set
            {
                m_craftingTable = value;
                ListToDict();
            }
        }

        public Dictionary<string, StoreCraftingData> craftingDict = new Dictionary<string, StoreCraftingData>();

        public void ListToDict()
        {
            craftingDict.Clear();
            foreach (var crafting in CraftingTable)
            {
                if (!craftingDict.ContainsKey(crafting.Name))
                {
                    craftingDict[crafting.Name] = crafting;
                }
            }
        }

        public StoreCraftingData GetCraftingDataByName(string name)
        {
            if (craftingDict.TryGetValue(name, out StoreCraftingData data))
            {
                return data;
            }
            return null;
        }
    }
}
