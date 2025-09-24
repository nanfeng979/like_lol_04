using LikeLoL04.EventSystem;
using UnityEngine;

namespace LikeLoL04
{
    public class OtherPlayerInfoController : MonoBehaviour
    {
        public static OtherPlayerInfoController Instance;

        [Header("MVC Components")]
        [SerializeField] private OtherPlayerInfoView view;
        private OtherPlayerInfoModel model = new OtherPlayerInfoModel();

        [Header("Timing")]
        [SerializeField] private float autoHideSeconds = 3f; // X 秒后自动隐藏；<=0 关闭
        private float hideAtTime = -1f;
        private bool isVisible = false;

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
                view = GetComponentInChildren<OtherPlayerInfoView>(true);
            }
            HideImmediate();

            m_ListenLOLGameObjectHpChange();
        }

        void Start()
        {
            ClickInteractionManager.Instance.OnLeftClickWithTarget += ShowOtherPlayerInfo;
            ClickInteractionManager.Instance.OnRightClickWithTarget += ShowOtherPlayerInfo;
        }

        void Update()
        {
            if (isVisible && autoHideSeconds > 0f && hideAtTime > 0f)
            {
                if (Time.time >= hideAtTime)
                {
                    // Hide();
                }
            }
        }

        void OnDestroy()
        {
            ClickInteractionManager.Instance.OnLeftClickWithTarget -= ShowOtherPlayerInfo;
            ClickInteractionManager.Instance.OnRightClickWithTarget -= ShowOtherPlayerInfo;

            m_UnlistenLOLGameObjectHpChange();
        }

        private void ShowOtherPlayerInfo(LOLGameObject target)
        {
            if (target != null)
            {
                model.FromLOLGameObject(target);
                ApplyModel();
                Show();
            }
        }
        private void ApplyModel()
        {
            view?.Apply(model);
        }

        public void Show()
        {
            if (view == null) return;
            view.Show();
            isVisible = true;
            if (autoHideSeconds > 0f)
            {
                hideAtTime = Time.time + autoHideSeconds;
            }
        }

        public void Hide()
        {
            if (view == null) return;
            view.Hide();
            isVisible = false;
            hideAtTime = -1f;
        }

        private void HideImmediate()
        {
            if (view == null) return;
            view.Hide();
            isVisible = false;
            hideAtTime = -1f;
        }

        private void m_ListenLOLGameObjectHpChange()
        {
            EventBus.On("HealthChanged", m_LOLGameObjectHpChangeAction);
        }

        private void m_UnlistenLOLGameObjectHpChange()
        {
            EventBus.Off("HealthChanged", m_LOLGameObjectHpChangeAction);
        }

        private void m_LOLGameObjectHpChangeAction(object[] args)
        {
            if (args.Length == 3 && args[0] is string name && args[1] is int currentHp && args[2] is int maxHp)
            {
                bool sourceNotNull = model.Source != null;
                bool dataNotNull = sourceNotNull && model.Source.Data != null;
                bool nameMatches = dataNotNull && model.Source.Data.Name == name;

                if (nameMatches)
                {
                    model.UpdateHp(currentHp, maxHp);
                    view?.UpdateHp(currentHp, maxHp);
                }
            }
        }
    }
}
