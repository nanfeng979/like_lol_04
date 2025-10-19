using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillItemView : MonoBehaviour
{
    private Image m_skillIcon;
    private Image m_skillBorder;
    private GameObject m_skillLevel;

    public Image SkillIconImage => m_skillIcon;
    public Image SkillBorderImage => m_skillBorder;
    public GameObject SkillLevelObject => m_skillLevel;

    void Start()
    {
        m_skillIcon = transform.Find("SkillIcon").GetComponent<Image>();
        m_skillBorder = transform.Find("SkillBorder").GetComponent<Image>();
        m_skillLevel = transform.Find("SkillLevel").gameObject;
    }

    public Image GetSkillLevelEnableObjImage(int skillLevel)
    {
        if (skillLevel > m_skillLevel.transform.childCount)
            return null;
        return m_skillLevel.transform.GetChild(skillLevel - 1).GetComponent<Image>();
    }

    public void SetSkillLevelEnableObjImageSprite(int skillLevel, Sprite sprite)
    {
        Image skillLevelEnableObjImage = GetSkillLevelEnableObjImage(skillLevel);
        if (skillLevelEnableObjImage != null)
        {
            skillLevelEnableObjImage.sprite = sprite;
        }
    }
}
