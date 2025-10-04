using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Game.Bag.Model;
using Game.Bag.View;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using LikeLoL04.EventSystem;
using XLua;

namespace Game.Bag.Controller
{
    /// <summary>
    /// 背包控制器：统一管理 Model 与 View，读取 JSON 与 Addressables，并进行绑定渲染。
    /// </summary>
    public partial class BagController : MonoBehaviour
    {
        [Header("视图")]
        [Tooltip("背包视图（包含网格、槽位与背景格子容器）")]
        public BagView bagView;

        [Header("配置")]
        [Tooltip("StreamingAssets 下背包快照 JSON 相对路径")] public string bagJsonPath = "Bag/bag.json";

        private ItemDatabase _itemDatabase = new ItemDatabase();
        private BagDatabase _bagDatabase = new BagDatabase();
        private readonly List<BagSlotView> _bagSlots = new List<BagSlotView>();
        private readonly List<GameObject> _bgSlots = new List<GameObject>();
        private bool hasCreatedBgSlots = false;
        private bool _isRendering = false;

        #region Unity
        private async void Start()
        {
            if (bagView == null)
            {
                Debug.LogError("BagController：未绑定 view。");
                return;
            }

            await InitializeAsync();
        }

        #endregion

        #region Init Flow

        private async Task<BagDatabase> LoadBagDataBaseAsync()
        {
            var bagDataBase = new BagDatabase();

            try
            {
                var path = Path.Combine(Application.streamingAssetsPath, bagJsonPath);
                if (!File.Exists(path))
                {
                    Debug.LogWarning($"BagController：未找到 bag.json：{path}，使用默认值。");
                    return bagDataBase;
                }

                var json = await ReadAllTextAsync(path);
                bagDataBase = JsonUtility.FromJson<BagDatabase>(json) ?? new BagDatabase();
                NormalizeItemsByIndex(bagDataBase);
                return bagDataBase;
            }
            catch (Exception ex)
            {
                Debug.LogError($"LoadBagDataBaseAsync 错误：{ex}");
                bagDataBase = new BagDatabase();
                return bagDataBase;
            }
        }

        public async Task InitializeAsync()
        {
            // 处理数据
            _itemDatabase = await ItemDataManager.Instance.GetItemDatabaseAsync();
            _bagDatabase = await LoadBagDataBaseAsync();
            await ValidateBagDataBaseAsync(_bagDatabase);

            // 处理视图
            await RenderAsync(_bagDatabase);
        }

        private async Task ValidateBagDataBaseAsync(BagDatabase bagDatabase)
        {
            if (bagDatabase == null)
            {
                bagDatabase = new BagDatabase();
            }

            bagDatabase.grid.rows = Math.Max(1, bagDatabase.grid.rows);
            bagDatabase.grid.columns = Math.Max(1, bagDatabase.grid.columns);

            int capacity = bagDatabase.Capacity;

            // 固定长度：用 null 填充到容量长度，便于按 index 操作（分批让出主线程）
            for (int i = bagDatabase.items.Count; i < capacity; i++)
            {
                bagDatabase.items.Add(null);
                if ((i & 31) == 0) await Task.Yield(); // 每 32 次让出一帧
            }
            if (bagDatabase.items.Count > capacity)
            {
                bagDatabase.items.RemoveRange(capacity, bagDatabase.items.Count - capacity);
                await Task.Yield();
            }

            // 数量与叠堆校验（跳过空位）
            for (int i = 0; i < bagDatabase.items.Count; i++)
            {
                BagItem bi = bagDatabase.items[i];
                if (bi == null) continue;
                // 确保索引与位置一致
                bi.index = i;
                if (bi.quantity < 1) bi.quantity = 1;

                if (_itemDatabase.TryGet(bi.itemId, out var data))
                {
                    int maxStack = Math.Max(1, data.maxStack <= 0 ? 99 : data.maxStack);
                    if (bi.quantity > maxStack)
                    {
                        Debug.LogWarning($"BagController：道具 {bi.itemId} 数量 {bi.quantity} 超过最大叠堆 {maxStack}，已限制。");
                        bi.quantity = maxStack;
                    }
                }
                else if (!string.IsNullOrEmpty(bi.itemId))
                {
                    Debug.LogWarning($"BagController：未知 itemId '{bi.itemId}'，将渲染占位图。");
                }

                if ((i & 31) == 0) await Task.Yield();
            }
        }

        /// <summary>
        /// 将 items 列表重排为以 index 为主的固定长度布局；越界的丢弃并告警。
        /// </summary>
        private void NormalizeItemsByIndex(BagDatabase bagDatabase)
        {
            if (bagDatabase.items == null) return;
            int capacity = bagDatabase.Capacity;
            List<BagItem> bagItemsArranged = new List<BagItem>(capacity);
            for (int i = 0; i < capacity; i++) bagItemsArranged.Add(null);

            foreach (BagItem bagItem in bagDatabase.items)
            {
                if (bagItem == null) continue;
                if (bagItem.index < 0 || bagItem.index >= capacity)
                {
                    Debug.LogWarning($"NormalizeItemsByIndex：道具 '{bagItem.itemId}' 的索引 {bagItem.index} 超出容量 {capacity}，已丢弃。");
                    continue;
                }
                if (bagItemsArranged[bagItem.index] != null)
                {
                    Debug.LogWarning($"NormalizeItemsByIndex：检测到重复索引 {bagItem.index}，保留第一个，丢弃后来的道具 '{bagItem.itemId}'。");
                    continue;
                }
                bagItemsArranged[bagItem.index] = bagItem;
            }

            bagDatabase.items = bagItemsArranged;
        }

        private async Task RenderAsync(BagDatabase bagDatabase)
        {
            if (_isRendering) return;
            _isRendering = true;
            ClearBagSlots();
            int capacity = bagDatabase.Capacity;
            RenderBackground(capacity);
            RenderSlot(capacity);

            for (int i = 0; i < capacity; i++)
            {
                BagSlotView bagSlot = _bagSlots[i];
                BagItem bagItem = (i < bagDatabase.items.Count) ? bagDatabase.items[i] : null;
                if (bagItem == null)
                {
                    ApplySlot(bagSlot, null, 0);
                    continue;
                }

                var iconHandle = await LoadIconHandleAsync(bagItem.itemId);
                ApplySlot(bagSlot, iconHandle, bagItem.quantity);
            }
            _isRendering = false;
        }

        private void ClearBagSlots()
        {
            foreach (BagSlotView slot in _bagSlots)
            {
                if (slot != null) Destroy(slot.gameObject);
            }
            _bagSlots.Clear();
        }

        private void RenderSlot(int capacity)
        {
            if (bagView == null || bagView.slotPrefab == null || bagView.slotsRoot == null)
            {
                Debug.LogError("BagController：bagView/slotPrefab/slotsRoot 未绑定。");
                return;
            }

            _bagSlots.Clear();
            _bagSlots.AddRange(bagView.CreateSlots(capacity, this));
        }

        private void RenderBackground(int capacity)
        {
            if (hasCreatedBgSlots) return;
            hasCreatedBgSlots = true;

            if (bagView == null || bagView.backgroundPrefab == null || bagView.backgroundRoot == null)
            {
                // 背景为可选项，未配置则直接跳过
                return;
            }

            // 确保背景根节点在层级上位于 slotsRoot 之前（背后）
            if (bagView.backgroundRoot.transform.GetSiblingIndex() > bagView.slotsRoot.transform.GetSiblingIndex())
            {
                bagView.backgroundRoot.SetSiblingIndex(bagView.slotsRoot.GetSiblingIndex());
            }

            _bgSlots.Clear();
            _bgSlots.AddRange(bagView.CreateBackgrounds(capacity));
        }

        private async Task<AsyncOperationHandle<Sprite>?> LoadIconHandleAsync(string itemId)
        {
            string address = null;
            if (_itemDatabase.TryGet(itemId, out var data))
            {
                address = data.iconAddress;
            }

            if (string.IsNullOrEmpty(address))
            {
                return null;
            }

            var handle = Addressables.LoadAssetAsync<Sprite>(address);
            await handle.Task;
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogWarning($"BagController：加载图标失败：'{address}'");
                return null;
            }

            return handle;
        }

        private void ApplySlot(BagSlotView slot, AsyncOperationHandle<Sprite>? iconHandle, int quantity)
        {
            if (slot == null) return;
            if (slot.icon != null)
            {
                slot.icon.sprite = iconHandle.HasValue ? iconHandle.Value.Result : null;
                slot.icon.enabled = slot.icon.sprite != null;
            }
            slot.SetCount(quantity);
        }

        private static async Task<string> ReadAllTextAsync(string path)
        {
            using (var reader = File.OpenText(path))
            {
                return await reader.ReadToEndAsync();
            }
        }

        #endregion
    }
}
