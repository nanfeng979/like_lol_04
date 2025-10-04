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
            model.FromLOLGameObject(mainPlayer);
            ApplyModel();

            view.HideAttributeView();

            MainPlayerSkillSystem.Instance.OnUpgradeSkill += OnUpgradeSkill;
        }

        void OnEnable()
        {
            LOLClientKeyEventManager.Instance.OnToggleAttributeViewShow += view.ToggleAttributeView;
        }

        void OnDisable()
        {
            LOLClientKeyEventManager.Instance.OnToggleAttributeViewShow -= view.ToggleAttributeView;
        }

        private void ApplyModel()
        {
            view?.ApplyModel(model);
        }

        public void OnUpgradeSkill(int skillType, int skillLevel)
        {
            view.OnUpgradeSkill(skillType, skillLevel);
        }
    }
}
