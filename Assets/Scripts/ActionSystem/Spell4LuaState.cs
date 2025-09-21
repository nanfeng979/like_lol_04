using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 使用 Lua 脚本驱动的 Spell4 状态。
    /// </summary>
    public class Spell4LuaState : LuaStateV2
    {
        public Spell4LuaState(StateMachineV2 sm, LOLGameObject obj)
            : base(sm, obj, "spell4")
        {
        }
    }
}
