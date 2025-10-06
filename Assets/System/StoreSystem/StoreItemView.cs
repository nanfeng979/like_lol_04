using System;
using System.Collections;
using System.Collections.Generic;
using LikeLoL04.EventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace LikeLoL04
{
    /// <summary>
    /// 商店物品视图组件
    /// 负责：显示图标和价格，处理点击事件
    /// </summary>
    public class StoreItemView : MonoBehaviour, ILeftClick
    {
        private Image m_icon => transform.Find("Icon").GetComponent<Image>();
        private Text m_priceText => transform.Find("Price").GetComponent<Text>();

        public Action OnLeftClickAction { get; set; }

        public void SetItem(StoreItemModel storeItemModel)
        {
            SetIconByAddress(storeItemModel);
            SetPrice(storeItemModel.price);
        }

        public void SetIconByAddress(StoreItemModel storeItemModel)
        {
            string iconAddress = storeItemModel.iconAddress + "/" + storeItemModel.itemName + ".png";
            AddressablesUtils.LoadAsset<Sprite>(iconAddress, sprite =>
            {
                m_icon.sprite = sprite;
            });
        }

        public void SetIcon(Sprite icon)
        {
            m_icon.sprite = icon;
        }

        public void SetPrice(int price)
        {
            m_priceText.text = price.ToString();
        }

        public void leftClickExecute()
        {
            OnLeftClickAction?.Invoke();
        }
    }
}
