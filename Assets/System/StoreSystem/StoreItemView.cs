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

        private Image m_lightEffect => transform.Find("LightEffect")?.GetComponent<Image>();
        private string m_shineAddress = "Assets/System/StoreSystem/Texture/shine.png";
        private string m_noShineAddress = "Assets/System/StoreSystem/Texture/noShine.png";

        public Action OnLeftClickAction { get; set; }

        public void SetItem(StoreItemData storeItemData)
        {
            SetIconByAddress(storeItemData);
            SetPrice(storeItemData.Cost);
        }

        public void SetIconByAddress(StoreItemData storeItemData)
        {
            string iconAddress = "Assets/LOLItems/" + storeItemData.Name + ".png";
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

        public void ShowLightEffect(bool show)
        {
            if (m_lightEffect != null)
            {
                if (show)
                {
                    AddressablesUtils.LoadAsset<Sprite>(m_shineAddress, sprite =>
                    {
                        m_lightEffect.sprite = sprite;
                    });
                }
                else
                {
                    AddressablesUtils.LoadAsset<Sprite>(m_noShineAddress, sprite =>
                    {
                        m_lightEffect.sprite = sprite;
                    });
                }
            }
        }
    }
}
