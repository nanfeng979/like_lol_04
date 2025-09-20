using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 状态机管理器V2，使用字符串标识符进行状态注册和切换
    /// </summary>
    public class StateMachineV2
    {
        #region Private Fields

        /// <summary>
        /// 当前活动状态
        /// </summary>
        private IState currentState;

        /// <summary>
        /// 当前状态的字符串标识符
        /// </summary>
        private string currentStateId;

        /// <summary>
        /// 状态字典，使用字符串作为键来快速查找状态实例
        /// </summary>
        private Dictionary<string, IState> stateDict = new Dictionary<string, IState>();

        /// <summary>
        /// 每个状态对的过渡时间配置 Key: (fromStateId, toStateId)
        /// </summary>
        private readonly Dictionary<Tuple<string, string>, float> transitionDurations = new Dictionary<Tuple<string, string>, float>();

        #endregion

        #region Public Properties

        /// <summary>
        /// 获取当前状态的字符串标识符
        /// </summary>
        public string CurrentStateId => currentStateId;

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
        /// <param name="stateId">状态的字符串标识符</param>
        /// <param name="state">状态实例</param>
        public void RegisterState(string stateId, IState state)
        {
            if (string.IsNullOrEmpty(stateId))
            {
                Debug.LogError("状态ID不能为空或null");
                return;
            }

            if (state == null)
            {
                Debug.LogError("状态实例不能为null");
                return;
            }

            if (stateDict.ContainsKey(stateId))
            {
                // Debug.LogWarning($"状态 {stateId} 已经注册，将覆盖原有状态");
            }
            stateDict[stateId] = state;
        }

        /// <summary>
        /// 注册状态到状态机（泛型版本，使用类型名作为默认ID）
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="state">状态实例</param>
        public void RegisterState<T>(T state) where T : IState
        {
            string stateId = typeof(T).Name;
            RegisterState(stateId, state);
        }

        /// <summary>
        /// 切换到指定状态
        /// </summary>
        /// <param name="stateId">目标状态的字符串标识符</param>
        /// <returns>是否切换成功</returns>
        public bool TransitionTo(string stateId)
        {
            if (string.IsNullOrEmpty(stateId))
            {
                Debug.LogError("状态ID不能为空或null");
                return false;
            }

            // 检查状态是否存在
            if (!stateDict.ContainsKey(stateId))
            {
                Debug.LogError($"状态 {stateId} 未注册到状态机中");
                return false;
            }

            // 检查当前状态是否允许切换
            if (currentState != null && !currentState.CanTransitionTo(stateDict[stateId].GetType()))
            {
                return false;
            }

            // 在切换前确定本次切换的过渡时间
            if (!string.IsNullOrEmpty(currentStateId))
            {
                var key = Tuple.Create(currentStateId, stateId);
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
            currentStateId = stateId;
            currentState = stateDict[stateId];
            currentState.OnEnter();

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
        /// 获取指定ID的状态实例
        /// </summary>
        /// <param name="stateId">状态的字符串标识符</param>
        /// <returns>状态实例</returns>
        public IState GetState(string stateId)
        {
            if (string.IsNullOrEmpty(stateId))
            {
                return null;
            }

            if (stateDict.ContainsKey(stateId))
            {
                return stateDict[stateId];
            }
            return null;
        }

        /// <summary>
        /// 获取指定类型的状态实例（泛型版本）
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <returns>状态实例</returns>
        public T GetState<T>() where T : IState
        {
            string stateId = typeof(T).Name;
            IState state = GetState(stateId);
            return state is T ? (T)state : default(T);
        }

        /// <summary>
        /// 设置从某状态切换到另一状态的动画过渡时间
        /// </summary>
        /// <param name="fromStateId">源状态ID</param>
        /// <param name="toStateId">目标状态ID</param>
        /// <param name="duration">过渡时间</param>
        public void SetTransitionDuration(string fromStateId, string toStateId, float duration)
        {
            if (string.IsNullOrEmpty(fromStateId) || string.IsNullOrEmpty(toStateId))
            {
                Debug.LogError("SetTransitionDuration: 状态ID不能为空");
                return;
            }
            transitionDurations[Tuple.Create(fromStateId, toStateId)] = Mathf.Max(0f, duration);
        }

        /// <summary>
        /// 检查状态是否已注册
        /// </summary>
        /// <param name="stateId">状态的字符串标识符</param>
        /// <returns>是否已注册</returns>
        public bool IsStateRegistered(string stateId)
        {
            return !string.IsNullOrEmpty(stateId) && stateDict.ContainsKey(stateId);
        }

        /// <summary>
        /// 获取所有已注册的状态ID
        /// </summary>
        /// <returns>状态ID集合</returns>
        public IEnumerable<string> GetRegisteredStateIds()
        {
            return stateDict.Keys;
        }

        /// <summary>
        /// 移除已注册的状态
        /// </summary>
        /// <param name="stateId">状态的字符串标识符</param>
        /// <returns>是否移除成功</returns>
        public bool UnregisterState(string stateId)
        {
            if (string.IsNullOrEmpty(stateId))
            {
                return false;
            }

            // 如果要移除的是当前状态，先清空当前状态
            if (currentStateId == stateId)
            {
                currentState?.OnExit();
                currentState = null;
                currentStateId = null;
            }

            return stateDict.Remove(stateId);
        }

        #endregion
    }
}