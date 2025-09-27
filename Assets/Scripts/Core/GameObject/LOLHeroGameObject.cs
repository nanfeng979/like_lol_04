using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 英雄单位的游戏对象组件
    /// </summary>
    public class LOLHeroGameObject : LOLGameObject
    {
        private LOLGameObjectSkill m_skill;
        public LOLGameObjectSkill Skill => m_skill;

        protected void OnEnable()
        {
            SubscribeKeyEvents();
        }

        protected void OnDisable()
        {
            UnsubscribeKeyEvents();
        }

        protected override void Start()
        {
            base.Start();
            m_skill = new LOLGameObjectSkill();
        }

        protected override void Update()
        {
            base.Update();
        }

        private void SubscribeKeyEvents()
        {
            if (LOLClientKeyEventManager.Instance != null)
            {
                LOLClientKeyEventManager.Instance.OnSkillKey -= HandleGlobalSkillKey; // 防重复
                LOLClientKeyEventManager.Instance.OnSkillKey += HandleGlobalSkillKey;
            }
        }

        private void UnsubscribeKeyEvents()
        {
            if (LOLClientKeyEventManager.Instance != null)
            {
                LOLClientKeyEventManager.Instance.OnSkillKey -= HandleGlobalSkillKey;
            }
        }

        private void HandleGlobalSkillKey(int slot)
        {
            // 通用技能学习判定：未学习直接忽略
            if (m_skill == null || !m_skill.IsSkillLearned(slot))
            {
                Debug.Log($"技能槽位 {slot} 未学习，忽略释放");
                return;
            }
            // 交给具体英雄实现
            OnSkillKeyCast(slot);
        }

        /// <summary>
        /// 子类重写此方法实现技能释放逻辑（本层已做是否学习判断）
        /// </summary>
        /// <param name="slot">技能槽位 1~4</param>
        protected virtual void OnSkillKeyCast(int slot) { }
    }
}
