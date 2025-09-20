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
            : base(sm, obj, "spell3") // 文件名（不含路径与后缀）
        {
            animationName = "Spell3"; // 若需要可由 lua 覆盖
        }

        public override void OnEnter()
        {
            base.OnEnter();
            // 衔接动画（Lua 侧也可做）
            if (animator != null)
            {
                float dur = stateMachine.CurrentTransitionDuration;
                animator.CrossFade(animationName, dur, -1, 0f);
            }
        }
    }
}
