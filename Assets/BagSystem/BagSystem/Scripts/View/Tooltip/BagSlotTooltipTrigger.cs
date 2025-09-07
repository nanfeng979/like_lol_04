using UnityEngine;
using UnityEngine.EventSystems;
using Game.Bag.Controller;

namespace Game.Bag.View
{
    /// <summary>
    /// 槽位悬浮提示触发器：挂在 BagSlotView 所在的对象上。
    /// </summary>
    [RequireComponent(typeof(BagSlotView))]
    public class BagSlotTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IBeginDragHandler
    {
        private BagSlotView _slot;

        private void Awake()
        {
            _slot = GetComponent<BagSlotView>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (BagController.Instance == null || TooltipController.Instance == null) return;
            if (!BagController.Instance.HasItemAt(_slot.slotIndex)) return; // 空物品不显示

            TooltipController.Instance.SetBoundary(BagController.Instance.SlotsBoundary);
            string name = BagController.Instance.GetDisplayNameAt(_slot.slotIndex);
            TooltipController.Instance.ShowWithDelay(name);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipController.Instance?.Hide(); // 无延迟隐藏
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            // 仅跟随逻辑由 TooltipController.Update 处理，这里无需每帧转发
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            TooltipController.Instance?.Hide();
        }
    }
}
