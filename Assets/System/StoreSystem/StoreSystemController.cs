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

            // 加载数据，初始化视图
            JsonUtils.LoadJson<List<StoreCategoryModel>>("Assets/System/StoreSystem/StoreCategoryJson.json", categories =>
            {
                model.Categories = categories;
                view.InitializeCategories(categories);
            });
        }

        public void SetBeSelectStoreItem(StoreItemModel storeItemModel)
        {
            StoreItemView itemView = view.beSelectStoreItem.GetComponent<StoreItemView>();
            itemView.SetItem(storeItemModel);
        }
    }
}
