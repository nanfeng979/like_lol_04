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
                // attackText.text = "";
                // currentHpAndMaxHpText.text = "";
                return;
            }
            avatarImage.sprite = model.Avatar;
            // attackText.text = model.AttackValue.ToString();
            // currentHpAndMaxHpText.text = $"{model.CurrentHp}/{model.MaxHp}";
            // currentHpBarImage.fillAmount = (float)model.CurrentHp / (float)model.MaxHp;
        }

        public void UpdateHp(int currentHp, int maxHp)
        {
            // currentHpAndMaxHpText.text = $"{currentHp}/{maxHp}";
            // currentHpBarImage.fillAmount = (float)currentHp / (float)maxHp;
        }

        public Image Speell1Image;
        public Image Speell2Image;
        public Image Speell3Image;
        public Image Speell4Image;

        public void OnUpgradeSkill(int skillIndex, int skillLevel)
        {
            switch (skillIndex)
            {
                case 1:
                    Speell1Image.enabled = false;
                    break;
                case 2:
                    Speell2Image.enabled = false;
                    break;
                case 3:
                    Speell3Image.enabled = false;
                    break;
                case 4:
                    Speell4Image.enabled = false;
                    break;
                default:
                    break;
            }
        }
    }
}