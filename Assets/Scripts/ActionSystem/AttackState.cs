using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 移动状态
    /// </summary>
    public class AttackState : BaseState
    {
        // 进入攻击时的短暂朝向过渡
        private float faceRotationDuration = 0.05f;
        private float faceRotationElapsed = 0f;
        private Quaternion faceInitialRot;
        private Quaternion faceTargetRot;

        #region Constructor

        public AttackState(StateMachine stateMachine, LOLGameObject selfLOLGameObject)
            : base(stateMachine, selfLOLGameObject)
        {
            animationName = "Attack";
            onAnimationEnd = (sm) =>
            {
                stateMachine.TransitionTo<Attack2State>();
            };
        }

        #endregion

        #region State Implementation

        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            // 进入攻击状态时设置一个短暂的朝向过渡（水平朝向）
            SetupFaceTargetTween();

            float dur = stateMachine.CurrentTransitionDuration;
            animator.CrossFade(animationName, dur, -1, 0f);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            // 推进进入攻击时的短暂朝向过渡
            if (faceRotationElapsed < faceRotationDuration)
            {
                faceRotationElapsed += Time.deltaTime;
                float t = Mathf.Clamp01(faceRotationElapsed / faceRotationDuration);
                selfLOLGameObject.transform.rotation = Quaternion.Slerp(faceInitialRot, faceTargetRot, t);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        #endregion

        // 准备进入攻击时的朝向过渡参数（仅水平旋转）
        private void SetupFaceTargetTween()
        {
            Vector3? targetPos = null;
            if (selfLOLGameObject.Target != null)
            {
                targetPos = selfLOLGameObject.Target.transform.position;
            }
            else if (selfLOLGameObject.TargetPosition != Vector3.zero)
            {
                targetPos = selfLOLGameObject.TargetPosition;
            }

            faceRotationElapsed = 0f;
            faceRotationDuration = Mathf.Max(0.01f, selfLOLGameObject.RotationDuration);
            faceInitialRot = selfLOLGameObject.transform.rotation;

            if (!targetPos.HasValue)
            {
                faceTargetRot = faceInitialRot;
                return;
            }

            Vector3 currentPos = selfLOLGameObject.transform.position;
            Vector3 lookDir = (targetPos.Value - currentPos);
            lookDir.y = 0f; // 仅水平朝向

            if (lookDir.sqrMagnitude < 0.0001f)
            {
                faceTargetRot = faceInitialRot;
                return;
            }

            faceTargetRot = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
        }
        
    }

}
