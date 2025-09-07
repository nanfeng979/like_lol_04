using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using Game.Bag.Model;

namespace Game.Bag.Controller
{
    /// <summary>
    /// 背包控制器 - 测试与调试 API。
    /// </summary>
    public partial class BagController : MonoBehaviour
    {
        #region Public API (Testing)
        /// <summary>
        /// 重新从 JSON 读取并刷新视图。
        /// 适合在修改 StreamingAssets 后进行一次完整刷新。
        /// </summary>
        public async void ReloadFromJsonAndRefresh()
        {
            try
            {
                await InitializeAsync();
            }
            catch (Exception ex)
            {
                Debug.LogError($"ReloadFromJsonAndRefresh 错误：{ex}");
            }
        }

        /// <summary>
        /// 在现有内存数据基础上重新校验并刷新渲染。
        /// </summary>
        public async void RefreshView()
        {
            try
            {
                await ValidateBagDataBaseAsync(_bagDatabase);
                await RenderAsync(_bagDatabase);
            }
            catch (Exception ex)
            {
                Debug.LogError($"RefreshView 错误：{ex}");
            }
        }

        /// <summary>
        /// 设置网格尺寸（行与列）。
        /// </summary>
        public void SetGridSize(int rows, int columns)
        {
            _bagDatabase.grid.rows = Math.Max(1, rows);
            _bagDatabase.grid.columns = Math.Max(1, columns);
        }

        /// <summary>
        /// 尝试添加一个条目（追加到末尾）。超容量将返回 false 并发出警告。
        /// </summary>
        public bool TryAddItem(string itemId, int quantity)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            if (quantity < 1) quantity = 1;

            int capacity = Math.Max(1, _bagDatabase.grid.rows) * Math.Max(1, _bagDatabase.grid.columns);
            if (_bagDatabase.items == null)
            {
                _bagDatabase.items = new List<BagItem>();
            }

            // 获取该道具的最大叠堆
            int maxStack = 99;
            if (_itemDatabase.TryGet(itemId, out var data) && data != null)
            {
                maxStack = Math.Max(1, data.maxStack <= 0 ? 99 : data.maxStack);
            }

            bool changed = false;
            int remaining = quantity;

            // 1) 优先堆叠到已有同类未满堆的条目
            for (int i = 0; i < _bagDatabase.items.Count && remaining > 0; i++)
            {
                var entry = _bagDatabase.items[i];
                if (entry == null || !string.Equals(entry.itemId, itemId, StringComparison.Ordinal))
                    continue;

                if (entry.quantity < maxStack)
                {
                    int canStack = maxStack - entry.quantity;
                    int toAdd = Mathf.Min(canStack, remaining);
                    entry.quantity += toAdd;
                    remaining -= toAdd;
                    changed = changed || toAdd > 0;
                }
            }

            // 2) 若仍有剩余，尝试创建新堆（受容量限制）
            while (remaining > 0)
            {
                if (_bagDatabase.items.Count >= capacity)
                {
                    Debug.LogWarning($"BagController：容量已满，无法继续添加，剩余={remaining}，itemId='{itemId}'。");
                    break;
                }

                    int toAdd = Mathf.Min(maxStack, remaining);
                    var bi = new BagItem { itemId = itemId, quantity = toAdd };
                    _bagDatabase.items.Add(bi);
                remaining -= toAdd;
                changed = true;
            }

            return changed;
        }

        /// <summary>
        /// 设置指定索引条目的数量。
        /// </summary>
        public bool TrySetItemQuantityAt(int index, int quantity)
        {
            if (_bagDatabase.items == null) return false;
            if (index < 0 || index >= _bagDatabase.items.Count) return false;
            if (quantity < 1) quantity = 1;

            var bi = _bagDatabase.items[index];
            if (bi == null) return false;
            bi.quantity = quantity;
            return true;
        }

        /// <summary>
        /// 移除指定索引的条目。
        /// </summary>
        public bool TryRemoveItemAt(int index)
        {
            if (_bagDatabase.items == null) return false;
            if (index < 0 || index >= _bagDatabase.items.Count) return false;
            _bagDatabase.items.RemoveAt(index);
            return true;
        }

        /// <summary>
        /// 清空所有条目。
        /// </summary>
        public void ClearAllItems()
        {
            _bagDatabase.items?.Clear();
        }

        /// <summary>
        /// 指定索引是否有条目（边界与空引用检测）。
        /// </summary>
        public bool HasItemAt(int index)
        {
            if (_bagDatabase.items == null) return false;
            if (index < 0 || index >= _bagDatabase.items.Count) return false;
            return _bagDatabase.items[index] != null;
        }

        /// <summary>
        /// 交换两个索引的条目（支持空位与越界保护）。
        /// 成功后刷新视图。
        /// </summary>
        public void TrySwapSlots(int indexA, int indexB)
        {
            if (_bagDatabase.items == null) return;
            if (indexA == indexB) return;
            int capacity = Math.Max(1, _bagDatabase.grid.rows) * Math.Max(1, _bagDatabase.grid.columns);
            if (indexA < 0 || indexA >= capacity || indexB < 0 || indexB >= capacity)
            {
                Debug.LogWarning($"BagController：交换索引越界 A={indexA}, B={indexB}, capacity={capacity}");
                return;
            }

            // 确保列表长度与容量一致（不足的填充空位，避免越界）
            while (_bagDatabase.items.Count < capacity)
            {
                _bagDatabase.items.Add(null);
            }

            var tmp = _bagDatabase.items[indexA];
            _bagDatabase.items[indexA] = _bagDatabase.items[indexB];
            _bagDatabase.items[indexB] = tmp;
            // 同步索引
            if (_bagDatabase.items[indexA] != null) _bagDatabase.items[indexA].index = indexA;
            if (_bagDatabase.items[indexB] != null) _bagDatabase.items[indexB].index = indexB;

            RefreshView();
        }

        /// <summary>
        /// 模拟“关闭背包”：把当前快照写回 StreamingAssets 的 bag.json（覆盖）。
        /// </summary>
        public void SaveSnapshotToJson()
        {
            try
            {
                // 写出时仅输出非空项（更紧凑）
                var export = new BagDatabase
                {
                    grid = _bagDatabase.grid,
                    items = new List<BagItem>()
                };
                for (int i = 0; i < _bagDatabase.items.Count; i++)
                {
                    var bi = _bagDatabase.items[i];
                    if (bi == null) continue;
                    bi.index = i; // 确保 index 一致
                    export.items.Add(bi);
                }

                var json = JsonUtility.ToJson(export, true);
                var path = Path.Combine(Application.streamingAssetsPath, bagJsonPath);
                File.WriteAllText(path, json);
                Debug.Log($"BagController：快照已保存到 {path}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"SaveSnapshotToJson 错误：{ex}");
            }
        }
        #endregion
    }
}