using System;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 默认状态
    /// </summary>
    public class Spell3 : BaseState
    {
        #region Constructor

        public Spell3(StateMachine stateMachine, LOLGameObject LOLGameObject)
            : base(stateMachine, LOLGameObject)
        {
            this.animator = LOLGameObject.animator;
        }

        #endregion

        #region State Implementation

        public override void OnEnter()
        {
            base.OnEnter();
            float dur = stateMachine.CurrentTransitionDuration;
            animator.CrossFade("Spell3", dur, -1, 0f);
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
