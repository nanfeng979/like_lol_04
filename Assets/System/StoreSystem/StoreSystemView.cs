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

        public void InitializeCategories(Dictionary<string, List<StoreItemData>> categories)
        {
            // Sort categories: "Initial" first, then "legend", then others alphabetically
            var sortedCategories = new List<KeyValuePair<string, List<StoreItemData>>>();

            List<string> priorityOrder = new List<string> { "Initial", "Legend" };
            foreach (var category in priorityOrder)
            {
                if (categories.ContainsKey(category))
                {
                    sortedCategories.Add(new KeyValuePair<string, List<StoreItemData>>(category, categories[category]));
                }
            }

            foreach (var category in sortedCategories)
            {
                GameObject categoryGO = Instantiate(categoryPrefab, itemsContent);
                string translatedName = categoryNameTranslate(category.Key);
                categoryGO.name = translatedName;
                StoreCategoryView categoryView = categoryGO.GetComponent<StoreCategoryView>();
                categoryView.SetTitle(translatedName);
                categoryView.AddItems(category.Value);
            }
        }

        private string categoryNameTranslate(string category)
        {
            return category switch
            {
                "Initial" => "初始",
                "Legend" => "传说",
                _ => category
            };
        }
    }
}
