using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreSystemModel : IMVC_Model
{
    private List<StoreCategoryModel> m_categories = new List<StoreCategoryModel>();
    public List<StoreCategoryModel> Categories { get { return m_categories; } set { m_categories = value; } }
}
