using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 移动状态
    /// </summary>
    public class AttackState : BaseState
    {
        #region Constructor

        public AttackState(StateMachine stateMachine, LOLGameObject LOLGameObject)
            : base(stateMachine, LOLGameObject)
        {
            this.animator = LOLGameObject.Animator;
        }

        #endregion

        #region State Implementation

        public override void OnEnter()
        {
            base.OnEnter();

            float dur = stateMachine.CurrentTransitionDuration;
            animator.CrossFade("Attack", dur, -1, 0f);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            int layer = 0;
            // 动画过渡过程中先不判定结束，避免误判
            if (animator.IsInTransition(layer)) return;

            var stateInfo = animator.GetCurrentAnimatorStateInfo(layer);

            if (stateInfo.IsName("Attack"))
            {
                // 非循环攻击动画在播放结束（normalizedTime >= 1）后回到默认状态
                if (!stateInfo.loop && stateInfo.normalizedTime >= 1f)
                {
                    stateMachine.TransitionTo<DefaultState>();
                    return;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        #endregion

    }

}
