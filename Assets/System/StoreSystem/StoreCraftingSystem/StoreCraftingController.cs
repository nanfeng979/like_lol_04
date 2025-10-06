using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 商店合成系统控制器
    /// </summary>
    public class StoreCraftingController
            : MVC_Controller<StoreCraftingController, StoreCraftingView, StoreCraftingModel>
    {

        private List<StoreCraftingModel> m_craftingTable = new List<StoreCraftingModel>();

        void Start()
        {
            JsonUtils.LoadJson<List<StoreCraftingModel>>("Assets/System/StoreSystem/StoreCraftingSystem/StoreCraftingTable.json", craftings =>
            {
                m_craftingTable = craftings;
            });
        }

        public void SetBeSelectStoreItem(StoreItemModel storeItemModel)
        {
            StoreItemView beSelectStoreItem = view.beSelectStoreItem.GetComponent<StoreItemView>();
            beSelectStoreItem.SetItem(storeItemModel);

            string beSelectItemName = storeItemModel.itemName;
            StoreCraftingModel craftingModel = m_craftingTable.Find(c => c.Name == beSelectItemName);
            if (craftingModel != null)
            {
                List<StoreItemModel> parents = new List<StoreItemModel>();
                foreach (var parentName in craftingModel.Parents)
                {
                    StoreItemModel parentItem = StoreSystemController.Instance.GetItemByName(parentName);
                    if (parentItem != null)
                    {
                        parents.Add(parentItem);
                    }
                }
                view.UpdateParents(parents);
            }
            else
            {
                view.UpdateParents(null);
            }
        }
    }
}
