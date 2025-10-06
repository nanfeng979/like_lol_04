using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoreCategoryModel
{
    public string title;
    public List<StoreItemData> itemList = new List<StoreItemData>();
}
