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
        }
    }
}
