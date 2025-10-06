using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreSystemModel : IMVC_Model
{
    private List<StoreCategoryModel> m_categories = new List<StoreCategoryModel>();
    public List<StoreCategoryModel> Categories
    {
        get { return m_categories; }
        set
        {
            m_categories = value;
            ListToDict();
        }
    }

    private Dictionary<string, StoreItemData> m_itemDict = new Dictionary<string, StoreItemData>();

    private void ListToDict()
    {
        m_itemDict.Clear();
        foreach (var category in Categories)
        {
            foreach (var item in category.itemList)
            {
                if (!m_itemDict.ContainsKey(item.itemName))
                {
                    m_itemDict[item.itemName] = item;
                }
            }
        }
    }

    public StoreItemData GetItemByName(string name)
    {
        if (m_itemDict.TryGetValue(name, out StoreItemData item))
        {
            return item;
        }
        return null;
    }
}
