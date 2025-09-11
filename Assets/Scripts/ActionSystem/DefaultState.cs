using System;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 默认状态
    /// </summary>
    public class DefaultState : BaseState
    {
        #region Constructor

        public DefaultState(StateMachine stateMachine, LOLGameObject LOLGameObject) 
            : base(stateMachine, LOLGameObject)
        {
        }

        #endregion

        #region State Implementation

        public override void OnEnter()
        {
            base.OnEnter();
            LOLGameObject.Animator.Play("Default");
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
            return false;
        }
        
        #endregion
    }
}
