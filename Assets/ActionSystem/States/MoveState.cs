using System;
using UnityEngine;
using NewActionSystem.Core;

namespace NewActionSystem.States
{
    /// <summary>
    /// 移动状态：玩家移动时的状态
    /// </summary>
    public class MoveState : BaseState
    {
        #region Constructor
        
        public MoveState(StateMachine stateMachine, Player.PlayerController playerController) 
            : base(stateMachine, playerController)
        {
        }
        
        #endregion
        
        #region State Implementation
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            // 播放移动动画
            playerController.PlayAnimation("Move");
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            
            // 处理移动逻辑
            playerController.HandleMovement();
            
            // 检查是否停止移动
            if (!playerController.HasMoveInput())
            {
                TransitionToState<IdleState>();
                return;
            }
            
            // 检查攻击输入（移动中也可以攻击）
            if (playerController.HasAttackInput())
            {
                TransitionToState<AttackState>();
                return;
            }
        }
        
        public override void OnExit()
        {
            base.OnExit();
        }
        
        public override bool CanTransitionTo(Type targetState)
        {
            // 移动状态可以切换到待机和攻击状态
            return targetState == typeof(IdleState) || 
                   targetState == typeof(AttackState);
        }
        
        #endregion
    }
}
