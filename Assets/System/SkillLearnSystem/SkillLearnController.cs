
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 商店系统控制器，负责管理商店的视图和数据模型。
    /// </summary>
    public class SkillLearnController
        : MVC_Controller<SkillLearnController, SkillLearnView, SkillLearnModel>
    {
        // protected override bool ViewDefaultActive => false;

        void Start()
        {
            MainPlayerSkillSystem.Instance.OnUpgradeSkill += OnUpgradeSkill;

            model.MainPlayerSkill = MainPlayerSkillSystem.Instance.mainPlayer.Skill;
        }

        void OnDestroy()
        {
            MainPlayerSkillSystem.Instance.OnUpgradeSkill -= OnUpgradeSkill;
        }

        private void OnUpgradeSkill(int skillType)
        {
            if (CanUpgradeSkill(skillType))
            {
                Debug.Log($"Upgrade skill {skillType} to level");
                int currentLevel = model.MainPlayerSkill.UpgradeSkill(skillType, 1);
                if (currentLevel != -1)
                {
                    LOLMainPlayer.Instance.UpgradeSkill();
                    Debug.Log($"Upgrade skill {skillType} to level {currentLevel}");
                    view?.OnUpgradeSkill(skillType, currentLevel);
                }
                else
                {
                    Debug.Log($"Upgrade skill {skillType} failed");
                }
            }
        }

        private bool CanUpgradeSkill(int skillType)
        {
            if (!LOLMainPlayer.Instance.CanUpgradeSkill())
            {
                Debug.LogWarning($"Player level is not high enough to upgrade skill {skillType}");
                return false;
            }

            int currentLevel = model.MainPlayerSkill.GetSkillLevel(skillType);
            int maxLevel = model.MainPlayerSkill.GetSkillMaxLevel(skillType);
            int playerLevel = LOLMainPlayer.Instance.Level;

            // 根据玩家等级获取该阶段的技能可升级上限
            int levelLimit = GetSkillLevelLimitByPlayerLevel(playerLevel);

            // 实际允许的最高等级为 “规则上限” 和 “技能自身上限” 之中较小的那个
            int finalMaxLevel = Mathf.Min(levelLimit, maxLevel);

            return currentLevel + 1 <= finalMaxLevel;
        }

        /// <summary>
        /// 根据玩家等级返回技能可升级的最大等级限制
        /// </summary>
        private int GetSkillLevelLimitByPlayerLevel(int playerLevel)
        {
            if (playerLevel >= 1 && playerLevel <= 2)
                return 1;
            else if (playerLevel >= 3 && playerLevel <= 4)
                return 2;
            else if (playerLevel >= 5 && playerLevel <= 6)
                return 3;
            else if (playerLevel >= 7 && playerLevel <= 8)
                return 4;
            else if (playerLevel >= 9 && playerLevel <= 10)
                return 5;

            // 超过10级时，默认返回最大上限（可根据实际需要调整）
            return int.MaxValue;
        }

    }
}
