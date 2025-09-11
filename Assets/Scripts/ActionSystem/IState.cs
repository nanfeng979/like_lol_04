using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 状态接口，定义状态的基本行为
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// 进入状态时调用
        /// </summary>
        void OnEnter();
        
        /// <summary>
        /// 状态更新，每帧调用
        /// </summary>
        void OnUpdate();
        
        /// <summary>
        /// 退出状态时调用
        /// </summary>
        void OnExit();
        
        /// <summary>
        /// 检查是否可以切换到目标状态
        /// </summary>
        /// <param name="targetState">目标状态类型</param>
        /// <returns>是否可以切换</returns>
        bool CanTransitionTo(System.Type targetState);
    }
}
