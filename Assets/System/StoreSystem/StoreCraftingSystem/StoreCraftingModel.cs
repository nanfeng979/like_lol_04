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

        private Dictionary<string, StoreCraftingData> m_craftingDict = new Dictionary<string, StoreCraftingData>();

        private void ListToDict()
        {
            m_craftingDict.Clear();
            foreach (var crafting in CraftingTable)
            {
                if (!m_craftingDict.ContainsKey(crafting.Name))
                {
                    m_craftingDict[crafting.Name] = crafting;
                }
            }
        }

        public StoreCraftingData GetCraftingDataByName(string name)
        {
            if (m_craftingDict.TryGetValue(name, out StoreCraftingData data))
            {
                return data;
            }
            return null;
        }
    }
}
