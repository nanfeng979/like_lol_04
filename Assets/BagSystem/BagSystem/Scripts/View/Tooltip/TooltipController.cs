using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Bag.View
{
    /// <summary>
    /// 全局唯一的 Tooltip 控制器：仅显示道具名（统一单色），跟随鼠标，越界/翻转处理。
    /// </summary>
    public class TooltipController : MonoBehaviour
    {
        [Header("引用")]
        [Tooltip("边界矩形（SlotsRoot 的父对象）")] public RectTransform boundaryRect;

        [Tooltip("Tooltip 组件")] public TooltipView tooltipView;

        [Header("设置")]
        [Tooltip("显示延迟（秒）")] public float showDelay = 0.15f;
        [Tooltip("鼠标偏移")] public Vector2 pointerOffset = new Vector2(50, 50);
        [Tooltip("文本颜色（统一单色）")] public Color nameColor = Color.white;

        private Coroutine _showRoutine;
        private bool _following;

        public static TooltipController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            HideImmediate();
            // 不阻挡射线
            foreach (var g in GetComponentsInChildren<Graphic>(true))
            {
                g.raycastTarget = false;
            }
        }

        private void OnDisable()
        {
            HideImmediate();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (!focus) Hide();
        }

        public void SetBoundary(RectTransform rect)
        {
            boundaryRect = rect;
        }

        public void ShowWithDelay(string displayName)
        {
            if (string.IsNullOrEmpty(displayName)) return;
            if (_showRoutine != null) StopCoroutine(_showRoutine);
            _showRoutine = StartCoroutine(CoShow(displayName));
        }

        public void Hide()
        {
            if (_showRoutine != null)
            {
                StopCoroutine(_showRoutine);
                _showRoutine = null;
            }
            _following = false;
            HideImmediate();
        }

        private IEnumerator CoShow(string displayName)
        {
            yield return new WaitForSeconds(showDelay);
            if (tooltipView != null)
            {
                // 将鼠标坐标转换成Canvas的本地坐标
                RectTransformUtility.ScreenPointToLocalPointInRectangle(boundaryRect, Input.mousePosition + (Vector3)pointerOffset, null, out Vector2 localPoint);
                tooltipView.Show(localPoint, displayName, nameColor);
            }
            _following = true;
        }

        private void Update()
        {
            if (!_following) return;
            FollowMouse();
        }

        private void FollowMouse()
        {
            if (tooltipView == null)
            {
                return;
            }
            
            // 将鼠标坐标转换成Canvas的本地坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(boundaryRect, Input.mousePosition + (Vector3)pointerOffset, null, out Vector2 localPoint);
            tooltipView.SetPosition(localPoint);
        }

        private void HideImmediate()
        {
            if (tooltipView != null)
            {
                tooltipView.Hide();
            }
        }
    }
}
