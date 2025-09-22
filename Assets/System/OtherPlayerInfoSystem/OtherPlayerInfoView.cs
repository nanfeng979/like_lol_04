using UnityEngine;
using UnityEngine.UI;

namespace LikeLoL04
{
    public class OtherPlayerInfoView : MonoBehaviour
    {
        [Header("Bindings")]
        public CanvasGroup canvasGroup;
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
                if (avatarImage) avatarImage.sprite = null;
                if (attackText) attackText.text = "";
                return;
            }
            if (avatarImage) avatarImage.sprite = model.Avatar;
            if (attackText) attackText.text = model.AttackValue.ToString();
        }

        public void Show()
        {
            if (canvasGroup == null) return;
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        public void Hide()
        {
            if (canvasGroup == null) return;
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }
}