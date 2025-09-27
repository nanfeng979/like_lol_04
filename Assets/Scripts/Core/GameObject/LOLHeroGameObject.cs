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

        protected override void Start()
        {
            base.Start();
            m_skill = new LOLGameObjectSkill();
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}
