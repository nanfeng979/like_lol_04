namespace NewActionSystem.Core
{
    /// <summary>
    /// 命令接口，定义命令的基本行为
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        void Execute();
        
        /// <summary>
        /// 撤销命令（可选实现）
        /// </summary>
        void Undo();
        
        /// <summary>
        /// 命令是否可以执行
        /// </summary>
        /// <returns>是否可以执行</returns>
        bool CanExecute();
    }
}
