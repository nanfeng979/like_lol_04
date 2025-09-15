using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 状态机管理器，负责状态的切换和管理
    /// </summary>
    public class StateMachine
    {
        #region Private Fields

        /// <summary>
        /// 当前活动状态
        /// </summary>
        private IState currentState;

        /// <summary>
        /// 状态字典，用于快速查找状态实例
        /// </summary>
        private Dictionary<Type, IState> stateDict = new Dictionary<Type, IState>();

        /// <summary>
        /// 每个状态对的过渡时间配置 Key: (fromStateType, toStateType)
        /// </summary>
        private readonly Dictionary<Tuple<Type, Type>, float> transitionDurations = new Dictionary<Tuple<Type, Type>, float>();

        #endregion

        #region Public Properties

        /// <summary>
        /// 获取当前状态类型
        /// </summary>
        public Type CurrentStateType => currentState?.GetType();

        /// <summary>
        /// 获取当前状态实例
        /// </summary>
        public IState CurrentState => currentState;

        /// <summary>
        /// 最近一次状态切换的过渡时间（供目标状态在 OnEnter 使用）
        /// </summary>
        public float CurrentTransitionDuration { get; private set; } = 0.25f;

        /// <summary>
        /// 默认过渡时间（当没有特定配置时使用）
        /// </summary>
        public float DefaultTransitionDuration { get; set; } = 0.25f;

        #endregion

        #region Public Methods

        /// <summary>
        /// 注册状态到状态机
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="state">状态实例</param>
        public void RegisterState<T>(T state) where T : IState
        {
            Type stateType = typeof(T);
            if (stateDict.ContainsKey(stateType))
            {
                Debug.LogWarning($"状态 {stateType.Name} 已经注册，将覆盖原有状态");
            }
            stateDict[stateType] = state;
        }

        /// <summary>
        /// 切换到指定状态
        /// </summary>
        /// <typeparam name="T">目标状态类型</typeparam>
        /// <returns>是否切换成功</returns>
        public bool TransitionTo<T>() where T : IState
        {
            return TransitionTo(typeof(T));
        }

        /// <summary>
        /// 切换到指定状态
        /// </summary>
        /// <param name="stateType">目标状态类型</param>
        /// <returns>是否切换成功</returns>
        public bool TransitionTo(Type stateType)
        {
            // 检查状态是否存在
            if (!stateDict.ContainsKey(stateType))
            {
                Debug.LogError($"状态 {stateType.Name} 未注册到状态机中");
                return false;
            }

            // 检查当前状态是否允许切换
            if (currentState != null && !currentState.CanTransitionTo(stateType))
            {
                Debug.LogWarning($"当前状态 {currentState.GetType().Name} 不允许切换到 {stateType.Name}");
                return false;
            }

            // 在切换前确定本次切换的过渡时间
            Type fromType = currentState?.GetType();
            if (fromType != null)
            {
                var key = Tuple.Create(fromType, stateType);
                if (transitionDurations.TryGetValue(key, out float dur))
                {
                    CurrentTransitionDuration = Mathf.Max(0f, dur);
                }
                else
                {
                    CurrentTransitionDuration = DefaultTransitionDuration;
                }
            }
            else
            {
                CurrentTransitionDuration = DefaultTransitionDuration;
            }

            // 退出当前状态
            currentState?.OnExit();

            // 切换到新状态
            currentState = stateDict[stateType];
            currentState.OnEnter();

            Debug.Log($"状态切换: {stateType.Name}");
            return true;
        }

        /// <summary>
        /// 更新当前状态
        /// </summary>
        public void Update()
        {
            currentState?.OnUpdate();
        }

        /// <summary>
        /// 获取指定类型的状态实例
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <returns>状态实例</returns>
        public T GetState<T>() where T : IState
        {
            Type stateType = typeof(T);
            if (stateDict.ContainsKey(stateType))
            {
                return (T)stateDict[stateType];
            }
            return default(T);
        }

        /// <summary>
        /// 设置从某状态切换到另一状态的动画过渡时间
        /// </summary>
        public void SetTransitionDuration(Type fromState, Type toState, float duration)
        {
            if (fromState == null || toState == null)
            {
                Debug.LogError("SetTransitionDuration: 参数不能为空");
                return;
            }
            transitionDurations[Tuple.Create(fromState, toState)] = Mathf.Max(0f, duration);
        }

        /// <summary>
        /// 设置从某状态切换到另一状态的动画过渡时间（泛型）
        /// </summary>
        public void SetTransitionDuration<TFrom, TTo>(float duration) where TFrom : IState where TTo : IState
        {
            SetTransitionDuration(typeof(TFrom), typeof(TTo), duration);
        }

        #endregion
    }

}
