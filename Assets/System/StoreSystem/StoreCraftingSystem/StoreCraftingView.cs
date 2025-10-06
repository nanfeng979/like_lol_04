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

        [SerializeField]
        private RectTransform m_ChildRow1;

        [SerializeField]
        private RectTransform m_ChildRow2;

        [SerializeField]
        private RectTransform m_ChildRow3;

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

        public void MultiLayerUpdateChildren(List<StoreItemModel> layer1, List<StoreItemModel> layer2)
        {
            // 先全部隐藏子物体
            for (int i = 0; i < m_ChildRow1.childCount; i++)
                m_ChildRow1.GetChild(i).gameObject.SetActive(false);
            for (int i = 0; i < m_ChildRow2.childCount; i++)
                m_ChildRow2.GetChild(i).gameObject.SetActive(false);
            for (int i = 0; i < m_ChildRow3.childCount; i++)
                m_ChildRow3.GetChild(i).gameObject.SetActive(false);

            // 默认隐藏所有行
            m_ChildRow1.gameObject.SetActive(false);
            m_ChildRow2.gameObject.SetActive(false);
            m_ChildRow3.gameObject.SetActive(false);

            if ((layer1 == null || layer1.Count == 0) && (layer2 == null || layer2.Count == 0))
            {
                return;
            }

            int row1Count = m_ChildRow1.childCount;
            int row2Count = m_ChildRow2.childCount;
            int row3Count = m_ChildRow3.childCount;

            bool row1Active = false, row2Active = false, row3Active = false;

            int total = (layer1?.Count ?? 0) + (layer2?.Count ?? 0);
            for (int i = 0; i < total; i++)
            {
                RectTransform childItem = null;
                if (i < row1Count)
                {
                    childItem = m_ChildRow1.GetChild(i).GetComponent<RectTransform>();
                    row1Active = true;
                }
                else if (i < row1Count + row2Count)
                {
                    childItem = m_ChildRow2.GetChild(i - row1Count).GetComponent<RectTransform>();
                    row2Active = true;
                }
                else if (i < row1Count + row2Count + row3Count)
                {
                    childItem = m_ChildRow3.GetChild(i - row1Count - row2Count).GetComponent<RectTransform>();
                    row3Active = true;
                }
                else
                {
                    break;
                }

                childItem.gameObject.SetActive(true);
                StoreItemView childItemView = childItem.GetComponent<StoreItemView>();
                int index = i;
                childItemView.OnLeftClickAction = () =>
                {
                    StoreItemModel target;
                    if (index < (layer1?.Count ?? 0))
                    {
                        target = layer1[index];
                    }
                    else
                    {
                        int offset = index - (layer1?.Count ?? 0);
                        target = layer2[offset];
                    }
                    StoreCraftingController.Instance.SetBeSelectStoreItem(target);
                };
                if (index < (layer1?.Count ?? 0))
                {
                    childItemView.SetItem(layer1[index]);
                }
                else
                {
                    int offset = index - (layer1?.Count ?? 0);
                    childItemView.SetItem(layer2[offset]);
                }
            }

            // 只激活有内容的行
            if (row1Active) m_ChildRow1.gameObject.SetActive(true);
            if (row2Active) m_ChildRow2.gameObject.SetActive(true);
            if (row3Active) m_ChildRow3.gameObject.SetActive(true);
        }

        // 兼容旧调用（只传一层）
        public void UpdateChildren(List<StoreItemModel> onlyLayer1)
        {
            MultiLayerUpdateChildren(onlyLayer1, null);
        }
    }
}
