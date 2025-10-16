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

            EventBus.On("AttackValueChanged", (param) =>
            {
                if (param.Length > 0 && param[0] is int attackValue)
                {
                    model.UpdateAttackValue(attackValue);
                    m_ApplyAttackValue();
                }
            });
        }

        void OnDestroy()
        {
            EventBus.Off("AttackValueChanged");
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

        private void m_ApplyAttackValue()
        {
            view?.UpdateAttackValue(model?.Attributes?.AttackValue ?? 0);
        }
    }
}
