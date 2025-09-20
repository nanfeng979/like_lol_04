using System;
using System.Collections.Generic;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 管理 LOLGameObject 上的 Buff 的组件式管理器
    /// 负责：添加/移除/更新/查询，处理过期与回调。
    /// </summary>
    public class BuffManager
    {
        private readonly LOLGameObject owner;
        private readonly List<Buff> buffs = new List<Buff>();
        private readonly Dictionary<Type, Buff> typeIndex = new Dictionary<Type, Buff>();

        /// <summary>
        /// （可选）是否允许同类型多实例；默认 false（替换旧的）
        /// </summary>
        public bool AllowDuplicateType = false;

        public IReadOnlyList<Buff> Buffs => buffs;

        public BuffManager(LOLGameObject owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// 添加一个 Buff。若不允许同类重复且已存在，则先移除旧的再加新的。
        /// </summary>
        public T Add<T>(T buff) where T : Buff
        {
            if (buff == null)
            {
                Debug.LogError("BuffManager.Add: buff 不能为 null");
                return null;
            }

            Type t = buff.GetType();
            if (!AllowDuplicateType && typeIndex.TryGetValue(t, out Buff existing))
            {
                // 先移除旧的
                Remove(existing);
            }

            buffs.Add(buff);
            if (!AllowDuplicateType)
            {
                typeIndex[t] = buff;
            }

            try
            {
                buff.OnApply();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Buff {t.Name} OnApply 异常: {ex}");
            }

            return buff;
        }

        /// <summary>
        /// 通过类型创建并添加（需要有无参或 LOLGameObject 构造函数）。
        /// </summary>
        public T Add<T>() where T : Buff
        {
            Buff instance = null;
            try
            {
                // 优先找 (LOLGameObject) 构造
                var ctor = typeof(T).GetConstructor(new Type[] { typeof(LOLGameObject) });
                if (ctor != null)
                {
                    instance = (Buff)ctor.Invoke(new object[] { owner });
                }
                else
                {
                    // 尝试无参构造
                    ctor = typeof(T).GetConstructor(Type.EmptyTypes);
                    if (ctor != null)
                    {
                        instance = (Buff)ctor.Invoke(null);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"BuffManager.Add<T>: 创建 {typeof(T).Name} 失败: {ex}");
                return null;
            }

            return Add((T)instance);
        }

        /// <summary>
        /// 移除指定 Buff 实例
        /// </summary>
        public bool Remove(Buff buff)
        {
            if (buff == null) return false;
            if (!buffs.Remove(buff)) return false;

            Type t = buff.GetType();
            if (!AllowDuplicateType && typeIndex.TryGetValue(t, out Buff existing) && existing == buff)
            {
                typeIndex.Remove(t);
            }

            try
            {
                buff.OnRemove();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Buff {t.Name} OnRemove 异常: {ex}");
            }
            return true;
        }

        /// <summary>
        /// 根据类型移除（仅首个或者索引缓存的）
        /// </summary>
        public bool Remove<T>() where T : Buff
        {
            Type t = typeof(T);
            if (!AllowDuplicateType && typeIndex.TryGetValue(t, out Buff existing))
            {
                return Remove(existing);
            }
            else
            {
                for (int i = 0; i < buffs.Count; i++)
                {
                    if (buffs[i].GetType() == t)
                    {
                        return Remove(buffs[i]);
                    }
                }
            }
            return false;
        }

        public bool Has<T>() where T : Buff
        {
            Type t = typeof(T);
            if (!AllowDuplicateType)
            {
                return typeIndex.ContainsKey(t);
            }
            else
            {
                for (int i = 0; i < buffs.Count; i++)
                {
                    if (buffs[i].GetType() == t) return true;
                }
                return false;
            }
        }

        public T Get<T>() where T : Buff
        {
            Type t = typeof(T);
            if (!AllowDuplicateType && typeIndex.TryGetValue(t, out Buff existing))
            {
                return (T)existing;
            }
            else
            {
                for (int i = 0; i < buffs.Count; i++)
                {
                    if (buffs[i].GetType() == t) return (T)buffs[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 每帧更新，由 LOLGameObject.Update 调用
        /// </summary>
        public void Update(float deltaTime)
        {
            for (int i = 0; i < buffs.Count; i++)
            {
                Buff buff = buffs[i];
                try
                {
                    buff.OnUpdate(deltaTime);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Buff {buff.GetType().Name} OnUpdate 异常: {ex}");
                }

                if (buff.IsExpired())
                {
                    // OnRemove 在 Remove 中调用，所以直接 Remove
                    Remove(buff);
                    i--; // 调整索引
                }
            }
        }

        public void Clear()
        {
            for (int i = buffs.Count - 1; i >= 0; i--)
            {
                Remove(buffs[i]);
            }
        }
    }
}
