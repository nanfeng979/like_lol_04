using System.Collections;
using System.Collections.Generic;
using LikeLoL04.EventSystem;
using UnityEngine;
using UnityEngine.UI;
using XLua;

public class PlayerInfoController : MonoBehaviour
{
    public static PlayerInfoController Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
    }

    void Start()
    {
        EventBus.On("AddHp", (param) =>
        {
            if (param.Length > 0 && param[0] is LuaTable luaTable)
            {
                var list = LuaTableToList(luaTable);
                int addHp = list[0] is string str ? int.Parse(str) : (int)(long)list[0];
                int currentHp = int.Parse(HpText.text);
                currentHp += addHp;
                HpText.text = currentHp.ToString();
            }
        });
    }

    public Text HpText;

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
