using System;
using System.Collections.Generic;
using XLua;

namespace LikeLoL04
{
    /// <summary>
    /// XLua 自定义生成配置。确保需要的委托、类型可以被 Lua 访问或回调。
    /// 生成步骤：菜单 XLua -> Generate Code （或对应自定义菜单）。
    /// </summary>
    public static class XLuaGenConfig
    {
        [CSharpCallLua]
        public static List<Type> CSharpCallLuaTypes = new List<Type>
        {
            typeof(Action),
            typeof(Action<float>),
            typeof(Action<string>),
            typeof(Func<string, bool>),
        };

        [LuaCallCSharp]
        public static List<Type> LuaCallCSharpTypes = new List<Type>
        {
            typeof(UnityEngine.Vector3),
            typeof(UnityEngine.Quaternion),
            typeof(UnityEngine.Transform),
            typeof(LOLGameObject),
            typeof(StateMachineV2),
        };
    }
}
