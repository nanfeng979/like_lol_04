using System.Collections.Generic;
using UnityEngine;

namespace NewActionSystem.Core
{
    /// <summary>
    /// 命令调用器，负责执行和管理命令
    /// </summary>
    public class CommandInvoker
    {
        #region Private Fields
        
        /// <summary>
        /// 命令历史记录（用于撤销功能）
        /// </summary>
        private Stack<ICommand> commandHistory = new Stack<ICommand>();
        
        /// <summary>
        /// 最大历史记录数量
        /// </summary>
        [SerializeField] private int maxHistoryCount = 50;
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">要执行的命令</param>
        /// <returns>是否执行成功</returns>
        public bool ExecuteCommand(ICommand command)
        {
            if (command == null)
            {
                Debug.LogWarning("尝试执行空命令");
                return false;
            }
            
            if (!command.CanExecute())
            {
                Debug.LogWarning($"命令 {command.GetType().Name} 不能执行");
                return false;
            }
            
            // 执行命令
            command.Execute();
            
            // 添加到历史记录
            commandHistory.Push(command);
            
            // 限制历史记录数量
            if (commandHistory.Count > maxHistoryCount)
            {
                var temp = new Stack<ICommand>();
                for (int i = 0; i < maxHistoryCount; i++)
                {
                    temp.Push(commandHistory.Pop());
                }
                commandHistory = temp;
            }
            
            Debug.Log($"执行命令: {command.GetType().Name}");
            return true;
        }
        
        /// <summary>
        /// 撤销上一个命令
        /// </summary>
        /// <returns>是否撤销成功</returns>
        public bool UndoLastCommand()
        {
            if (commandHistory.Count == 0)
            {
                Debug.LogWarning("没有可撤销的命令");
                return false;
            }
            
            ICommand lastCommand = commandHistory.Pop();
            lastCommand.Undo();
            
            Debug.Log($"撤销命令: {lastCommand.GetType().Name}");
            return true;
        }
        
        /// <summary>
        /// 清空命令历史
        /// </summary>
        public void ClearHistory()
        {
            commandHistory.Clear();
            Debug.Log("命令历史已清空");
        }
        
        /// <summary>
        /// 获取历史命令数量
        /// </summary>
        /// <returns>历史命令数量</returns>
        public int GetHistoryCount()
        {
            return commandHistory.Count;
        }
        
        #endregion
    }
}
