using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreSystemModel : IMVC_Model
{
    private List<StoreItemData> m_storeItemDatas = new List<StoreItemData>();

    public List<StoreItemData> StoreItemDatas
    {
        get { return m_storeItemDatas; }
        set
        {
            m_storeItemDatas = value;
            m_categoryDict.Clear();
        }
    }

    private Dictionary<string, List<StoreItemData>> m_categoryDict = new Dictionary<string, List<StoreItemData>>();

    public Dictionary<string, List<StoreItemData>> CategoryDict
    {
        get
        {
            if (m_categoryDict.Count == 0)
            {
                foreach (StoreItemData item in m_storeItemDatas)
                {
                    if (!m_categoryDict.ContainsKey(item.Category))
                    {
                        m_categoryDict[item.Category] = new List<StoreItemData>();
                    }
                    m_categoryDict[item.Category].Add(item);
                }
            }
            return m_categoryDict;
        }
    }

    public StoreItemData GetItemByName(string itemName)
    {
        return m_storeItemDatas.Find(item => item.Name == itemName);
    }
}
