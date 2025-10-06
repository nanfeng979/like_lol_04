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

        void Start()
        {
            JsonUtils.LoadJson<List<StoreCraftingData>>("Assets/System/StoreSystem/StoreCraftingSystem/StoreCraftingTable.json", craftings =>
            {
                model.CraftingTable = craftings;
            });
        }

        public void SetBeSelectStoreItem(StoreItemModel storeItemModel)
        {
            StoreItemView beSelectStoreItem = view.beSelectStoreItem.GetComponent<StoreItemView>();
            beSelectStoreItem.SetItem(storeItemModel);

            string beSelectItemName = storeItemModel.itemName;
            StoreCraftingData storeCraftingModelData = model.GetCraftingDataByName(beSelectItemName);
            if (storeCraftingModelData != null)
            {
                List<StoreItemModel> parents = new List<StoreItemModel>();
                foreach (var parentName in storeCraftingModelData.Parents)
                {
                    StoreItemModel parentItem = StoreSystemController.Instance.GetItemByName(parentName);
                    if (parentItem != null)
                    {
                        parents.Add(parentItem);
                    }
                }
                view.UpdateParents(parents);
                // 第一层 Children
                var layer1Children = new List<StoreItemModel>();
                var layer2Children = new List<StoreItemModel>();
                foreach (var childName in storeCraftingModelData.Children)
                {
                    StoreItemModel childItem = StoreSystemController.Instance.GetItemByName(childName);
                    if (childItem != null)
                    {
                        layer1Children.Add(childItem);
                        // 查找该 child 的 crafting 数据，收集第二层（即孙辈）
                        var childCraft = model.GetCraftingDataByName(childName);
                        if (childCraft != null && childCraft.Children != null)
                        {
                            foreach (var grand in childCraft.Children)
                            {
                                var grandItem = StoreSystemController.Instance.GetItemByName(grand);
                                if (grandItem != null)
                                {
                                    layer2Children.Add(grandItem);
                                }
                            }
                        }
                    }
                }
                view.MultiLayerUpdateChildren(layer1Children, layer2Children);
            }
            else
            {
                view.UpdateParents(null);
                view.MultiLayerUpdateChildren(null, null);
            }
        }
    }
}
