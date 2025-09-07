using System;
using System.Collections.Generic;

namespace LikeLoL04.EventSystem
{
    /// <summary>
    /// 全局事件总线（MVP）：支持 On/Off/Once（泛型参数），仅同步分发。
    /// - 使用事件类型 T 作为“频道键”，同一类型事件共享订阅列表。
    /// - 不支持队列/缓存/优先级/多线程（如需可后续扩展）。
    /// </summary>
    public static class EventBus
    {
        #region 字段
        // 事件类型 -> 订阅者列表（委托集合）
        private static readonly Dictionary<Type, List<Delegate>> s_Handlers = new Dictionary<Type, List<Delegate>>();

        // 事件类型 -> （原始委托 -> 包装委托），用于 once 反向查找以便 Off
        private static readonly Dictionary<Type, Dictionary<Delegate, Delegate>> s_OnceWrapperMap = new Dictionary<Type, Dictionary<Delegate, Delegate>>();

        // 事件名 -> 订阅者列表（Action<object[]>）
        private static readonly Dictionary<string, List<Action<object[]>>> s_NamedHandlers = new Dictionary<string, List<Action<object[]>>>(StringComparer.Ordinal);

        // 事件名 -> （原始委托 -> 包装委托），用于 once 的反查
        private static readonly Dictionary<string, Dictionary<Action<object[]>, Action<object[]>>> s_NamedOnceWrapperMap = new Dictionary<string, Dictionary<Action<object[]>, Action<object[]>>>(StringComparer.Ordinal);

        // 事件名 -> （原始委托 -> 包装委托），用于 on（带预设参数）的反查，便于 Off
        private static readonly Dictionary<string, Dictionary<Action<object[]>, Action<object[]>>> s_NamedOnWrapperMap = new Dictionary<string, Dictionary<Action<object[]>, Action<object[]>>>(StringComparer.Ordinal);

        private static bool s_Initialized;
        #endregion

        #region 生命周期
        /// <summary>
        /// 初始化（幂等）。当前仅用于显式标记初始化点，便于扩展（如日志、性能采样等）。
        /// </summary>
        public static void Initialize()
        {
            if (s_Initialized)
            {
                return;
            }

            s_Initialized = true;
        }

        /// <summary>
        /// 清空所有订阅（谨慎使用，通常仅用于测试或热重载场景）。
        /// </summary>
        public static void ClearAll()
        {
            s_Handlers.Clear();
            s_OnceWrapperMap.Clear();
            s_NamedHandlers.Clear();
            s_NamedOnceWrapperMap.Clear();
            s_NamedOnWrapperMap.Clear();
            s_Initialized = false;
        }
        #endregion

        #region 订阅/取消/一次性订阅
        /// <summary>
        /// 订阅某类型事件。
        /// </summary>
        /// <typeparam name="T">事件负载类型，作为事件频道键</typeparam>
        /// <param name="handler">事件回调</param>
        public static void On<T>(Action<T> handler)
        {
            if (handler == null)
            {
                return;
            }

            Type key = typeof(T);
            if (!s_Handlers.TryGetValue(key, out List<Delegate> list))
            {
                list = new List<Delegate>();
                s_Handlers[key] = list;
            }

            // 避免重复添加
            if (!list.Contains(handler))
            {
                list.Add(handler);
            }
        }

        /// <summary>
        /// 订阅一次，下次触发后自动取消。
        /// </summary>
        public static void Once<T>(Action<T> handler)
        {
            if (handler == null)
            {
                return;
            }

            Type key = typeof(T);

            Action<T> wrapper = null;
            wrapper = (payload) =>
            {
                // 先移除包装，避免用户回调内再次触发造成重复
                Off(wrapper);
                handler(payload);
            };

            if (!s_OnceWrapperMap.TryGetValue(key, out Dictionary<Delegate, Delegate> map))
            {
                map = new Dictionary<Delegate, Delegate>();
                s_OnceWrapperMap[key] = map;
            }

            // 记录原始->包装 映射，便于 Off(原始) 正确移除
            map[handler] = wrapper;
            On(wrapper);
        }

        /// <summary>
        /// 取消订阅。
        /// </summary>
        public static void Off<T>(Action<T> handler)
        {
            if (handler == null)
            {
                return;
            }

            Type key = typeof(T);

            // 若传入的是原始 once 委托，则取出包装委托进行移除
            if (s_OnceWrapperMap.TryGetValue(key, out Dictionary<Delegate, Delegate> map))
            {
                // 情况1：传入原始回调
                if (map.TryGetValue(handler, out Delegate wrapper))
                {
                    map.Remove(handler);
                    handler = (Action<T>)wrapper;
                }
                else
                {
                    // 情况2：可能传入的是包装委托，需要反查并移除映射
                    Delegate foundOriginal = null;
                    foreach (KeyValuePair<Delegate, Delegate> kv in map)
                    {
                        if (kv.Value == (Delegate)handler)
                        {
                            foundOriginal = kv.Key;
                            break;
                        }
                    }

                    if (foundOriginal != null)
                    {
                        map.Remove(foundOriginal);
                    }
                }

                if (map.Count == 0)
                {
                    s_OnceWrapperMap.Remove(key);
                }
            }

            if (s_Handlers.TryGetValue(key, out List<Delegate> list))
            {
                list.Remove(handler);
                if (list.Count == 0)
                {
                    s_Handlers.Remove(key);
                }
            }
        }
        #endregion

        #region 订阅/取消/一次性订阅（基于事件名）
        /// <summary>
        /// 按事件名订阅。第三个参数为预设参数列表，将在触发时与运行时参数合并后传给回调。
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="handler">回调（接收对象参数数组）</param>
        /// <param name="presetArgs">预设参数（可选）</param>
        public static void On(string eventName, Action<object[]> handler, params object[] presetArgs)
        {
            if (string.IsNullOrEmpty(eventName) || handler == null)
            {
                return;
            }

            Action<object[]> effective = handler;
            if (presetArgs != null && presetArgs.Length > 0)
            {
                effective = (runtimeArgs) =>
                {
                    object[] merged = ConcatArgs(presetArgs, runtimeArgs);
                    handler(merged);
                };

                if (!s_NamedOnWrapperMap.TryGetValue(eventName, out Dictionary<Action<object[]>, Action<object[]>> onMap))
                {
                    onMap = new Dictionary<Action<object[]>, Action<object[]>>();
                    s_NamedOnWrapperMap[eventName] = onMap;
                }
                onMap[handler] = effective;
            }

            if (!s_NamedHandlers.TryGetValue(eventName, out List<Action<object[]>> list))
            {
                list = new List<Action<object[]>>();
                s_NamedHandlers[eventName] = list;
            }

            if (!list.Contains(effective))
            {
                list.Add(effective);
            }
        }

        /// <summary>
        /// 按事件名一次性订阅。触发一次后自动取消。可提供预设参数列表。
        /// </summary>
        public static void Once(string eventName, Action<object[]> handler, params object[] presetArgs)
        {
            if (string.IsNullOrEmpty(eventName) || handler == null)
            {
                return;
            }

            Action<object[]> wrapped = null;
            if (presetArgs != null && presetArgs.Length > 0)
            {
                wrapped = (runtimeArgs) =>
                {
                    Off(eventName, wrapped);
                    object[] merged = ConcatArgs(presetArgs, runtimeArgs);
                    handler(merged);
                };
            }
            else
            {
                wrapped = (runtimeArgs) =>
                {
                    Off(eventName, wrapped);
                    handler(runtimeArgs);
                };
            }

            if (!s_NamedOnceWrapperMap.TryGetValue(eventName, out Dictionary<Action<object[]>, Action<object[]>> onceMap))
            {
                onceMap = new Dictionary<Action<object[]>, Action<object[]>>();
                s_NamedOnceWrapperMap[eventName] = onceMap;
            }
            onceMap[handler] = wrapped;

            On(eventName, wrapped);
        }

        /// <summary>
        /// 取消按事件名的订阅。
        /// </summary>
        public static void Off(string eventName, Action<object[]> handler)
        {
            if (string.IsNullOrEmpty(eventName) || handler == null)
            {
                return;
            }

            // 优先处理 once 的包装映射
            if (s_NamedOnceWrapperMap.TryGetValue(eventName, out Dictionary<Action<object[]>, Action<object[]>> onceMap))
            {
                if (onceMap.TryGetValue(handler, out Action<object[]> onceWrapped))
                {
                    onceMap.Remove(handler);
                    handler = onceWrapped;
                }
                else
                {
                    // 反查：传入的可能是包装委托
                    Action<object[]> foundOriginal = null;
                    foreach (KeyValuePair<Action<object[]>, Action<object[]>> kv in onceMap)
                    {
                        if (kv.Value == handler)
                        {
                            foundOriginal = kv.Key;
                            break;
                        }
                    }
                    if (foundOriginal != null)
                    {
                        onceMap.Remove(foundOriginal);
                    }
                }

                if (onceMap.Count == 0)
                {
                    s_NamedOnceWrapperMap.Remove(eventName);
                }
            }

            // 处理 on（带预设参数）的包装映射
            if (s_NamedOnWrapperMap.TryGetValue(eventName, out Dictionary<Action<object[]>, Action<object[]>> onMap))
            {
                if (onMap.TryGetValue(handler, out Action<object[]> onWrapped))
                {
                    onMap.Remove(handler);
                    handler = onWrapped;
                }
                else
                {
                    // 反查：传入的可能是包装委托
                    Action<object[]> foundOriginal = null;
                    foreach (KeyValuePair<Action<object[]>, Action<object[]>> kv in onMap)
                    {
                        if (kv.Value == handler)
                        {
                            foundOriginal = kv.Key;
                            break;
                        }
                    }
                    if (foundOriginal != null)
                    {
                        onMap.Remove(foundOriginal);
                    }
                }

                if (onMap.Count == 0)
                {
                    s_NamedOnWrapperMap.Remove(eventName);
                }
            }

            if (s_NamedHandlers.TryGetValue(eventName, out List<Action<object[]>> list))
            {
                list.Remove(handler);
                if (list.Count == 0)
                {
                    s_NamedHandlers.Remove(eventName);
                }
            }
        }
        #endregion

        #region 事件分发（同步）
        /// <summary>
        /// 分发事件（同步）。
        /// </summary>
        public static void Emit<T>(T payload)
        {
            Type key = typeof(T);
            if (!s_Handlers.TryGetValue(key, out List<Delegate> list) || list.Count == 0)
            {
                return;
            }

            // 使用快照，避免回调内部 Off/Once 修改集合导致迭代异常
            Delegate[] snapshot = list.ToArray();
            int length = snapshot.Length;
            for (int i = 0; i < length; i++)
            {
                Action<T> action = snapshot[i] as Action<T>;
                if (action != null)
                {
                    action(payload);
                }
            }
        }
        #endregion

        #region 事件分发（同步，基于事件名）
        /// <summary>
        /// 分发事件（同步，按事件名），参数为可变对象数组。
        /// </summary>
        public static void Emit(string eventName, params object[] args)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                return;
            }

            if (!s_NamedHandlers.TryGetValue(eventName, out List<Action<object[]>> list) || list.Count == 0)
            {
                return;
            }

            Action<object[]>[] snapshot = list.ToArray();
            int length = snapshot.Length;
            for (int i = 0; i < length; i++)
            {
                Action<object[]> action = snapshot[i];
                if (action != null)
                {
                    action(args);
                }
            }
        }
        #endregion

        #region 工具
        /// <summary>
        /// 合并两个参数数组（a 在前，b 在后）。
        /// </summary>
        private static object[] ConcatArgs(object[] a, object[] b)
        {
            int aLen = a == null ? 0 : a.Length;
            int bLen = b == null ? 0 : b.Length;
            if (aLen == 0 && bLen == 0)
            {
                return Array.Empty<object>();
            }
            object[] result = new object[aLen + bLen];
            if (aLen > 0)
            {
                Array.Copy(a, 0, result, 0, aLen);
            }
            if (bLen > 0)
            {
                Array.Copy(b, 0, result, aLen, bLen);
            }
            return result;
        }
        #endregion
    }
}
