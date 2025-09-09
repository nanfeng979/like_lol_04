using System;
using UnityEngine;
using NewActionSystem.Core;

namespace NewActionSystem.States
{
    /// <summary>
    /// 攻击状态：玩家攻击时的状态
    /// </summary>
    public class AttackState : BaseState
    {
        #region Private Fields
        
        /// <summary>
        /// 攻击持续时间
        /// </summary>
        private float attackDuration = 1.0f;
        
        /// <summary>
        /// 攻击开始时间
        /// </summary>
        private float attackStartTime;
        
        /// <summary>
        /// 是否已经造成伤害
        /// </summary>
        private bool hasDamageDealt;
        
        #endregion
        
        #region Constructor
        
        public AttackState(StateMachine stateMachine, Player.PlayerController playerController) 
            : base(stateMachine, playerController)
        {
        }
        
        #endregion
        
        #region State Implementation
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            // 播放攻击动画
            playerController.PlayAnimation("Attack");
            
            // 停止移动
            playerController.StopMovement();
            
            // 记录攻击开始时间
            attackStartTime = Time.time;
            hasDamageDealt = false;
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            
            float elapsedTime = Time.time - attackStartTime;
            
            // 在攻击动画的中段造成伤害
            if (!hasDamageDealt && elapsedTime >= attackDuration * 0.5f)
            {
                playerController.DealDamage();
                hasDamageDealt = true;
            }
            
            // 攻击动画结束后，根据输入决定下一个状态
            if (elapsedTime >= attackDuration)
            {
                // 检查是否有移动输入
                if (playerController.HasMoveInput())
                {
                    TransitionToState<MoveState>();
                }
                else
                {
                    TransitionToState<IdleState>();
                }
            }
        }
        
        public override void OnExit()
        {
            base.OnExit();
        }
        
        public override bool CanTransitionTo(Type targetState)
        {
            // 攻击状态只能在攻击结束后切换到其他状态
            // 这里简化处理，实际游戏中可能需要更复杂的逻辑
            return targetState == typeof(IdleState) || 
                   targetState == typeof(MoveState);
        }
        
        #endregion
    }
}
