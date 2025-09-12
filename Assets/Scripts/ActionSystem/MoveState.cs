using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 移动状态
    /// </summary>
    public class MoveState : BaseState
    {
        // 移动速度（单位：单位/秒）
        private float moveSpeed = 3.5f;

        // 到达目标的停止距离阈值
        private float stopDistance = 0.5f;

        #region Constructor

        public MoveState(StateMachine stateMachine, LOLGameObject LOLGameObject)
            : base(stateMachine, LOLGameObject)
        {
            this.moveSpeed = LOLGameObject.MoveSpeed;
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
                LOLGameObject.Animator.CrossFade("Move", animationTransitionDuration, -1, 0f);
            }
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();

            // 如果没有目标，直接返回到默认状态
            var target = LOLGameObject.Target;
            if (target == null && LOLGameObject.TargetPosition == null)
            {
                stateMachine.TransitionTo<DefaultState>();
                return;
            }

            // 计算目标位置（以目标的 Transform 为准）
            Vector3 targetPos = target != null ? target.transform.position : LOLGameObject.TargetPosition;
            Vector3 currentPos = LOLGameObject.transform.position;

            // 计算距离
            float distance = Vector3.Distance(currentPos, targetPos);

            // 如果已经足够接近，切回默认状态
            if (distance <= stopDistance)
            {
                stateMachine.TransitionTo<DefaultState>();
                return;
            }

            // 移动朝向目标（平滑移动）
            Vector3 direction = (targetPos - currentPos).normalized;
            Vector3 move = direction * moveSpeed * Time.deltaTime;

            // 避免超越目标
            if (move.magnitude > distance)
            {
                move = direction * distance;
            }

            LOLGameObject.transform.position = currentPos + move;
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
