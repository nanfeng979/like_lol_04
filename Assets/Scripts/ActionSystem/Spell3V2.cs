using System;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 默认状态
    /// </summary>
    public class Spell3V2 : BaseStateV2
    {
        #region Constructor

        public Spell3V2(StateMachineV2 stateMachine, LOLGameObject LOLGameObject)
            : base(stateMachine, LOLGameObject)
        {
        }

        #endregion

        #region State Implementation

        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            float dur = stateMachine.CurrentTransitionDuration;
            animator.CrossFade("Spell3", dur, -1, 0f);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            selfLOLGameObject.HandleMoveToPosition();
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
