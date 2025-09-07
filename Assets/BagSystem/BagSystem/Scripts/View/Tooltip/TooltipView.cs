using UnityEngine;
using UnityEngine.UI;

namespace Game.Bag.View
{
    public class TooltipView : MonoBehaviour
    {
        public RectTransform rectTransform;

        public CanvasGroup canvasGroup;
        public Text nameText;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        #region Public Methods

        public void Show(Vector3 position, string displayName, Color nameColor)
        {
            SetPosition(position);
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.blocksRaycasts = false;
            }

            if (nameText != null)
            {
                nameText.text = displayName;
                nameText.color = nameColor;
            }
        }

        // 移出屏幕外
        public void Hide()
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.blocksRaycasts = false;
                SetPosition(new Vector2(-1000, -1000));
            }
        }
        
        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="anchoredPosition"></param>
        public void SetPosition(Vector2 anchoredPosition)
        {
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = anchoredPosition;
            }
        }

        #endregion
    }
}