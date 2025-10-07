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

        public void SetBeSelectStoreItem(StoreItemData storeItemData)
        {
            view.UpdateCurrent(storeItemData);

            string beSelectItemName = storeItemData.itemName;
            StoreCraftingData storeCraftingData = model.GetCraftingDataByName(beSelectItemName);
            if (storeCraftingData != null)
            {
                List<StoreItemData> parentStoreItemDatas = GetParentStoreItemDatas(storeCraftingData.Parents);
                view.UpdateParents(parentStoreItemDatas);

                // 第一层 Children
                var layer1DataList = new List<StoreItemData>();
                var layer2DataList = new List<StoreItemData>();
                Dictionary<int, List<StoreItemData>> layer2DataDict = new Dictionary<int, List<StoreItemData>>();

                for (int i = 0; i < storeCraftingData.Children.Count; i++)
                {
                    string childName = storeCraftingData.Children[i];
                    StoreItemData childItem = StoreSystemController.Instance.GetItemByName(childName);
                    if (childItem != null)
                    {
                        layer1DataList.Add(childItem);
                        // 查找该 child 的 crafting 数据，收集第二层（即孙辈）
                        var childCraft = model.GetCraftingDataByName(childName);
                        if (childCraft != null && childCraft.Children != null)
                        {
                            foreach (var grand in childCraft.Children)
                            {
                                var grandItem = StoreSystemController.Instance.GetItemByName(grand);
                                if (grandItem != null)
                                {
                                    layer2DataList.Add(grandItem);
                                    if (!layer2DataDict.ContainsKey(i))
                                    {
                                        layer2DataDict[i] = new List<StoreItemData>();
                                    }
                                    layer2DataDict[i].Add(grandItem);
                                }
                            }
                        }
                    }
                }

                // view.MultiLayerUpdateChildren(layer1DataList, layer2DataList);
                List<float> layer1ItemPosX = view.UpdateChildRow1(layer1DataList);
                view.UpdateChildRow2(layer2DataDict, layer1ItemPosX);
            }
            else
            {
                view.UpdateParents(null);
                // view.MultiLayerUpdateChildren(null, null);
                view.UpdateChildRow1(null);
                view.UpdateChildRow2(null, null);
            }
        }

        private List<StoreItemData> GetParentStoreItemDatas(List<string> parentNames)
        {
            List<StoreItemData> parentStoreItemDatas = new List<StoreItemData>();
            foreach (var parentName in parentNames)
            {
            StoreItemData parentItem = StoreSystemController.Instance.GetItemByName(parentName);
                if (parentItem != null)
                {
                    parentStoreItemDatas.Add(parentItem);
                }
            }
            return parentStoreItemDatas;
        }
    }
}
