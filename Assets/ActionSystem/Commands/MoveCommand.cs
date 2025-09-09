using UnityEngine;
using NewActionSystem.Core;

namespace NewActionSystem.Commands
{
    /// <summary>
    /// 移动命令：控制玩家移动到指定位置
    /// </summary>
    public class MoveCommand : ICommand
    {
        #region Private Fields
        
        /// <summary>
        /// 玩家控制器引用
        /// </summary>
        private Player.PlayerController playerController;
        
        /// <summary>
        /// 目标位置
        /// </summary>
        private Vector3 targetPosition;
        
        /// <summary>
        /// 移动前的位置（用于撤销）
        /// </summary>
        private Vector3 previousPosition;
        
        #endregion
        
        #region Constructor
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="playerController">玩家控制器</param>
        /// <param name="targetPosition">目标位置</param>
        public MoveCommand(Player.PlayerController playerController, Vector3 targetPosition)
        {
            this.playerController = playerController;
            this.targetPosition = targetPosition;
        }
        
        #endregion
        
        #region ICommand Implementation
        
        public void Execute()
        {
            if (playerController == null) return;
            
            // 记录当前位置用于撤销
            previousPosition = playerController.transform.position;
            
            // 设置目标位置
            playerController.SetMoveTarget(targetPosition);
            
            Debug.Log($"执行移动命令，目标位置: {targetPosition}");
        }
        
        public void Undo()
        {
            if (playerController == null) return;
            
            // 移动回原来的位置
            playerController.SetMoveTarget(previousPosition);
            
            Debug.Log($"撤销移动命令，返回位置: {previousPosition}");
        }
        
        public bool CanExecute()
        {
            // 检查玩家控制器是否存在
            return playerController != null;
        }
        
        #endregion
    }
}
