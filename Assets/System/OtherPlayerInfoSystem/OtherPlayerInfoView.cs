using LikeLoL04.EventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace LikeLoL04
{
    public class OtherPlayerInfoView : MonoBehaviour
    {
        [Header("Bindings")]
        public CanvasGroup canvasGroup;
        public Text currentHpAndMaxHpText;
        public Image currentHpBarImage;
        public Image avatarImage;
        public Text attackText;

        private void Awake()
        {
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Apply(OtherPlayerInfoModel model)
        {
            if (model == null)
            {
                avatarImage.sprite = null;
                attackText.text = "";
                currentHpAndMaxHpText.text = "";
                return;
            }
            avatarImage.sprite = model.Avatar;
            attackText.text = model.Attributes != null ? model.Attributes.AttackValue.ToString() : "0";
            currentHpAndMaxHpText.text = $"{model.CurrentHp}/{model.MaxHp}";
            currentHpBarImage.fillAmount = (float)model.CurrentHp / (float)model.MaxHp;
        }

        public void UpdateHp(int currentHp, int maxHp)
        {
            currentHpAndMaxHpText.text = $"{currentHp}/{maxHp}";
            currentHpBarImage.fillAmount = (float)currentHp / (float)maxHp;
        }

        public void Show()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        public void Hide()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }
}