using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemView : MonoBehaviour
{
    private Image m_icon => transform.Find("Icon").GetComponent<Image>();
    private Text m_priceText => transform.Find("Price").GetComponent<Text>();

    public void SetIcon(Sprite icon)
    {
        m_icon.sprite = icon;
    }

    public void SetPrice(int price)
    {
        m_priceText.text = price.ToString();
    }
}
