using LikeLoL04.EventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace LikeLoL04
{
    public class PlayerInfoView : MonoBehaviour
    {
        [Header("Bindings")]
        public CanvasGroup canvasGroup;
        // public Text currentHpAndMaxHpText;
        // public Image currentHpBarImage;
        public Image avatarImage;
        // public Text attackText;

        private void Awake()
        {
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        }

        public void ApplyModel(PlayerInfoModel model)
        {
            if (model == null)
            {
                avatarImage.sprite = null;

                AttackText.text = "0";
                MagicText.text = "0";
                ArmorText.text = "0";
                MagicResistText.text = "0";
                AttackSpeedText.text = "0";
                CoolDownText.text = "0";
                CriticalStrikeText.text = "0%";
                MoveSpeedText.text = "0";
                return;
            }
            avatarImage.sprite = model.Avatar;

            AttackText.text = model.Attributes != null ? model.Attributes.AttackValue.ToString() : "0";
            MagicText.text = model.Attributes != null ? model.Attributes.MagicValue.ToString() : "0";
            ArmorText.text = model.Attributes != null ? model.Attributes.ArmorValue.ToString() : "0";
            MagicResistText.text = model.Attributes != null ? model.Attributes.MagicResistValue.ToString() : "0";
            AttackSpeedText.text = model.Attributes != null ? model.Attributes.AttackSpeedValue.ToString() : "0";
            CoolDownText.text = model.Attributes != null ? model.Attributes.CoolDownValue.ToString() : "0";
            CriticalStrikeText.text = model.Attributes != null ? model.Attributes.CriticalStrikeValue.ToString() + "%" : "0%";
            MoveSpeedText.text = model.Attributes != null ? model.Attributes.MoveSpeedValue.ToString() : "0";
        }

        public void UpdateHp(int currentHp, int maxHp)
        {
            // currentHpAndMaxHpText.text = $"{currentHp}/{maxHp}";
            // currentHpBarImage.fillAmount = (float)currentHp / (float)maxHp;
        }

        public Text AttackText;
        public Text MagicText;
        public Text ArmorText;
        public Text MagicResistText;
        public Text AttackSpeedText;
        public Text CoolDownText;
        public Text CriticalStrikeText;
        public Text MoveSpeedText;

        public RectTransform AttributeViewRectTransform;

        public void ToggleAttributeView()
        {
            if (AttributeViewRectTransform != null)
            {
                bool isActive = AttributeViewRectTransform.gameObject.activeSelf;
                AttributeViewRectTransform.gameObject.SetActive(!isActive);
            }
        }

        public void HideAttributeView()
        {
            if (AttributeViewRectTransform != null)
            {
                AttributeViewRectTransform.gameObject.SetActive(false);
            }
        }
        
        public void UpdateAttackValue(int attackValue)
        {
            AttackText.text = attackValue.ToString();
        }
    }
}