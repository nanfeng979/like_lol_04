using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 移动状态
    /// </summary>
    public class Spell1_run : BaseState
    {
        #region Constructor

        public Spell1_run(StateMachine stateMachine, LOLGameObject LOLGameObject)
            : base(stateMachine, LOLGameObject)
        {
        }

        #endregion

        #region State Implementation

        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            float dur = stateMachine.CurrentTransitionDuration;
            selfLOLGameObject.animator.CrossFade("Spell1_run", dur, -1, 0f);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            // if (!GameManager.Instance.mainPlayer.mainPlayer.useQSkill)
            // {
            //     if (selfLOLGameObject.HandleMoveToPosition())
            //     {
            //         stateMachine.TransitionTo<DefaultState>();
            //     }
            //     else
            //     {
            //         stateMachine.TransitionTo<MoveState>();
            //     }

            //     return;
            // }

            LOLGameObject target = selfLOLGameObject.Target;
            // 若存在 Target 并且进入攻击范围，切换到 AttackState
            if (target != null)
            {
                if (selfLOLGameObject.IsTargetInAttackRange(target))
                {
                    stateMachine.TransitionTo<AttackState>();
                    return;
                }
            }

            // 计算目标位置（以目标的 Transform 为准）
            Vector3 targetPos = selfLOLGameObject.TargetPosition;

            // 处理旋转
            selfLOLGameObject.HandleRotation(targetPos);

            if (selfLOLGameObject.HandleMoveToPosition())
            {
                stateMachine.TransitionTo<DefaultState>();
                return;
            }
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
