using System;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 攻击2状态V2，适配StateMachineV2
    /// </summary>
    public class Attack2StateV2 : BaseStateV2
    {
        #region Constructor

        public Attack2StateV2(StateMachineV2 stateMachine, LOLGameObject LOLGameObject)
            : base(stateMachine, LOLGameObject)
        {
            animationName = "Attack2";
            onAnimationEnd = (sm) =>
            {
                sm.TransitionTo("AttackState");
            };
        }

        #endregion

        #region State Implementation

        public override void OnEnter()
        {
            base.OnEnter();

            float dur = stateMachine.CurrentTransitionDuration;
            animator.CrossFade(animationName, dur, -1, 0f);
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        
        public override void OnExit()
        {
            base.OnExit();
        }

        public override bool CanTransitionTo(Type targetState)
        {
            base.CanTransitionTo(targetState);
            return true;
        }
        
        #endregion
    }
}