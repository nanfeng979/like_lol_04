using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 商店系统视图
    /// </summary>
    public class StoreSystemView : MVC_View
    {
        [SerializeField]
        private RectTransform itemsContent;

        [SerializeField]
        private GameObject categoryPrefab;

        void Start()
        {
            InitializeCategories();
        }

        private void InitializeCategories()
        {

            JsonUtils.LoadJson<List<StoreCategoryModel>>("Assets/System/StoreSystem/StoreCategoryJson.json", categories =>
            {
                foreach (var category in categories)
                {
                    GameObject categoryGO = Instantiate(categoryPrefab, itemsContent);
                    categoryGO.name = category.title;
                    StoreCategoryView categoryView = categoryGO.GetComponent<StoreCategoryView>();
                    categoryView.SetTitle(category.title);
                    categoryView.AddItems(category.itemList);
                }
            });
        }
    }
}
