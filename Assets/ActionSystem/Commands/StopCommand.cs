using UnityEngine;
using NewActionSystem.Core;

namespace NewActionSystem.Commands
{
    /// <summary>
    /// 停止命令：让玩家停止当前动作
    /// </summary>
    public class StopCommand : ICommand
    {
        #region Private Fields
        
        /// <summary>
        /// 玩家控制器引用
        /// </summary>
        private Player.PlayerController playerController;
        
        #endregion
        
        #region Constructor
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="playerController">玩家控制器</param>
        public StopCommand(Player.PlayerController playerController)
        {
            this.playerController = playerController;
        }
        
        #endregion
        
        #region ICommand Implementation
        
        public void Execute()
        {
            if (playerController == null) return;
            
            // 停止所有动作
            playerController.StopAllActions();
            
            Debug.Log("执行停止命令");
        }
        
        public void Undo()
        {
            // 停止命令通常不需要撤销功能
            Debug.Log("停止命令无法撤销");
        }
        
        public bool CanExecute()
        {
            // 检查玩家控制器是否存在
            return playerController != null;
        }
        
        #endregion
    }
}
