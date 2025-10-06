using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LikeLoL04
{
    /// <summary>
    /// 商店分类视图
    /// </summary>
    public class StoreCategoryView : MonoBehaviour
    {
        private Text m_titleText => transform.Find("Title").GetComponent<Text>();

        public void SetTitle(string title)
        {
            m_titleText.text = title;
        }

        private RectTransform m_itemRoot => transform.Find("ItemRoot").GetComponent<RectTransform>();

        public void AddItems(List<StoreItemData> items)
        {
            m_itemRoot.GetChild(0).gameObject.SetActive(false);

            foreach (var item in items)
            {
                GameObject itemGO = Instantiate(m_itemRoot.GetChild(0).gameObject);
                itemGO.SetActive(true);
                itemGO.transform.SetParent(m_itemRoot);
                itemGO.name = item.itemName;
                StoreItemView itemView = itemGO.GetComponent<StoreItemView>();
                itemView.OnLeftClickAction = () =>
                {
                    StoreCraftingController.Instance.SetBeSelectStoreItem(item);
                };
                itemView.SetItem(item);
            }
        }
    }
}
