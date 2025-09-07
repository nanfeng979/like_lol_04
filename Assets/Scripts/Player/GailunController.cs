using System.Collections;
using System.Collections.Generic;
using LikeLoL04.EventSystem;
using UnityEngine;
using XLua;

public class GailunController : MonoBehaviour
{
    public Animator animator;

    public Animator enemyAnimator;

    public float attackValue = 10f;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        EventBus.On("AddAttack", (args) =>
        {
            if (args.Length > 0 && args[0] is LuaTable luaTable)
            {
                var list = LuaTableToList(luaTable);
                int addAttack = list[0] is string str ? int.Parse(str) : (int)(long)list[0];
                attackValue += addAttack;
                Debug.Log("攻击力提升，当前攻击力：" + attackValue);
            }
        });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartAttack();
        }
    }

    public void StartAttack()
    {
        animator.SetTrigger("StartAttack");
    }

    public void EndAttack()
    {
        enemyAnimator.SetTrigger("BeHit");
        EffectUIController.Instance.ShowEnemyEffectUI(enemyAnimator.gameObject, 200, attackValue);
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
