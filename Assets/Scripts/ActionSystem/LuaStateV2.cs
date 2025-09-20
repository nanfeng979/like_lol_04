using System;
using UnityEngine;
using XLua;

namespace LikeLoL04
{
    /// <summary>
    /// 基于 XLua 的 Lua 驱动状态基类。
    /// 约定：脚本放在 Resources/lua 下，以 .txt 结尾（便于避免 Unity 将其识别为 TextAsset 以外类型）。
    /// Lua 文件需返回一个 table，包含可选的 OnEnter/OnUpdate/OnExit/CanTransitionTo 方法。
    /// </summary>
    public abstract class LuaStateV2 : BaseStateV2
    {
        protected string luaFileName; // 不含路径与后缀。例如：spell3 => Resources/lua/spell3.txt
        protected LuaEnv luaEnv;
        protected LuaTable luaTable;
        // 独立执行环境，避免多个实例互相覆盖 global self
        private LuaTable luaEnvTable;

        // 绑定的 Lua 函数委托
        private Action luaOnEnter;
        private Action luaOnExit;
        private Action<float> luaOnUpdate;
        private Func<string, bool> luaCanTransitionTo; // 参数是目标状态 ID（字符串），更贴近日志与扩展

        internal static float lastGCTime = 0;
        internal const float GCInterval = 1;
        protected bool luaLoaded = false;

        public LuaStateV2(StateMachineV2 stateMachine, LOLGameObject obj, string luaFileName)
            : base(stateMachine, obj)
        {
            this.luaFileName = luaFileName;
        }

        /// <summary>
        /// 获取或创建全局 LuaEnv（简易单例）。
        /// </summary>
        private LuaEnv GetEnv()
        {
            if (LuaEnvSingleton.Instance == null)
            {
                LuaEnvSingleton.Create();
            }
            return LuaEnvSingleton.Instance.Env;
        }

        private void EnsureLuaLoaded()
        {
            if (luaLoaded) return;
            luaEnv = GetEnv();
            try
            {
                // 读取 Resources/lua/<file>.txt
                string path = $"lua/{luaFileName}.lua"; // Resources 下的相对路径（不含后缀）
                TextAsset ta = Resources.Load<TextAsset>(path);
                if (ta == null)
                {
                    Debug.LogError($"LuaStateV2 加载失败: 未找到 {path}.txt，请确认放在 Resources/lua/ 目录");
                    return;
                }

                // 创建独立环境，设置元表 __index 指向全局，允许访问公共函数/库
                luaEnvTable = luaEnv.NewTable();
                LuaTable meta = luaEnv.NewTable();
                meta.Set("__index", luaEnv.Global);
                luaEnvTable.SetMetaTable(meta);
                meta.Dispose();

                luaEnv.DoString("print = function(...) CS.UnityEngine.Debug.Log(table.concat({...}, ' ')) end");

                // 先注入上下文（脚本内可直接使用 global self / stateId）
                luaEnvTable.Set("self", selfLOLGameObject);
                luaEnvTable.Set("stateId", luaFileName);

                // 在该环境中执行脚本，脚本需 return table（定义回调）
                object[] rets = luaEnv.DoString(ta.text, luaFileName, luaEnvTable);
                if (rets == null || rets.Length == 0 || !(rets[0] is LuaTable))
                {
                    Debug.LogError($"LuaStateV2: 脚本 {luaFileName} 未返回 table");
                    return;
                }
                luaTable = (LuaTable)rets[0];

                // 绑定函数（可选）
                luaOnEnter = luaTable.Get<Action>("OnEnter");
                luaOnExit = luaTable.Get<Action>("OnExit");
                luaOnUpdate = luaTable.Get<Action<float>>("OnUpdate");
                luaCanTransitionTo = luaTable.Get<Func<string, bool>>("CanTransitionTo");

                luaLoaded = true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"LuaStateV2 加载脚本 {luaFileName} 异常: {ex}");
            }
        }

        // 若需要在运行中更新 self（例如对象被替换），可调用此方法刷新上下文
        private void RefreshContext()
        {
            if (!luaLoaded || luaEnvTable == null) return;
            luaEnvTable.Set("self", selfLOLGameObject);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            EnsureLuaLoaded();
            RefreshContext();
            luaOnEnter?.Invoke();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!luaLoaded) return;
            luaOnUpdate?.Invoke(Time.deltaTime);

            if (Time.time - lastGCTime > GCInterval)
            {
                luaEnv.Tick();
                lastGCTime = Time.time;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (!luaLoaded) return;
            luaOnExit?.Invoke();
        }

        public override bool CanTransitionTo(Type targetState)
        {
            // 先让 Lua 机会通过 stateId 判断（更灵活）。传递注册状态 ID（这里采用目标类型名）。
            if (luaCanTransitionTo != null)
            {
                try
                {
                    bool ok = luaCanTransitionTo(targetState.Name);
                    if (!ok) return false;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"LuaStateV2 CanTransitionTo 调用异常: {ex}");
                }
            }
            return base.CanTransitionTo(targetState);
        }
    }

    /// <summary>
    /// 全局唯一 LuaEnv 持有者，简单单例。
    /// </summary>
    public class LuaEnvSingleton
    {
        public LuaEnv Env { get; private set; }
        public static LuaEnvSingleton Instance { get; private set; }

        private LuaEnvSingleton()
        {
            Env = new LuaEnv();
            // 可添加自定义 loader，例如：
            // Env.AddLoader(CustomLoader);
        }

        public static void Create()
        {
            if (Instance == null)
            {
                Instance = new LuaEnvSingleton();
            }
        }

        public static void Dispose()
        {
            if (Instance != null)
            {
                Instance.Env.Dispose();
                Instance = null;
            }
        }
    }
}
