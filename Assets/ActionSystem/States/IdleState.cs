using System;
using UnityEngine;
using NewActionSystem.Core;

namespace NewActionSystem.States
{
    /// <summary>
    /// 待机状态：玩家站立不动的状态
    /// </summary>
    public class IdleState : BaseState
    {
        #region Constructor
        
        public IdleState(StateMachine stateMachine, Player.PlayerController playerController) 
            : base(stateMachine, playerController)
        {
        }
        
        #endregion
        
        #region State Implementation
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            // 播放待机动画
            playerController.PlayAnimation("Idle");
            
            // 停止移动
            playerController.StopMovement();
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            
            // 检查输入，如果有移动输入则切换到移动状态
            if (playerController.HasMoveInput())
            {
                TransitionToState<MoveState>();
                return;
            }
            
            // 检查攻击输入
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
            // 待机状态可以切换到移动和攻击状态
            return targetState == typeof(MoveState) || 
                   targetState == typeof(AttackState);
        }
        
        #endregion
    }
}
