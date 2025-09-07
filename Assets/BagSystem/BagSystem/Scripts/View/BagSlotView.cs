using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Game.Bag.Controller;
using Game.Bag.View;

namespace Game.Bag.View
{
    /// <summary>
    /// 单个格子的显示：图标与数量文本（背景由 BagView 统一渲染）。
    /// 支持拖拽交换：视图 -> 控制器 -> 模型 -> 控制器 -> 视图。
    /// </summary>
    public class BagSlotView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("显示")]
        [Tooltip("道具图标 Image")]
        public Image icon;
        [Tooltip("数量文本（数量<=1 时隐藏）")]
        public Text countText;

        [Header("运行时绑定")]
        [Tooltip("该格子在网格中的序号（行优先）")]
        public int slotIndex;                           // 该格子在网格中的序号（行优先）
        [Tooltip("背包控制器引用（由控制器在实例化时绑定）")]
        public BagController controller;                // 控制器引用

        private CanvasGroup _canvasGroup;               // 拖拽时禁用射线阻挡
        private Image _raycastCatcher;                  // 透明射线接收器（保证空格可接收 OnDrop）
        private bool _pointerInside;

        public void SetCount(int quantity)
        {
            if (countText == null)
            {
                return;
            }

            countText.text = quantity > 1 ? quantity.ToString() : string.Empty;
        }

        public void Bind(int index, BagController ctrl)
        {
            slotIndex = index;
            controller = ctrl;
        }

        #region 拖拽与放置
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            // 保证空格也能接收 OnDrop：在本节点挂一个透明 Image 作为射线接收器
            _raycastCatcher = GetComponent<Image>();
            if (_raycastCatcher == null)
            {
                _raycastCatcher = gameObject.AddComponent<Image>();
            }
            _raycastCatcher.color = new Color(0, 0, 0, 0); // 完全透明
            _raycastCatcher.raycastTarget = true;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (controller == null) return;
            // 空格子不允许拖拽
            if (!controller.HasItemAt(slotIndex)) return;

            _canvasGroup.blocksRaycasts = false; // 让下层目标能接收到 OnDrop
            _canvasGroup.alpha = 0.7f;
            // 拖拽开始立即隐藏 Tooltip
            TooltipController.Instance?.Hide();
        }

        public void OnDrag(PointerEventData eventData)
        {
            // MVP 阶段：不做拖影或跟随，仅提供交换逻辑
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;
            // 拖拽结束时不自动显示 Tooltip，等待重新进入
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (controller == null) return;
            var sourceObj = eventData.pointerDrag;
            if (sourceObj == null) return;

            var sourceSlot = sourceObj.GetComponent<BagSlotView>();
            if (sourceSlot == null) return;

            if (sourceSlot.slotIndex == slotIndex) return;

            controller.TrySwapSlots(sourceSlot.slotIndex, slotIndex);
        }
        #endregion

        #region Tooltip 触发
        public void OnPointerEnter(PointerEventData eventData)
        {
            _pointerInside = true;
            if (controller == null) return;
            if (!controller.HasItemAt(slotIndex)) return; // 空物品不显示
            var tooltip = TooltipController.Instance;
            if (tooltip == null) return;
            tooltip.SetBoundary(controller.GetSlotsBoundary());
            string name = controller.GetDisplayNameAtIndex(slotIndex);
            tooltip.ShowWithDelay(name);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _pointerInside = false;
            TooltipController.Instance?.Hide();
        }

        private void OnDisable()
        {
            if (_pointerInside)
            {
                TooltipController.Instance?.Hide();
                _pointerInside = false;
            }
        }
        #endregion

    }
}
