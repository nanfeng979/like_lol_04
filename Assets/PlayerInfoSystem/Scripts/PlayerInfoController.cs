using System.Collections;
using System.Collections.Generic;
using LikeLoL04.EventSystem;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace LikeLoL04
{
    /// <summary>
    /// 玩家信息控制器，负责管理玩家信息的显示和更新
    /// </summary>
    public class PlayerInfoController : MonoBehaviour
    {
        public static PlayerInfoController Instance;

        [Header("MVC Components")]
        [SerializeField] private PlayerInfoView view;
        private PlayerInfoModel model = new PlayerInfoModel();

        public LOLGameObject mainPlayer;

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

            if (view == null)
            {
                view = GetComponentInChildren<PlayerInfoView>(true);
            }

        }

        void Start()
        {
            // EventBus.On("AddHp", (param) =>
            // {
            //     if (param.Length > 0 && param[0] is LuaTable luaTable)
            //     {
            //         var list = LuaTableToList(luaTable);
            //         int addHp = list[0] is string str ? int.Parse(str) : (int)(long)list[0];
            //         int currentHp = int.Parse(HpText.text);
            //         currentHp += addHp;
            //         HpText.text = currentHp.ToString();
            //     }
            // });

            model.FromLOLGameObject(mainPlayer);
            ApplyModel();

            MainPlayerSkillSystem.Instance.OnUpgradeSkill += OnUpgradeSkill;
        }

        private void ApplyModel()
        {
            view?.ApplyModel(model);
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

        public void OnUpgradeSkill(int skillType, int skillLevel)
        {
            view.OnUpgradeSkill(skillType, skillLevel);
        }
    }
}
