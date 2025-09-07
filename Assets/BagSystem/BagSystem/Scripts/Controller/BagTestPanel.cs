using UnityEngine;

namespace Game.Bag.Controller
{
    /// <summary>
    /// 简易测试面板：提供可供 Button 绑定的公开方法，模拟增删改与刷新。
    /// 将本组件挂到一个 GameObject，并把 controller 引用拖拽进来。
    /// </summary>
    public class BagTestPanel : MonoBehaviour
    {
        public BagController controller;

        [Header("Defaults for Testing")]
        public string addItemId = "potion";
        public int addQuantity = 1;
        public int targetIndex = 0;
        public int setQuantity = 5;
        public int setRows = 4;
        public int setColumns = 5;

        #region Button Hooks
        public void Btn_ReloadFromJson()
        {
            controller?.ReloadFromJsonAndRefresh();
        }

        public void Btn_Refresh()
        {
            controller?.RefreshView();
        }

        public void Btn_AddItem()
        {
            if (controller == null) return;
            if (controller.TryAddItem(addItemId, addQuantity))
            {
                controller.RefreshView();
            }
        }

        public void Btn_SetItemQuantityAt()
        {
            if (controller == null) return;
            if (controller.TrySetItemQuantityAt(targetIndex, setQuantity))
            {
                controller.RefreshView();
            }
        }

        public void Btn_RemoveItemAt()
        {
            if (controller == null) return;
            if (controller.TryRemoveItemAt(targetIndex))
            {
                controller.RefreshView();
            }
        }

        public void Btn_ClearAll()
        {
            if (controller == null) return;
            controller.ClearAllItems();
            controller.RefreshView();
        }

        public void Btn_SetGridSize()
        {
            if (controller == null) return;
            controller.SetGridSize(setRows, setColumns);
            controller.RefreshView();
        }

        public void Btn_SaveSnapshotToJson()
        {
            controller?.SaveSnapshotToJson();
        }
        #endregion
    }
}
