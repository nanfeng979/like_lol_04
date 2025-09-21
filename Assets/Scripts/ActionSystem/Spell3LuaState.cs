using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 使用 Lua 脚本驱动的 Spell3 状态。
    /// 对应脚本：Resources/lua/spell3.txt
    /// </summary>
    public class Spell3LuaState : LuaStateV2
    {
        public Spell3LuaState(StateMachineV2 sm, LOLGameObject obj)
            : base(sm, obj, "spell3")
        {
        }
    }
}
