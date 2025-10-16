using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillLearnView : MVC_View
{
    public GameObject Speell1Object;
    public GameObject Speell2Object;
    public GameObject Speell3Object;
    public GameObject Speell4Object;

    public string m_skillLevelEnableAddress = "Assets/PlayerInfoSystem/UI/技能激活圆.png";

    public void OnUpgradeSkill(int skillIndex, int skillLevel)
    {
        GameObject skillObject = null;
        switch (skillIndex)
        {
            case 1:
                skillObject = Speell1Object;
                break;
            case 2:
                skillObject = Speell2Object;
                break;
            case 3:
                skillObject = Speell3Object;
                break;
            case 4:
                skillObject = Speell4Object;
                break;
            default:
                break;
        }

        OnUpgradeSkillAction(skillObject, skillLevel);
    }

    public void OnUpgradeSkillAction(GameObject skillObject, int skillLevel)
    {
        if (skillObject == null)
            return;

        if (skillLevel == 1)
        {
            Image SkillIconImage = skillObject.transform.Find("SkillIcon").GetComponent<Image>();
            SkillIconImage.color = Color.white;
            Image SkillBorderImage = skillObject.transform.Find("SkillBorder").GetComponent<Image>();
            SkillBorderImage.color = Color.white;
        }

        GameObject LevelObject = skillObject.transform.Find("Level").gameObject;
        GameObject skillLevelEnableObj = LevelObject.transform.GetChild(skillLevel - 1).gameObject;
        if (skillLevelEnableObj)
        {
            Image skillLevelEnableObjImage = skillLevelEnableObj.GetComponent<Image>();
            AddressablesUtils.LoadAsset<Sprite>(m_skillLevelEnableAddress, sprite =>
            {
                skillLevelEnableObjImage.sprite = sprite;
            });
        }
    }
}
