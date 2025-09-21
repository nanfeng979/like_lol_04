using System;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 默认状态
    /// </summary>
    public class Attack2State : BaseState
    {
        #region Constructor

        public Attack2State(StateMachine stateMachine, LOLGameObject LOLGameObject)
            : base(stateMachine, LOLGameObject)
        {
            animationName = "Attack2";
            onAnimationEnd = (sm) =>
            {
                sm.TransitionTo<AttackState>();
            };
        }

        #endregion

        #region State Implementation

        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);

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
