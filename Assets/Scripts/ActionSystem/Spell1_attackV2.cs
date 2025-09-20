using System;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 默认状态
    /// </summary>
    public class Spell1_attackV2 : BaseStateV2
    {

        #region Constructor

        public Spell1_attackV2(StateMachineV2 stateMachine, LOLGameObject LOLGameObject)
            : base(stateMachine, LOLGameObject)
        {
            onAnimationEnd = (sm) =>
            {
                sm.TransitionTo(selfLOLGameObject.DefaultStateId);
            };
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

            // 处理旋转
            selfLOLGameObject.HandleRotation(selfLOLGameObject.TargetPosition);
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
