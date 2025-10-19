using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LikeLoL04
{
    public class SkillLearnModel : IMVC_Model
    {
        private LOLGameObjectSkill m_mainPlayerSkill;

        public LOLGameObjectSkill MainPlayerSkill
        {
            get { return m_mainPlayerSkill; }
            set { m_mainPlayerSkill = value; }
        }

        public int UpgradeSkill(int levelType, int addLevel)
        {
            if (m_mainPlayerSkill == null)
            {
                Debug.LogError("MainPlayerSkill is null");
                return -1;
            }
            return m_mainPlayerSkill.UpgradeSkill(levelType, addLevel);
        }

        public int GetSkillLevel(int levelType)
        {
            if (m_mainPlayerSkill == null)
            {
                Debug.LogError("MainPlayerSkill is null");
                return -1;
            }
            return m_mainPlayerSkill.GetSkillLevel(levelType);
        }

        public int GetSkillMaxLevel(int levelType)
        {
            if (m_mainPlayerSkill == null)
            {
                Debug.LogError("MainPlayerSkill is null");
                return -1;
            }
            return m_mainPlayerSkill.GetSkillMaxLevel(levelType);
        }

        public bool CanUpgradeSkill => LOLMainPlayer.Instance.CanUpgradeSkill();

        public int PlayerLevel => LOLMainPlayer.Instance.Level;
    }
}
