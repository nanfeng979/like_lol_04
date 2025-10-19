using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillLearnView : MVC_View
{
    public List<SkillItemView> skillItemViews;

    public void OnUpgradeSkill(int skillIndex, int skillLevel, Sprite sprite)
    {
        SkillItemView sillItemView = skillItemViews[skillIndex - 1];
        OnUpgradeSkillAction(sillItemView, skillLevel, sprite);
    }

    public void OnUpgradeSkillAction(SkillItemView sillItemView, int skillLevel, Sprite sprite)
    {
        if (sillItemView == null)
            return;

        if (skillLevel == 1)
        {
            Image SkillIconImage = sillItemView.SkillIconImage;
            SkillIconImage.color = Color.white;
            Image SkillBorderImage = sillItemView.SkillBorderImage;
            SkillBorderImage.color = Color.white;
        }

        sillItemView.SetSkillLevelEnableObjImageSprite(skillLevel, sprite);
    }

}
