using UnityEngine;
using NewActionSystem.Core;

namespace NewActionSystem.Commands
{
    /// <summary>
    /// 攻击命令：控制玩家攻击指定目标
    /// </summary>
    public class AttackCommand : ICommand
    {
        #region Private Fields
        
        /// <summary>
        /// 玩家控制器引用
        /// </summary>
        private Player.PlayerController playerController;
        
        /// <summary>
        /// 攻击目标
        /// </summary>
        private Transform attackTarget;
        
        #endregion
        
        #region Constructor
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="playerController">玩家控制器</param>
        /// <param name="attackTarget">攻击目标</param>
        public AttackCommand(Player.PlayerController playerController, Transform attackTarget)
        {
            this.playerController = playerController;
            this.attackTarget = attackTarget;
        }
        
        #endregion
        
        #region ICommand Implementation
        
        public void Execute()
        {
            if (playerController == null) return;
            
            // 设置攻击目标
            playerController.SetAttackTarget(attackTarget);
            
            Debug.Log($"执行攻击命令，目标: {(attackTarget != null ? attackTarget.name : "无目标")}");
        }
        
        public void Undo()
        {
            if (playerController == null) return;
            
            // 取消攻击目标
            playerController.SetAttackTarget(null);
            
            Debug.Log("撤销攻击命令");
        }
        
        public bool CanExecute()
        {
            // 检查玩家控制器是否存在
            return playerController != null;
        }
        
        #endregion
    }
}
