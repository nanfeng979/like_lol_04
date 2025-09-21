using System;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 默认状态V2，适配StateMachineV2
    /// </summary>
    public class DefaultStateV2 : BaseStateV2
    {
        #region Constructor

        public DefaultStateV2(StateMachineV2 stateMachine, LOLGameObject LOLGameObject)
            : base(stateMachine, LOLGameObject)
        {
        }

        #endregion

        #region State Implementation

        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            float dur = stateMachine.CurrentTransitionDuration;
            animator.CrossFade("Default", dur, -1, 0f);
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