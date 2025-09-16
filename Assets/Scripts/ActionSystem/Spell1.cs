using System;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 默认状态
    /// </summary>
    public class Spell1 : BaseState
    {
        #region Constructor

        public Spell1(StateMachine stateMachine, LOLGameObject LOLGameObject)
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
            animator.CrossFade("Spell1", dur, -1, 0f);
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
