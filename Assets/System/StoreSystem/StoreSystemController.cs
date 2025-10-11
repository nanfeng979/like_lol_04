using System.Collections.Generic;
using LikeLoL04.EventSystem;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 商店系统控制器，负责管理商店的视图和数据模型。
    /// </summary>
    public class StoreSystemController
        : MVC_Controller<StoreSystemController, StoreSystemView, StoreSystemModel>
    {
        // protected override bool ViewDefaultActive => false;

        void Start()
        {
            LOLClientKeyEventManager.Instance.OnToggleStoreSystemShow += ToggleShow;
            LOLClientKeyEventManager.Instance.OnToggleStoreSystemShow += ToggleShowCraftingView;

            // 加载数据，初始化视图
            JsonUtils.LoadJson<List<StoreItemData>>("Assets/System/StoreSystem/StoreCategoryJson.json", storeItemDatas =>
            {
                model.StoreItemDatas = storeItemDatas;
                view.InitializeCategories(model.CategoryDict);
            });
        }

        void OnDestroy()
        {
            LOLClientKeyEventManager.Instance.OnToggleStoreSystemShow -= ToggleShow;
            LOLClientKeyEventManager.Instance.OnToggleStoreSystemShow -= ToggleShowCraftingView;
        }

        public StoreItemData GetItemByName(string itemName)
        {
            return model.GetItemByName(itemName);
        }

        private void ToggleShowCraftingView()
        {
            StoreCraftingController.Instance.ToggleShow();
        }
    }
}
