using System;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 状态基类，提供状态的通用实现
    /// </summary>
    public abstract class BaseState : IState
    {
        #region Protected Fields

        /// <summary>
        /// 状态所属的状态机
        /// </summary>
        protected StateMachine stateMachine;

        /// <summary>
        /// 游戏对象引用
        /// </summary>
        protected LOLGameObject selfLOLGameObject;

        #endregion

        #region Constructor

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stateMachine">状态机</param>
        /// <param name="LOLGameObject">游戏对象</param>
        public BaseState(StateMachine stateMachine, LOLGameObject LOLGameObject)
        {
            this.stateMachine = stateMachine;
            this.selfLOLGameObject = LOLGameObject;
            this.animator = LOLGameObject.animator;
        }

        #endregion

        #region IState Implementation

        /// <summary>
        /// 进入状态
        /// </summary>
        public virtual void OnEnter()
        {
            // Debug.Log($"进入状态: {GetType().Name}");
        }

        /// <summary>
        /// 状态更新
        /// </summary>
        public virtual void OnUpdate()
        {
            // 子类实现具体的更新逻辑
        }

        /// <summary>
        /// 退出状态
        /// </summary>
        public virtual void OnExit()
        {
            // Debug.Log($"退出状态: {GetType().Name}");
        }

        /// <summary>
        /// 检查是否可以切换到目标状态
        /// </summary>
        /// <param name="targetState">目标状态类型</param>
        /// <returns>是否可以切换</returns>
        public virtual bool CanTransitionTo(Type targetState)
        {
            // 默认情况下允许切换到任何状态，子类可以重写此方法来限制状态切换
            return true;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// 切换到指定状态的辅助方法
        /// </summary>
        /// <typeparam name="T">目标状态类型</typeparam>
        protected void TransitionToState<T>() where T : IState
        {
            stateMachine.TransitionTo<T>();
        }

        #endregion

        protected Animator animator;
    }
}
