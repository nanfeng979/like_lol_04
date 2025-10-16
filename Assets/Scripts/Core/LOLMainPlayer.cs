using System.Collections.Generic;
using LikeLoL04.EventSystem;
using UnityEngine;
using XLua;

namespace LikeLoL04
{
    public class LOLMainPlayer : Singleton<LOLMainPlayer>
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

            EventBus.On("AddAttack", (param) =>
            {
                if (param.Length > 0 && param[0] is LuaTable luaTable)
                {
                    var list = LuaTableToList(luaTable);
                    int addAttack = list[0] is string str ? int.Parse(str) : (int)(long)list[0];
                    int currentAttack = mainPlayer.GetAttackValue();
                    currentAttack += addAttack;
                    mainPlayer.SetAttackValue(currentAttack);
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

        private int m_gold = 500;
        public int Gold { get => m_gold; set => m_gold = value; }

        private int m_level = 1;
        public int Level { get => m_level; set => m_level = value; }
        private int m_maxLevel = 18;
        public int MaxLevel { get => m_maxLevel; set => m_maxLevel = value; }

        // 可以升级技能的点数
        private int m_skillUpgradePoint = 1;
        public int SkillUpgradePoint { get => m_skillUpgradePoint; set => m_skillUpgradePoint = value; }

        public void OnUpgradeSkill()
        {
            if (Level < MaxLevel)
            {
                Level++;
                SkillUpgradePoint++;
            }
        }

        public bool CanUpgradeSkill()
        {
            return SkillUpgradePoint > 0;
        }

        public void UpgradeSkill()
        {
            if (CanUpgradeSkill())
            {
                SkillUpgradePoint--;
            }
        }
    }
}

