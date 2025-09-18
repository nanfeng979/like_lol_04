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

        protected string animationName = "";
        protected Action<StateMachine> onAnimationEnd = (stateMachine) =>
        {
            stateMachine.TransitionTo<DefaultState>();
        };

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
            int layer = 0;
            // 动画过渡过程中先不判定结束，避免误判
            if (animator.IsInTransition(layer)) return;

            var stateInfo = animator.GetCurrentAnimatorStateInfo(layer);

            if (animationName != "" && stateInfo.IsName(animationName))
            {
                // 非循环攻击动画在播放结束（normalizedTime >= 1）后回到默认状态
                if (!stateInfo.loop && stateInfo.normalizedTime >= 1f)
                {
                    onAnimationEnd?.Invoke(stateMachine);
                    return;
                }
            }
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
