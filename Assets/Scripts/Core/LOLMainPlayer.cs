using System.Collections.Generic;
using LikeLoL04.EventSystem;
using UnityEngine;
using XLua;

namespace LikeLoL04
{
    public class LOLMainPlayer : MonoBehaviour
    {

        public LOLGameObject mainPlayer;

        protected void Start()
        {
            mainPlayer.Camp = new Camp { Type = CampType.Blue };

            EventBus.On("AddHp", (param) =>
            {
                if (param.Length > 0 && param[0] is LuaTable luaTable)
                {
                    var list = LuaTableToList(luaTable);
                    int addHp = list[0] is string str ? int.Parse(str) : (int)(long)list[0];
                    int currentHp = mainPlayer.GetHealthPercent();
                    currentHp += addHp;
                    mainPlayer.SetHealthPercent(currentHp);
                    Debug.Log($"AddHp: {addHp}, CurrentHp: {currentHp}");
                }
            });
        }

        public static List<object> LuaTableToList(LuaTable table)
        {
            var list = new List<object>();
            int index = 1;

            while (true)
            {
                object value;
                table.Get(index, out value);
                if (value != null)
                {
                    list.Add(value);
                    index++;
                }
                else
                {
                    break;
                }
            }

            return list;
        }

    }
}

