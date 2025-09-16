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
            this.moveSpeed = LOLGameObject.MoveSpeed;
            this.rotationDuration = LOLGameObject.RotationDuration;
        }

        #endregion

        #region State Implementation

        public override void OnEnter()
        {
            base.OnEnter();
            if (LOLGameObject.Animator != null)
            {
                float dur = stateMachine.CurrentTransitionDuration;
                LOLGameObject.Animator.CrossFade("Move", dur, -1, 0f);
            }

            // 每次进入状态时重置旋转插值
            rotationElapsed = 0f;
            initialRotation = LOLGameObject.transform.rotation;

            // 初始化一个默认的 targetRotation（如果后面检测到目标会更新）
            targetRotation = initialRotation;
            lastRotationStepIndex = -1;
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

            // 若存在 Target 并且进入攻击范围，切换到 AttackState
            if (target != null)
            {
                float distToTarget = Vector3.Distance(LOLGameObject.transform.position, target.transform.position);
                if (distToTarget <= LOLGameObject.AttackRange)
                {
                    stateMachine.TransitionTo<AttackState>();
                    return;
                }
            }

            // 处理移动，若已到达则回到默认状态
            if (HandleMovement(targetPos))
            {
                stateMachine.TransitionTo<DefaultState>();
                return;
            }

            // 处理旋转
            HandleRotation(targetPos);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override bool CanTransitionTo(Type targetState)
        {
            return targetState != typeof(MoveState);
        }

        // 处理离散旋转，使前向 Z 轴朝向 targetPos
        private void HandleRotation(Vector3 targetPos)
        {
            // 计算新方向（只考虑水平平面可选：若需要忽略高度差可将 lookDir.y = 0）
            Vector3 lookDir = (targetPos - LOLGameObject.transform.position).normalized;
            if (lookDir.sqrMagnitude <= 0.0001f)
                return;

            Quaternion desired = Quaternion.LookRotation(lookDir, Vector3.up);

            // 判断是否需要重新开始一轮旋转（角度变化阈值）
            if (rotationElapsed == 0f || Quaternion.Angle(targetRotation, desired) > 5f)
            {
                initialRotation = LOLGameObject.transform.rotation;
                targetRotation = desired;
                rotationElapsed = 0f;
                lastRotationStepIndex = -1;
            }

            if (rotationElapsed < rotationDuration)
            {
                rotationElapsed += Time.deltaTime;
                float totalT = Mathf.Clamp01(rotationElapsed / rotationDuration);

                // 根据 totalT 计算当前应处于第几个离散步
                int currentStepIndex = Mathf.Clamp(Mathf.FloorToInt(totalT * rotationSteps), 0, rotationSteps);

                // 只有当步进前进时才更新旋转（离散化）
                if (currentStepIndex > lastRotationStepIndex)
                {
                    lastRotationStepIndex = currentStepIndex;
                    float stepT = (float)currentStepIndex / rotationSteps; // 量化后的插值系数
                    LOLGameObject.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, stepT);
                }

                // 到达最终步确保精确设置目标朝向
                if (currentStepIndex >= rotationSteps)
                {
                    LOLGameObject.transform.rotation = targetRotation;
                }
            }
            else
            {
                // 已完成旋转
                LOLGameObject.transform.rotation = targetRotation;
            }
        }

        // 处理移动，返回是否到达停止距离
        private bool HandleMovement(Vector3 targetPos)
        {
            Vector3 currentPos = LOLGameObject.transform.position;
            float distance = Vector3.Distance(currentPos, targetPos);
            if (distance <= stopDistance)
            {
                return true; // 已到达
            }

            Vector3 direction = (targetPos - currentPos).normalized;
            Vector3 move = direction * moveSpeed * Time.deltaTime;
            if (move.magnitude > distance)
            {
                move = direction * distance;
            }
            LOLGameObject.transform.position = currentPos + move;
            return false;
        }

        #endregion

    }

}
