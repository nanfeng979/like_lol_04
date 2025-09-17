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

        // 到达目标的停止距离阈值
        private float stopDistance = 0.5f;

        // 旋转总耗时（秒）
        private float rotationDuration = 0.3f;
        private float rotationElapsed = 0f;
        private Quaternion initialRotation;
        private Quaternion targetRotation;

        // 离散旋转步数（在一次旋转时长内进行多少次插值更新）
        private int rotationSteps = 6;
        private int lastRotationStepIndex = -1;

        #region Constructor

        public MoveState(StateMachine stateMachine, LOLGameObject LOLGameObject)
            : base(stateMachine, LOLGameObject)
        {
            this.rotationDuration = LOLGameObject.RotationDuration;
        }

        #endregion

        #region State Implementation

        public override void OnEnter()
        {
            base.OnEnter();
            if (selfLOLGameObject.animator != null)
            {
                float dur = stateMachine.CurrentTransitionDuration;
                selfLOLGameObject.animator.CrossFade("Move", dur, -1, 0f);
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            // 计算目标位置（以目标的 Transform 为准）
            Vector3 targetPos = selfLOLGameObject.TargetPosition;

            // 若存在 Target 并且进入攻击范围，切换到 AttackState
            if (selfLOLGameObject.Target != null)
            {
                float distToTarget = Vector3.Distance(selfLOLGameObject.transform.position, selfLOLGameObject.Target.transform.position);
                if (distToTarget <= selfLOLGameObject.AttackRange)
                {
                    stateMachine.TransitionTo<AttackState>();
                    return;
                }
            }

            // 处理旋转
            if (selfLOLGameObject.HandleMoveToPosition())
            {
                stateMachine.TransitionTo<DefaultState>();
                selfLOLGameObject.HandleRotation(targetPos);

                return;
            }

            selfLOLGameObject.HandleRotation(targetPos);

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override bool CanTransitionTo(Type targetState)
        {
            return targetState != typeof(MoveState);
        }

        #endregion

    }

}
