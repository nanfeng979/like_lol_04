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

        public void UpdateCurrent(StoreItemData current)
        {
            StoreItemView currentView = beSelectStoreItem.GetComponent<StoreItemView>();
            currentView.SetItem(current);
            currentView.ShowLightEffect(current.Cost <= LOLMainPlayer.Instance.Gold);
        }

        public void UpdateParents(List<StoreItemData> parents)
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
                GameObject parentItem;
                if (i < BeSelectItemParents.childCount)
                {
                    parentItem = BeSelectItemParents.GetChild(i).gameObject;
                    parentItem.SetActive(true);
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

        // 仅更新第一层 // 暂定固定数量
        public List<float> UpdateChildRow1(List<StoreItemData> layer1DataList)
        {
            for (int i = 0; i < m_ChildRow1.childCount; i++)
                m_ChildRow1.GetChild(i).gameObject.SetActive(false);
            m_ChildRow1.gameObject.SetActive(false);
            if (layer1DataList == null || layer1DataList.Count == 0)
            {
                return null;
            }
            m_ChildRow1.gameObject.SetActive(true);

            List<float> layer1ItemPosX = new List<float>();

            for (int i = 0; i < layer1DataList.Count; i++)
            {
                GameObject childItem = m_ChildRow1.GetChild(i).gameObject;
                childItem.SetActive(true);
                StoreItemView childItemView = childItem.GetComponent<StoreItemView>();
                int index = i; // 捕获当前的 i 值
                childItemView.SetItem(layer1DataList[index]);
            }

            AdaptChildRow1Positions(layer1DataList.Count);

            Canvas.ForceUpdateCanvases(); // 强制更新布局

            for (int i = 0; i < layer1DataList.Count; i++)
            {
                GameObject childItem = m_ChildRow1.GetChild(i).gameObject;
                layer1ItemPosX.Add(childItem.GetComponent<RectTransform>().anchoredPosition.x);
            }

            return layer1ItemPosX;
        }

        public void UpdateChildRow2(Dictionary<int, List<StoreItemData>> layer2DataDict, List<float> layer1ItemPosX)
        {
            for (int i = 0; i < m_ChildRow2.childCount; i++)
                m_ChildRow2.GetChild(i).gameObject.SetActive(false);
            m_ChildRow2.gameObject.SetActive(false);
            if (layer2DataDict == null || layer2DataDict.Count == 0 || layer1ItemPosX == null || layer1ItemPosX.Count == 0)
            {
                return;
            }
            m_ChildRow2.gameObject.SetActive(true);

            for (int i = 0; i < layer1ItemPosX.Count; i++)
            {
                if (!layer2DataDict.ContainsKey(i)) continue;

                var layer2CurrentColDataList = layer2DataDict[i];
                if (layer2CurrentColDataList == null || layer2CurrentColDataList.Count == 0) continue;

                RectTransform childRow2Col = m_ChildRow2.GetChild(i).GetComponent<RectTransform>();

                Canvas.ForceUpdateCanvases(); // 强制更新布局

                childRow2Col.gameObject.SetActive(true);
                childRow2Col.anchoredPosition = new Vector2(layer1ItemPosX[i], childRow2Col.anchoredPosition.y);

                for (int j = 0; j < childRow2Col.childCount; j++)
                {
                    childRow2Col.GetChild(j).gameObject.SetActive(false);
                }

                for (int j = 0; j < layer2CurrentColDataList.Count; j++)
                {
                    if (j < childRow2Col.childCount)
                    {
                        GameObject childItem = childRow2Col.GetChild(j).gameObject;
                        childItem.SetActive(true);
                        StoreItemView childItemView = childItem.GetComponent<StoreItemView>();
                        int index = j; // 捕获当前的 j 值
                        childItemView.SetItem(layer2CurrentColDataList[index]);
                    }
                }
            }
        }

        private void AdaptChildRow1Positions(int layer1DataListCount)
        {
            switch (layer1DataListCount)
            {
                case 1:
                    m_ChildRow1.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                    break;
                case 2:
                    const float gap2 = 67f;
                    m_ChildRow1.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-gap2, 0);
                    m_ChildRow1.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(gap2, 0);
                    break;
                case 3:
                    const float gap3 = 134f;
                    m_ChildRow1.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-gap3, 0);
                    m_ChildRow1.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                    m_ChildRow1.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(gap3, 0);
                    break;
                default:
                    break;
            }
        }

    }
}
