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
            // 使用 CrossFade 进行平滑过渡（"Move" 为动画状态名，需要与 Animator 控制器一致）
            if (LOLGameObject.Animator != null)
            {
                // 第三个参数 layer = -1 表示所有层，第四个参数 normalizedTime = 0 表示从头开始
                LOLGameObject.Animator.CrossFade("Default", animationTransitionDuration, -1, 0f);
            }
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
