using System;
using System.Collections.Generic;

namespace LikeLoL04.EventSystem
{
    public static class EventBus
    {
        private static readonly Dictionary<string, Action<object[]>> _eventTable = 
            new Dictionary<string, Action<object[]>>();

        // 订阅事件
        public static void On(string eventName, Action<object[]> callback)
        {
            if (!_eventTable.ContainsKey(eventName))
            {
                _eventTable[eventName] = null;
            }
            _eventTable[eventName] += callback;
        }

        // 取消订阅
        public static void Off(string eventName, Action<object[]> callback)
        {
            if (_eventTable.ContainsKey(eventName))
            {
                _eventTable[eventName] -= callback;
            }
        }

        // 取消所有订阅
        public static void Off(string eventName)
        {
            if (_eventTable.ContainsKey(eventName))
            {
                _eventTable[eventName] = null;
            }
        }

        // 发送事件
        public static void Emit(string eventName, params object[] args)
        {
            if (_eventTable.TryGetValue(eventName, out var action) && action != null)
            {
                action.Invoke(args);
            }
        }
    }
}