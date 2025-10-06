using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 商店合成系统视图
    /// </summary>
    public class StoreCraftingView : MVC_View
    {
        [SerializeField]
        public RectTransform beSelectStoreItem;

        [SerializeField]
        public RectTransform BeSelectItemParents;

        void Start()
        {
            foreach (RectTransform child in BeSelectItemParents)
            {
                child.gameObject.SetActive(false);
            }
        }

        public void UpdateParents(List<StoreItemModel> parents)
        {
            foreach (RectTransform child in BeSelectItemParents)
            {
                child.gameObject.SetActive(false);
            }

            if (parents == null || parents.Count == 0)
            {
                return;
            }

            for (int i = 0; i < parents.Count; i++)
            {
                RectTransform parentItem;
                if (i < BeSelectItemParents.childCount)
                {
                    parentItem = BeSelectItemParents.GetChild(i).GetComponent<RectTransform>();
                    parentItem.gameObject.SetActive(true);
                    StoreItemView parentItemView = parentItem.GetComponent<StoreItemView>();
                    int index = i; // 捕获当前的 i 值
                    parentItemView.OnLeftClickAction = () =>
                    {
                        StoreCraftingController.Instance.SetBeSelectStoreItem(parents[index]);
                    };
                    parentItemView.SetItem(parents[index]);
                }

            }
        }
    }
}
